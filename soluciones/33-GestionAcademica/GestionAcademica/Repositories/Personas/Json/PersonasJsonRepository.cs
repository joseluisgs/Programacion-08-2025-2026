using System.Text.Json;
using GestionAcademica.Config;
using GestionAcademica.Models.Personas;
using GestionAcademica.Repositories.Common;
using GestionAcademica.Repositories.Personas.Base;
using Serilog;

namespace GestionAcademica.Repositories.Json;

/// <summary>
///     Repositorio JSON para la gestión de Personas.
///     NOTA PARA EL ALUMNO: Este repositorio implementa persistencia en archivo JSON.
///     Cada operación de escritura (Create, Update, Delete) guarda automáticamente.
///     Es más simple que Binary pero menos eficiente para grandes volúmenes de datos.
/// </summary>
public class PersonasJsonRepository : IPersonasRepository {
    // PATRÓN SINGLETON: Usamos Lazy<T> para garantizar una sola instancia.
    // Esto evita múltiples archivos JSON y problemas de consistencia.
    private static readonly Lazy<PersonasJsonRepository> Lazy = new(() => new PersonasJsonRepository());

    private readonly ILogger _logger = Log.ForContext<PersonasJsonRepository>();

    // ÍNDICE PRIMARIO: ID -> Persona (búsqueda O(1))
    private readonly Dictionary<int, Persona> _porId = new();

    // ÍNDICE SECUNDARIO: DNI -> ID (búsqueda O(1) por DNI)
    // NOTA PARA EL ALUMNO: Mantenemos dos diccionarios para búsquedas eficientes.
    // Si solo tuviéramos _porId, buscar por DNI requeriría iterar O(n).
    private readonly Dictionary<string, int> _dniIndex = new();

    // Ruta del archivo JSON
    private readonly string _filePath;

    // Contador de IDs para asignar nuevos registros
    private int _idCounter;

    // OPCIONES DE SERIALIZACIÓN JSON
    // NOTA PARA EL ALUMNO: Configuramos el serializador para que sea legible (WriteIndented)
    // y use camelCase en las propiedades (estándar en JSON).
    private readonly JsonSerializerOptions _jsonOptions = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    ///     Constructor privado (requerido por Singleton).
    ///     Inicializa el repositorio y carga los datos existentes.
    /// </summary>
    private PersonasJsonRepository() {
        _logger.Debug("Inicializando repositorio JSON.");
        // Obtenemos la ruta del archivo desde Configuracion (lee de appsettings.json)
        _filePath = Path.Combine(Configuracion.DataFolder, "academia.json");
        EnsureDirectory();
        Load(); // Cargamos datos al iniciar
    }

    /// <summary>Instancia única del repositorio.</summary>
    public static PersonasJsonRepository Instance => Lazy.Value;

    // ============================================================
    // MÉTODOS DE INFRAESTRUCTURA
    // ============================================================

