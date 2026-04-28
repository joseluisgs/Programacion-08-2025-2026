using System.Text;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers.Personas;
using GestionAcademica.Models.Personas;
using GestionAcademica.Repositories.Personas.Base;
using Serilog;

namespace GestionAcademica.Repositories.Personas.Binary;

public class PersonasBinaryRepository : IPersonasRepository
{
    private const string DataFileName = "academia-db.dat";
    private const string IndexFileName = "academia-db.idx";
    private const string FragmentsFileName = "academia-db.frag";

    private const string MagicNumber = "ACAD";
    private const string MagicNumberIndex = "ACDI";
    private const string MagicNumberFragments = "ACFR";
    private const int CurrentVersion = 1;

    private const double FragmentationThreshold = 0.3;

    private static readonly Lazy<PersonasBinaryRepository> Lazy = new(() => new PersonasBinaryRepository());

    private readonly Dictionary<string, int> _dniIndex = new();
    private readonly Dictionary<int, long> _idIndex = new();
    private readonly Dictionary<int, long> _sizeIndex = new();
    private readonly Dictionary<long, long> _fragments = new();

    private readonly ILogger _logger = Log.ForContext<PersonasBinaryRepository>();
    private int _idCounter;

    private PersonasBinaryRepository()
    {
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

    private void InitializeFile()
    {
        try
        {
            if (!File.Exists(DataFilePath))
            {
                _logger.Information("Creando base de datos binaria v{CurrentVersion}.", CurrentVersion);
                using var stream = File.Create(DataFilePath);
                using var writer = new BinaryWriter(stream, Encoding.UTF8);
                writer.Write(Encoding.ASCII.GetBytes(MagicNumber));
                writer.Write(CurrentVersion);
            }
            else
            {
                using var stream = File.OpenRead(DataFilePath);
                using var reader = new BinaryReader(stream, Encoding.UTF8);
                if (stream.Length < 8)
                {
                    throw new InvalidOperationException("Archivo corrupto o demasiado pequeño.");
                }
                var magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
                if (magic != MagicNumber)
                {
                    throw new InvalidOperationException("La firma del archivo no coincide con nuestra base de datos.");
                }
                var version = reader.ReadInt32();
                if (version != CurrentVersion)
                {
                    throw new InvalidOperationException($"Versión de archivo no soportada ({version}).");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al inicializar el almacenamiento binario.");
            throw;
        }
    }

    /// <summary>
    ///     Busca el mejor hueco libre para el tamaño requerido.
    ///     ALGORITMO LINQ:
    ///     1. Filtramos los huecos cuyo tamaño sea >= al requerido
    ///     2. Los ordenamos por tamaño (menor primero) para encontrar el mejor ajuste (best-fit)
    ///     3. Tomamos el primero que cumpla
    ///     NOTA PARA EL ALUMNO: Best-fit minimiza el desperdicio de espacio.
    /// </summary>
    private long FindFreeSpace(long requiredSize)
    {
        // LINQ: Buscar el hueco más pequeño que sea suficiente
        var bestHole = _fragments
            .Where(kvp => kvp.Value >= requiredSize)
            .OrderBy(kvp => kvp.Value)
            .FirstOrDefault();

        var holePos = bestHole.Key;

        if (holePos == default || !_fragments.ContainsKey(holePos))
            return -1;

        var holeSize = _fragments[holePos];
        _fragments.Remove(holePos);

        if (holeSize > requiredSize + 10)
        {
            long remainingPos = holePos + requiredSize;
            long remainingSize = holeSize - requiredSize;
            _fragments[remainingPos] = remainingSize;
        }

        return holePos;
    }

    /// <summary>
    ///     Calcula el porcentaje de fragmentación del archivo.
    ///     Fórmula: (suma de tamaños de huecos) / (tamaño total del archivo)
    ///     ALGORITMO LINQ: Usa Sum() para calcular el tamaño total de huecos.
    /// </summary>
    private double CalcularFragmentacion()
    {
        if (!File.Exists(DataFilePath))
            return 0;

        var tamanoArchivo = new FileInfo(DataFilePath).Length;
        if (tamanoArchivo == 0)
            return 0;

        // LINQ: Sumar todos los tamaños de huecos
        var tamanoHuecos = _fragments.Values.Sum();

        return (double)tamanoHuecos / tamanoArchivo;
    }

    /// <summary>
    ///     Compacta el archivo de datos eliminando todos los huecos.
    ///     ALGORITMO:
    ///     1. Leer todos los registros existentes
    ///     2. Crear un nuevo archivo de datos vacío
    ///     3. Escribir todos los registros secuencialmente (sin huecos)
    ///     4. Actualizar el índice con las nuevas posiciones
    ///     5. Vaciar la lista de huecos
    /// </summary>
    private void Compactar()
    {
        if (_idIndex.Count == 0)
            return;

        _logger.Warning("Compactando archivo binario por alta fragmentación...");

        var registros = _idIndex.Keys
            .Select(id => (Id: id, Persona: GetById(id)))
            .OfType<(int Id, Persona Persona)>()
            .ToDictionary(x => x.Id, x => x.Persona);

        using var stream = File.Create(DataFilePath);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        writer.Write(Encoding.ASCII.GetBytes(MagicNumber));
        writer.Write(CurrentVersion);

        _idIndex.Clear();
        _fragments.Clear();

        foreach (var (id, persona) in registros)
        {
            var position = stream.Position;
            EscribirPersonaBinario(writer, persona with { Id = id });
            _idIndex[id] = position;
            _sizeIndex[id] = GetPersonaSize(persona with { Id = id });
        }

        SaveIndex();
        SaveFragments();
        _logger.Information("Compactación completada.");
    }

    /// <summary>
    ///     Comprueba si la fragmentación supera el umbral y compacta si es necesario.
    ///     Se llama después de cada Delete.
    /// </summary>
    private void ComprobarFragmentacion()
    {
        if (CalcularFragmentacion() > FragmentationThreshold)
            Compactar();
    }

    private void EscribirPersonaBinario(BinaryWriter writer, Persona persona)
    {
        var dto = persona.ToDto();
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
    }

    private long WritePersona(Persona persona, long position = -1)
    {
        var dto = persona.ToDto();
        using var stream = File.Open(DataFilePath, FileMode.Open, FileAccess.Write);

        if (position == -1)
        {
            stream.Seek(0, SeekOrigin.End);
        }
        else
        {
            stream.Seek(position, SeekOrigin.Begin);
        }

        var actualPosition = stream.Position;
        using var writer = new BinaryWriter(stream, Encoding.UTF8);
        EscribirPersonaBinario(writer, dto);

        return actualPosition;
    }

    private void EscribirPersonaBinario(BinaryWriter writer, PersonaDto dto)
    {
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
    }

    private Persona? ReadPersonaAt(long position)
    {
        if (!File.Exists(DataFilePath)) return null;

        using var stream = File.OpenRead(DataFilePath);
        stream.Seek(position, SeekOrigin.Begin);
        using var reader = new BinaryReader(stream, Encoding.UTF8);
        return LeerPersonaBinario(reader);
    }

    private Persona? LeerPersonaBinario(BinaryReader reader)
    {
        try
        {
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
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al deserializar un registro.");
            return null;
        }
    }

    private long GetPersonaSize(Persona persona)
    {
        var dto = persona.ToDto();
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms, Encoding.UTF8);
        EscribirPersonaBinario(writer, dto);
        return ms.Length;
    }

    public IEnumerable<Persona> GetAll()
    {
        _logger.Debug("Consultando repositorio binario.");

        // LINQ: Usamos LINQ para obtener y filtrar personas
        return _idIndex.Keys
            .OrderBy(id => id)
            .Select(id => GetById(id))
            .OfType<Persona>();
    }

    public Persona? GetById(int id)
    {
        if (_idIndex.TryGetValue(id, out var position))
        {
            return ReadPersonaAt(position);
        }
        return null;
    }

    public Persona? GetByDni(string dni)
    {
        // LINQ: Buscar en el índice de DNI
        if (_dniIndex.TryGetValue(dni, out var id))
        {
            return GetById(id);
        }
        return null;
    }

    public bool ExisteDni(string dni)
    {
        return _dniIndex.ContainsKey(dni);
    }

    public Persona? Create(Persona entity)
    {
        if (ExisteDni(entity.Dni)) return null;

        var id = ++_idCounter;
        var newPersona = entity with
        {
            Id = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        try
        {
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
        catch
        {
            throw new InvalidOperationException("No se pudo completar la creación del registro binario.");
        }
    }

    public Persona? Update(int id, Persona entity)
    {
        if (_idIndex.TryGetValue(id, out var oldPosition) == false) return null;

        var oldSize = _sizeIndex[id];
        // LINQ: Buscar el DNI asociado a esta ID
        var existingDni = _dniIndex.FirstOrDefault(x => x.Value == id).Key;

        // Si cambió el DNI, verificar que no exista en otra persona
        if (entity.Dni != existingDni && _dniIndex.TryGetValue(entity.Dni, out var otroId) && otroId != id)
        {
            _logger.Warning("No se puede actualizar persona con id {Id} porque el DNI {Dni} ya está en uso por otra persona", id, entity.Dni);
            return null;
        }

        var updatedPersona = entity with
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        try
        {
            var newSize = GetPersonaSize(updatedPersona);
            long finalPosition;

            if (newSize <= oldSize)
            {
                finalPosition = WritePersona(updatedPersona, oldPosition);
                if (oldSize > newSize + 10)
                {
                    _fragments[oldPosition + newSize] = oldSize - newSize;
                    SaveFragments();
                }
            }
            else
            {
                _fragments[oldPosition] = oldSize;
                var foundPosition = FindFreeSpace(newSize);
                finalPosition = WritePersona(updatedPersona, foundPosition);
                SaveFragments();
            }

            _idIndex[id] = finalPosition;
            _sizeIndex[id] = newSize;

            if (existingDni != updatedPersona.Dni)
            {
                _dniIndex.Remove(existingDni);
                _dniIndex[updatedPersona.Dni] = id;
            }

            SaveIndex();
            return updatedPersona;
        }
        catch
        {
            throw new InvalidOperationException("Error crítico al actualizar los datos en disco.");
        }
    }

    public Persona? Delete(int id)
    {
        if (_idIndex.TryGetValue(id, out var position) == false) return null;

        if (ReadPersonaAt(position) is not { } persona) return null;

        var deletedPersona = persona with
        {
            IsDeleted = true,
            UpdatedAt = DateTime.UtcNow
        };

        WritePersona(deletedPersona, position);
        ComprobarFragmentacion();
        return deletedPersona;
    }

    public bool DeleteAll()
    {
        _idIndex.Clear();
        _dniIndex.Clear();
        _fragments.Clear();
        _idCounter = 0;

        try
        {
            if (File.Exists(DataFilePath)) File.Delete(DataFilePath);

            InitializeFile();
            SaveIndex();
            SaveFragments();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SaveIndex()
    {
        using var stream = File.Create(IndexFilePath);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        writer.Write(Encoding.ASCII.GetBytes(MagicNumberIndex));
        writer.Write(CurrentVersion);
        writer.Write(_idCounter);

        writer.Write(_idIndex.Count);
        foreach (var kvp in _idIndex)
        {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value);
        }

        writer.Write(_dniIndex.Count);
        foreach (var kvp in _dniIndex)
        {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value);
        }

        writer.Write(_sizeIndex.Count);
        foreach (var kvp in _sizeIndex)
        {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value);
        }
    }

    private void LoadIndex()
    {
        if (!File.Exists(IndexFilePath)) return;

        try
        {
            using var stream = File.OpenRead(IndexFilePath);
            using var reader = new BinaryReader(stream, Encoding.UTF8);

            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != MagicNumberIndex) return;

            reader.ReadInt32();
            _idCounter = reader.ReadInt32();

            var idCount = reader.ReadInt32();
            for (var i = 0; i < idCount; i++)
            {
                _idIndex[reader.ReadInt32()] = reader.ReadInt64();
            }

            var dniCount = reader.ReadInt32();
            for (var i = 0; i < dniCount; i++)
            {
                _dniIndex[reader.ReadString()] = reader.ReadInt32();
            }

            if (stream.Position < stream.Length)
            {
                var sizeCount = reader.ReadInt32();
                for (var i = 0; i < sizeCount; i++)
                {
                    _sizeIndex[reader.ReadInt32()] = reader.ReadInt64();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al reconstruir los índices.");
        }
    }

    private void LoadFragments()
    {
        if (!File.Exists(FragmentsFilePath)) return;

        try
        {
            using var stream = File.OpenRead(FragmentsFilePath);
            using var reader = new BinaryReader(stream, Encoding.UTF8);

            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != MagicNumberFragments) return;

            reader.ReadInt32();
            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                _fragments[reader.ReadInt64()] = reader.ReadInt64();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al cargar fragmentos.");
        }
    }

    private void SaveFragments()
    {
        using var stream = File.Create(FragmentsFilePath);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        writer.Write(Encoding.ASCII.GetBytes(MagicNumberFragments));
        writer.Write(CurrentVersion);

        writer.Write(_fragments.Count);
        foreach (var kvp in _fragments)
        {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value);
        }
    }

    private void EnsureDataFolder()
    {
        if (!Directory.Exists(Configuracion.DataFolder))
        {
            Directory.CreateDirectory(Configuracion.DataFolder);
        }
    }
}
