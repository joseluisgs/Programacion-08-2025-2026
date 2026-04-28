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
            // Convertimos los modelos a DTOs para una mejor serialización
            var dtos = items.Select(p => p.ToDto()).ToList();
            // Serializamos los DTOs a XML usando XmlSerializer
            var serializer = new XmlSerializer(typeof(List<PersonaDto>));
            // Escribimos en el archivo con las opciones de XML configuradas globalmente
            using var streamWriter = new StreamWriter(path);
            var xmlWriter = XmlWriter.Create(streamWriter, XmlWriterSettings);
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
            // Deserializamos el XML a DTOs usando XmlSerializer y luego a modelos
            var serializer = new XmlSerializer(typeof(List<PersonaDto>));
            using var streamReader = new StreamReader(path);
            // Deserializamos el XML a una lista de PersonaDto
            var dtos = serializer.Deserialize(streamReader) as List<PersonaDto>;
            // Convertimos los DTOs a modelos y los devuelve
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