    /// <summary>
    ///     Asegura que el directorio de datos exista.
    ///     NOTA PARA EL ALUMNO: Es importante crear el directorio antes de intentar escribir.
    /// </summary>
    private void EnsureDirectory() {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
            _logger.Debug("Creando directorio: {dir}", dir);
            Directory.CreateDirectory(dir);
        }
    }

    /// <summary>
    ///     CARGA INICIAL: Lee el archivo JSON y reconstruye los índices.
    ///     NOTA PARA EL ALUMNO: Al iniciar, reconstruimos los diccionarios desde el archivo.
    ///     Esto permite búsquedas O(1) durante toda la ejecución.
    /// </summary>
    private void Load() {
        try {
            if (!File.Exists(_filePath)) {
                _logger.Information("Archivo JSON no existe. Repositorio vacío.");
                return;
            }

            var json = File.ReadAllText(_filePath);
            var personas = JsonSerializer.Deserialize<List<Persona>>(json, _jsonOptions);
            
            if (personas == null) return;

            foreach (var p in personas) {
                _porId[p.Id] = p;
                _dniIndex[p.Dni] = p.Id;
                // Actualizamos el contador para que no se repitan IDs
                if (p.Id > _idCounter) _idCounter = p.Id;
            }

            _logger.Information("Cargados {count} registros desde JSON.", _porId.Count);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al cargar el archivo JSON.");
        }
    }

    /// <summary>
    ///     PERSISTENCIA: Guarda todo el estado en el archivo JSON.
    ///     NOTA PARA EL ALUMNO: Serializamos TODAS las personas a JSON.
    ///     Este enfoque es simple pero ineficiente si hay muchos registros.
    ///     Para optimizar, se podría usar el patrón "Log de transacciones" (como las bases de datos).
    /// </summary>
    private void Save() {
        try {
            var personas = _porId.Values.ToList();
            var json = JsonSerializer.Serialize(personas, _jsonOptions);
            File.WriteAllText(_filePath, json);
            _logger.Debug("Datos guardados en JSON. Total: {count}", personas.Count);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al guardar en JSON.");
            throw;
        }
    }

    // ============================================================
    // OPERACIONES CRUD
    // ============================================================

    /// <inheritdoc />
    public IEnumerable<Persona> GetAll() {
        return _porId.Values;
    }

    /// <inheritdoc />
    public Persona? GetById(int id) {
        return _porId.GetValueOrDefault(id);
    }

    /// <inheritdoc />
    public Persona? GetByDni(string dni) {
        // BÚSQUEDA O(1): Usamos el índice de DNI para encontrar rápido.
        return _dniIndex.TryGetValue(dni, out var id) ? _porId.GetValueOrDefault(id) : null;
    }

    /// <inheritdoc />
    public bool ExisteDni(string dni) {
        return _dniIndex.ContainsKey(dni);
    }

    /// <inheritdoc />
    public Persona? Create(Persona entity) {
        // VALIDACIÓN: Comprobamos que no exista el DNI
        if (ExisteDni(entity.Dni)) return null;

        // Asignamos nuevo ID y marcas de tiempo
        var nuevaPersona = entity with {
            Id = ++_idCounter,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        // Actualizamos índices en memoria
        _porId[nuevaPersona.Id] = nuevaPersona;
        _dniIndex[nuevaPersona.Dni] = nuevaPersona.Id;
        
        // PERSISTENCIA INMEDIATA: Guardamos tras cada modificación
        Save();
        return nuevaPersona;
    }

    /// <inheritdoc />
    public Persona? Update(int id, Persona entity) {
        if (!_porId.TryGetValue(id, out var actual)) return null;

        // Si cambió el DNI, verificar que no existe en otra persona
        if (entity.Dni != actual.Dni && _dniIndex.TryGetValue(entity.Dni, out var otroId) && otroId != id) {
            _logger.Warning("No se puede actualizar persona con id {Id} porque el DNI {Dni} ya está en uso por otra persona", id, entity.Dni);
            return null;
        }

        // Creamos copia con los nuevos datos, preservando CreatedAt original
        var actualizada = entity with {
            Id = id,
            CreatedAt = actual.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _porId[id] = actualizada;

        // Si cambió el DNI, actualizamos el índice secundario
        if (actual.Dni != actualizada.Dni) {
            _dniIndex.Remove(actual.Dni);
            _dniIndex[actualizada.Dni] = id;
        }

        // PERSISTENCIA INMEDIATA
        Save();
        return actualizada;
    }

    /// <inheritdoc />
    public Persona? Delete(int id) {
        if (!_porId.TryGetValue(id, out var persona)) return null;

        // BORRADO LÓGICO: Marcamos como eliminada sin borrar de memoria
        var eliminada = persona with {
            IsDeleted = true,
            UpdatedAt = DateTime.UtcNow
        };

        _porId[id] = eliminada;
        
        // PERSISTENCIA INMEDIATA
        Save();
        return eliminada;
    }

    /// <inheritdoc />
    public bool DeleteAll() {
        // Limpiamos estructuras en memoria
        _porId.Clear();
        _dniIndex.Clear();
        _idCounter = 0;

        // Borramos el archivo físico
        if (File.Exists(_filePath)) {
            File.Delete(_filePath);
        }

        _logger.Information("Repositorio JSON limpiado.");
        return true;
    }
}
