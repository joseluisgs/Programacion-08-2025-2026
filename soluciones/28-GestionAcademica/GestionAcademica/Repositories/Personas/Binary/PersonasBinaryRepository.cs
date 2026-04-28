using System.Text;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers.Personas;
using GestionAcademica.Models.Personas;
using GestionAcademica.Repositories.Personas.Base;
using Serilog;

namespace GestionAcademica.Repositories.Personas.Binary;

public class PersonasBinaryRepository : IPersonasRepository {
    private const string DataFileName = "academia-db.dat";
    private const string IndexFileName = "academia-db.idx";
    private const string FragmentsFileName = "academia-db.frag";

    private const string MagicNumber = "ACAD";
    private const string MagicNumberIndex = "ACDI";
    private const string MagicNumberFragments = "ACFR";
    private const int CurrentVersion = 1;

    private static readonly Lazy<PersonasBinaryRepository> Lazy = new(() => new PersonasBinaryRepository());

    private readonly Dictionary<string, int> _dniIndex = new();
    private readonly Dictionary<int, long> _idIndex = new();
    private readonly Dictionary<int, long> _sizeIndex = new();
    private readonly Dictionary<long, long> _fragments = new();

    private readonly ILogger _logger = Log.ForContext<PersonasBinaryRepository>();
    private int _idCounter;

    private PersonasBinaryRepository() {
        _logger.Debug("Inicializando el motor de persistencia binaria.");
        EnsureDataFolder();
        InitializeFile();
        LoadIndex();
        LoadFragments();
    }

    public static PersonasBinaryRepository Instance => Lazy.Value;

    private string DataFilePath => Path.Combine(Configuracion.DataFolder, DataFileName);
    private string IndexFilePath => Path.Combine(Configuracion.DataFolder, IndexFileName);
    private string FragmentsFilePath => Path.Combine(Configuracion.DataFolder, FragmentsFileName);

    private void InitializeFile() {
        try {
            if (!File.Exists(DataFilePath)) {
                _logger.Information("Creando base de datos binaria v{CurrentVersion}.", CurrentVersion);
                using var stream = File.Create(DataFilePath);
                using var writer = new BinaryWriter(stream, Encoding.UTF8);
                writer.Write(Encoding.ASCII.GetBytes(MagicNumber));
                writer.Write(CurrentVersion);
            }
            else {
                using var stream = File.OpenRead(DataFilePath);
                using var reader = new BinaryReader(stream, Encoding.UTF8);
                if (stream.Length < 8) {
                    throw new InvalidOperationException("Archivo corrupto o demasiado pequeño.");
                }
                var magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
                if (magic != MagicNumber) {
                    throw new InvalidOperationException("La firma del archivo no coincide con nuestra base de datos.");
                }
                var version = reader.ReadInt32();
                if (version != CurrentVersion) {
                    throw new InvalidOperationException($"Versión de archivo no soportada ({version}).");
                }
            }
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al inicializar el almacenamiento binario.");
            throw;
        }
    }

    private long FindFreeSpace(long requiredSize) {
        var holePos = _fragments.Keys.FirstOrDefault(pos => _fragments[pos] >= requiredSize, -1);
        if (holePos == -1) return -1;

        var holeSize = _fragments[holePos];
        _fragments.Remove(holePos);

        if (holeSize > requiredSize + 10) {
            long remainingPos = holePos + requiredSize;
            long remainingSize = holeSize - requiredSize;
            _fragments[remainingPos] = remainingSize;
        }

        return holePos;
    }

    private long WritePersona(Persona persona, long position = -1) {
        var dto = persona.ToDto();
        using var stream = File.Open(DataFilePath, FileMode.Open, FileAccess.Write);
        
        if (position == -1) {
            stream.Seek(0, SeekOrigin.End);
        }
        else {
            stream.Seek(position, SeekOrigin.Begin);
        }
        
        var actualPosition = stream.Position;
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        writer.Write(dto.Id);
        writer.Write(dto.Dni);
        writer.Write(dto.Nombre);
        writer.Write(dto.Apellidos);
        writer.Write(dto.Tipo);
        writer.Write(dto.Experiencia ?? string.Empty);
        writer.Write(dto.Especialidad ?? string.Empty);
        writer.Write(dto.Ciclo);
        writer.Write(dto.Curso ?? string.Empty);
        writer.Write(dto.Calificacion ?? string.Empty);
        writer.Write(dto.CreatedAt);
        writer.Write(dto.UpdatedAt);
        writer.Write(dto.IsDeleted);

        return actualPosition;
    }

