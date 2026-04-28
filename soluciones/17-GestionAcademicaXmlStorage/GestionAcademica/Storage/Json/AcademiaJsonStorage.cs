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
    private readonly JsonSerializerOptions Options = new() {
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
            // Convertimos los modelos a DTOs para una mejor serialización
            var dtos = items.Select(p => p.ToDto()).ToList();
            // Serializamos los DTOs a JSON y escribimos en el archivo
            var json = JsonSerializer.Serialize(dtos, Options);
            // Aseguramos que el directorio exista antes de escribir
            File.WriteAllText(path, json, Encoding.UTF8);
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
            // Leemos el contenido del archivo JSON
            var json = File.ReadAllText(path, Encoding.UTF8);
            // Deserializamos el JSON a DTOs y luego a modelos
            var dtos = JsonSerializer.Deserialize<List<PersonaDto>>(json, Options);
            return dtos?.Select(dto => dto.ToModel()).ToList() ??
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