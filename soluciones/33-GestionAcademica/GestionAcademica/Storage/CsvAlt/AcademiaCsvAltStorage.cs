using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers.Personas;
using GestionAcademica.Models;
using GestionAcademica.Models.Personas;
using Serilog;

namespace GestionAcademica.Storage.CsvAlt;

public class AcademiaCsvAltStorage : IAcademiaCsvAltStorage {
    private readonly ILogger _logger = Log.ForContext<AcademiaCsvAltStorage>();

    // Configuramos las opciones de CSV de forma global
    // CsvConfiguration para configurar el lectura y escritura del CSV
    private readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture) {
        Delimiter = ";", // Usamos punto y coma como delimitador
        HasHeaderRecord = true // Indica que el CSV tiene cabecera
        // CsvHelper genera la cabecera automáticamente usando los nombres de las propiedades del DTO (PersonaDto)
        // Por ejemplo: id, dni, nombre, apellidos, tipo, ciclo, createdAt, updatedAt, isDeleted
    };


    public AcademiaCsvAltStorage() {
        _logger.Debug("Inicializando la clase AcademiaCsvAltStorage");
        InitStorage();
    }


    /// <inheritdoc cref="IAcademiaCsvAltStorage.Salvar" />
    public void Salvar(IEnumerable<Persona> items, string path) {
        try {
            _logger.Debug("Guardando los items en el archivo '{path}'", path);
            // Convertimos los modelos a DTOs para una mejor serialización
            var dtos = items.Select(p => p.ToDto()).ToList();
            // Creamos el StreamWriter para escribir el archivo CSV
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            // Creamos el CsvWriter para escribir los registros
            using var csv = new CsvWriter(writer, CsvConfiguration);
            // Escribimos los registros en el CSV
            csv.WriteRecords(dtos);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al guardar los items en el archivo '{path}'", path);
            throw;
        }
    }

    /// <inheritdoc cref="IAcademiaCsvAltStorage.Cargar" />
    public IEnumerable<Persona> Cargar(string path) {
        _logger.Debug("Cargando los items del archivo '{path}'", path);
        if (!Path.Exists(path)) {
            _logger.Warning("El archivo '{path}' no existe. No se puede cargar nada.", path);
            throw new FileNotFoundException($"El archivo '{path}' no existe.");
        }

        try {
            // Creamos el StreamReader para leer el archivo CSV
            using var reader = new StreamReader(path, Encoding.UTF8);
            // Creamos el CsvReader para leer los registros
            using var csv = new CsvReader(reader, CsvConfiguration);
            // Obtenemos los registros del CSV como lista de PersonaDto
            var dtos = csv.GetRecords<PersonaDto>().ToList();
            // Convertimos los DTOs a modelos Persona
            return dtos.Select(dto => dto.ToModel()).ToList();
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