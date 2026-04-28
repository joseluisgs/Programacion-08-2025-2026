using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers;
using GestionAcademica.Models;
using GestionAcademica.Storage.Text;
using Serilog;

namespace GestionAcademica.Storage.Json;

public class AcademiaJsonStorage : IAcademiaJsonStorage {
    private readonly JsonSerializerOptions _options = new() {
        WriteIndented = true, // Para que el JSON sea más legible, es el equivalente a "pretty print"
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Convierte las propiedades a camelCase en el JSON
        DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull, // Ignora las propiedades que son null al escribir el JSON
        Converters = { new JsonStringEnumConverter() }, // Para serializar los enums como strings en lugar de números
        Encoder = JavaScriptEncoder
            .UnsafeRelaxedJsonEscaping // Permite caracteres especiales sin escaparlos, como acentos y eñes, lo cual es importante para el español
    };

    private readonly ILogger _logger = Log.ForContext<AcademiaJsonStorage>();


    public AcademiaJsonStorage() {
        _logger.Debug("Inicializando la clase AcademiaJsonStorage");
        InitStorage();
    }


    /// <inheritdoc cref="IAcademiaJsonStorage.Salvar" />
    public void Salvar(IEnumerable<Persona> items, string path) {
        try {
            _logger.Debug("Guardando los items en el archivo '{path}'", path);
            // NOTA PARA EL ALUMNO: Serializamos directamente al Stream del archivo.
            // Evitamos crear un string JSON gigante en memoria con JsonSerializer.Serialize(dtos).
            using var stream = File.Create(path);
            var dtos = items.Select(p => p.ToDto()).ToList(); // El serializador necesita una colección materializada para JSON arrays.
            JsonSerializer.Serialize(stream, dtos, _options);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al guardar los items en el archivo '{path}'", path);
            throw;
        }
    }

    /// <inheritdoc cref="IAcademiaJsonStorage.Cargar" />
    public IEnumerable<Persona> Cargar(string path) {
        _logger.Debug("Cargando los items del archivo '{path}'", path);
        if (!Path.Exists(path)) {
            _logger.Warning("El archivo '{path}' no existe. No se puede cargar nada.", path);
            throw new FileNotFoundException($"El archivo '{path}' no existe.");
        }

        try {
            // NOTA PARA EL ALUMNO: Leemos directamente del Stream.
            // Es mucho más eficiente que File.ReadAllText() para ficheros grandes.
            using var stream = File.OpenRead(path);
            var dtos = JsonSerializer.Deserialize<List<PersonaDto>>(stream, _options);
            
            // Devolvemos el IEnumerable proyectado sin el .ToList() final
            return dtos?.Select(dto => dto.ToModel()) ??
                   throw new InvalidOperationException("No se pudieron deserializar los DTOs.");
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al cargar los items del archivo '{path}'", path);
            throw;
        }
    }

    /// <summary>
    ///     Inicializa el almacenamiento asegurando que el directorio de datos exista.
    /// </summary>
    private void InitStorage() {
        if (Directory.Exists(Configuracion.DataFolder))
            return;
        _logger.Debug("El directorio 'data' no existe. Creándolo...");
        Directory.CreateDirectory("data");
    }
}