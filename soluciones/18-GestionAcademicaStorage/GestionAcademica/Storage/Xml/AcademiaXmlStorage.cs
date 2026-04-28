using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers;
using GestionAcademica.Models;
using Serilog;

namespace GestionAcademica.Storage.Xml;

public class AcademiaXmlStorage : IAcademiaXmlStorage {
    private readonly ILogger _logger = Log.ForContext<AcademiaXmlStorage>();

    // XmlSerializerNamespaces vacío para evitar que aparezcan prefijos de namespace en el XML
    private readonly XmlSerializerNamespaces XmlSerializerNamespaces = new();

    // Configuramos las opciones de serialización para el XML de forma global
    // XmlWriterSettings para configurar la escritura del XML
    private readonly XmlWriterSettings XmlWriterSettings = new() {
        Indent = true, // Pretty print para que el XML sea más legible
        Encoding = Encoding.UTF8 // Usamos UTF-8 para soportar caracteres especiales como tildes y eñes
    };


    public AcademiaXmlStorage() {
        _logger.Debug("Inicializando la clase AcademiaXmlStorage");
        InitStorage();
    }


    /// <inheritdoc cref="IAcademiaXmlStorage.Salvar" />
    public void Salvar(IEnumerable<Persona> items, string path) {
        try {
            _logger.Debug("Guardando los items en el archivo '{path}'", path);
            // XmlSerializer requiere una lista o array para serializar una colección.
            // Materializamos los DTOs, pero lo hacemos justo antes de serializar.
            var dtos = items.Select(p => p.ToDto()).ToList();
            var serializer = new XmlSerializer(typeof(List<PersonaDto>));
            
            // NOTA PARA EL ALUMNO: Usamos XmlWriter sobre el stream para un control total de la codificación y el indentado.
            using var streamWriter = new StreamWriter(path, false, Encoding.UTF8);
            using var xmlWriter = XmlWriter.Create(streamWriter, XmlWriterSettings);
            serializer.Serialize(xmlWriter, dtos, XmlSerializerNamespaces);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al guardar los items en el archivo '{path}'", path);
            throw;
        }
    }

    /// <inheritdoc cref="IAcademiaXmlStorage.Cargar" />
    public IEnumerable<Persona> Cargar(string path) {
        _logger.Debug("Cargando los items del archivo '{path}'", path);
        if (!Path.Exists(path)) {
            _logger.Warning("El archivo '{path}' no existe. No se puede cargar nada.", path);
            throw new FileNotFoundException($"El archivo '{path}' no existe.");
        }

        try {
            var serializer = new XmlSerializer(typeof(List<PersonaDto>));
            // NOTA PARA EL ALUMNO: Deserializamos directamente del stream del fichero.
            using var stream = File.OpenRead(path);
            var dtos = serializer.Deserialize(stream) as List<PersonaDto>;
            
            // Devolvemos el flujo proyectado sin el .ToList() final para mantener el Lazy Evaluation.
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