    private Persona? ReadPersonaAt(long position) {
        if (!File.Exists(DataFilePath)) return null;
        
        using var stream = File.OpenRead(DataFilePath);
        stream.Seek(position, SeekOrigin.Begin);
        using var reader = new BinaryReader(stream, Encoding.UTF8);
        return ReadPersona(reader);
    }

    private Persona? ReadPersona(BinaryReader reader) {
        try {
            var dto = new PersonaDto(
                reader.ReadInt32(), 
                reader.ReadString(), 
                reader.ReadString(), 
                reader.ReadString(),
                reader.ReadString(), 
                reader.ReadString(), 
                reader.ReadString(), 
                reader.ReadString(),
                reader.ReadString(), 
                reader.ReadString(), 
                reader.ReadString(), 
                reader.ReadString(),
                reader.ReadBoolean()
            );
            return dto.ToModel();
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al deserializar un registro.");
            return null;
        }
    }

    private long GetPersonaSize(Persona persona) {
        var dto = persona.ToDto();
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms, Encoding.UTF8);
        
        writer.Write(dto.Id); 
        writer.Write(dto.Dni); 
        writer.Write(dto.Nombre); 
        writer.Write(dto.Apellidos);
        writer.Write(dto.Tipo); 
        writer.Write(dto.Experiencia ?? ""); 
        writer.Write(dto.Especialidad ?? "");
        writer.Write(dto.Ciclo); 
        writer.Write(dto.Curso ?? ""); 
        writer.Write(dto.Calificacion ?? "");
        writer.Write(dto.CreatedAt); 
        writer.Write(dto.UpdatedAt); 
        writer.Write(dto.IsDeleted);
        
        return ms.Length;
    }

    public IEnumerable<Persona> GetAll() {
        _logger.Debug("Consultando repositorio binario.");
        var personas = new List<Persona>();
        
        if (!File.Exists(DataFilePath)) return personas;

        using var stream = File.OpenRead(DataFilePath);
        using var reader = new BinaryReader(stream, Encoding.UTF8);

        foreach (var kvp in _idIndex.OrderBy(x => x.Key)) {
            stream.Seek(kvp.Value, SeekOrigin.Begin);
            if (ReadPersona(reader) is {} persona) {
                personas.Add(persona);
            }
        }

        return personas.Where(p => !p.IsDeleted);
    }

    public Persona? GetById(int id) {
        if (_idIndex.TryGetValue(id, out var position)) {
            return ReadPersonaAt(position);
        }
        return null;
    }

    public Persona? GetByDni(string dni) {
        if (_dniIndex.TryGetValue(dni, out var id)) {
            return GetById(id);
        }
        return null;
    }

    public bool ExisteDni(string dni) {
        return _dniIndex.ContainsKey(dni);
    }

    public Persona? Create(Persona entity) {
        if (ExisteDni(entity.Dni)) return null;

        var id = ++_idCounter;
        var newPersona = entity with { 
            Id = id, 
            CreatedAt = DateTime.UtcNow, 
            UpdatedAt = DateTime.UtcNow, 
            IsDeleted = false 
        };

        try {
            var size = GetPersonaSize(newPersona);
            var foundPosition = FindFreeSpace(size); 
            
            var position = WritePersona(newPersona, foundPosition);
            
            _idIndex[id] = position;
            _dniIndex[newPersona.Dni] = id;
            _sizeIndex[id] = size; 

            SaveIndex();
            if (foundPosition != -1) SaveFragments();
            
            return newPersona;
        }
        catch {
            throw new InvalidOperationException("No se pudo completar la creación del registro binario.");
        }
    }

    public Persona? Update(int id, Persona entity) {
        if (_idIndex.TryGetValue(id, out var oldPosition) == false) return null;
        
        var oldSize = _sizeIndex[id];
        var existingDni = _dniIndex.FirstOrDefault(x => x.Value == id).Key;
        
        // Si cambió el DNI, verificar que no exista en otra persona
        if (entity.Dni != existingDni && _dniIndex.TryGetValue(entity.Dni, out var otroId) && otroId != id) {
            _logger.Warning("No se puede actualizar persona con id {Id} porque el DNI {Dni} ya está en uso por otra persona", id, entity.Dni);
            return null;
        }
        
        var updatedPersona = entity with { 
            Id = id, 
            UpdatedAt = DateTime.UtcNow, 
            IsDeleted = false 
        };

        try {
            var newSize = GetPersonaSize(updatedPersona);
            long finalPosition;

            if (newSize <= oldSize) {
                finalPosition = WritePersona(updatedPersona, oldPosition);
                if (oldSize > newSize + 10) {
                    _fragments[oldPosition + newSize] = oldSize - newSize;
                    SaveFragments();
                }
            }
            else {
                _fragments[oldPosition] = oldSize;
                var foundPosition = FindFreeSpace(newSize);
                finalPosition = WritePersona(updatedPersona, foundPosition);
                SaveFragments();
            }

            _idIndex[id] = finalPosition;
            _sizeIndex[id] = newSize;
            
            if (existingDni != updatedPersona.Dni) { 
                _dniIndex.Remove(existingDni); 
                _dniIndex[updatedPersona.Dni] = id; 
            }
            
            SaveIndex();
            return updatedPersona;
        }
        catch {
            throw new InvalidOperationException("Error crítico al actualizar los datos en disco.");
        }
    }

    public Persona? Delete(int id) {
        if (_idIndex.TryGetValue(id, out var position) == false) return null;
        
        if (ReadPersonaAt(position) is not {} persona) return null;

        var deletedPersona = persona with { 
            IsDeleted = true, 
            UpdatedAt = DateTime.UtcNow 
        };

        WritePersona(deletedPersona, position);
        return deletedPersona;
    }

    public bool DeleteAll() {
        _idIndex.Clear(); 
        _dniIndex.Clear(); 
        _fragments.Clear(); 
        _idCounter = 0;
        
        try {
            if (File.Exists(DataFilePath)) File.Delete(DataFilePath);
            
            InitializeFile(); 
            SaveIndex(); 
            SaveFragments();
            return true;
        }
        catch { 
            return false; 
        }
    }

    private void SaveIndex() {
        using var stream = File.Create(IndexFilePath);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);
        
        writer.Write(Encoding.ASCII.GetBytes(MagicNumberIndex));
        writer.Write(CurrentVersion);
        writer.Write(_idCounter);
        
        writer.Write(_idIndex.Count);
        foreach (var kvp in _idIndex) { 
            writer.Write(kvp.Key); 
            writer.Write(kvp.Value); 
        }

        writer.Write(_dniIndex.Count);
        foreach (var kvp in _dniIndex) { 
            writer.Write(kvp.Key); 
            writer.Write(kvp.Value); 
        }

        writer.Write(_sizeIndex.Count);
        foreach (var kvp in _sizeIndex) { 
            writer.Write(kvp.Key); 
            writer.Write(kvp.Value); 
        }
    }

    private void LoadIndex() {
        if (!File.Exists(IndexFilePath)) return;
        
        try {
            using var stream = File.OpenRead(IndexFilePath);
            using var reader = new BinaryReader(stream, Encoding.UTF8);
            
            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != MagicNumberIndex) return;
            
            reader.ReadInt32();
            _idCounter = reader.ReadInt32();

            var idCount = reader.ReadInt32();
            for (var i = 0; i < idCount; i++) {
                _idIndex[reader.ReadInt32()] = reader.ReadInt64();
            }

            var dniCount = reader.ReadInt32();
            for (var i = 0; i < dniCount; i++) {
                _dniIndex[reader.ReadString()] = reader.ReadInt32();
            }

            if (stream.Position < stream.Length) {
                var sizeCount = reader.ReadInt32();
                for (var i = 0; i < sizeCount; i++) {
                    _sizeIndex[reader.ReadInt32()] = reader.ReadInt64();
                }
            }
        }
        catch (Exception ex) { 
            _logger.Error(ex, "Error al reconstruir los índices."); 
        }
    }

    private void LoadFragments() {
        if (!File.Exists(FragmentsFilePath)) return;
        
        try {
            using var stream = File.OpenRead(FragmentsFilePath);
            using var reader = new BinaryReader(stream, Encoding.UTF8);
            
            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != MagicNumberFragments) return;
            
            reader.ReadInt32();
            var count = reader.ReadInt32();
            
            for (var i = 0; i < count; i++) {
                _fragments[reader.ReadInt64()] = reader.ReadInt64();
            }
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al cargar fragmentos.");
        }
    }

    private void SaveFragments() {
        using var stream = File.Create(FragmentsFilePath);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);
        
        writer.Write(Encoding.ASCII.GetBytes(MagicNumberFragments));
        writer.Write(CurrentVersion);
        
        writer.Write(_fragments.Count);
        foreach (var kvp in _fragments) { 
            writer.Write(kvp.Key); 
            writer.Write(kvp.Value); 
        }
    }

    private void EnsureDataFolder() {
        if (!Directory.Exists(Configuracion.DataFolder)) {
            Directory.CreateDirectory(Configuracion.DataFolder);
        }
    }
}
