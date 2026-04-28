using System.Text;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers;
using GestionAcademica.Models;
using GestionAcademica.Storage.Text;
using Serilog;

namespace GestionAcademica.Storage.Csv;

public class AcademiaCsvStorage : IAcademiaCsvStorage {
    private readonly ILogger _logger = Log.ForContext<AcademiaTextStorage>();


    public AcademiaCsvStorage() {
        _logger.Debug("Inicializando la clase AcademiaCsvStorage");
        InitStorage();
    }


    /// <inheritdoc cref="IAcademiaTextStorage.Salvar" />
    public void Salvar(IEnumerable<Persona> items, string path) {
        try {
            _logger.Debug("Guardando los items en el archivo '{path}'", path);
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            // Escribimos la cabecera del CSV
            writer.WriteLine(
                "Id;Dni;Nombre;Apellidos;Tipo;Experiencia;Especialidad;Ciclo;Curso;Calificacion;CreatedAt;UpdatedAt;IsDeleted");
            // Escribimos los items en el CSV, pero primero los convertimos a DTOs
            // para asegurarnos de que el formato sea correcto
            items.Select(p => p.ToDto())
                .ToList()
                .ForEach(dto => {
                    writer.WriteLine(
                        $"{dto.Id};{dto.Dni};{dto.Nombre};{dto.Apellidos};{dto.Tipo};{dto.Experiencia};{dto.Especialidad};{dto.Ciclo};{dto.Curso};{dto.Calificacion};{dto.CreatedAt};{dto.UpdatedAt};{dto.IsDeleted}");
                });
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al guardar los items en el archivo '{path}'", path);
            throw;
        }
    }

    /// <inheritdoc cref="IAcademiaTextStorage.Cargar" />
    public IEnumerable<Persona> Cargar(string path) {
        _logger.Debug("Cargando los items del archivo '{path}'", path);
        if (!Path.Exists(path)) {
            _logger.Warning("El archivo '{path}' no existe. No se puede cargar nada.", path);
            return [];
        }

        try {
            return File.ReadLines(path, Encoding.UTF8)
                .Skip(1)
                .Select(linea => linea.Split(';'))
                .Select(campos => new PersonaDto(
                    int.Parse(campos[0]),
                    campos[1],
                    campos[2],
                    campos[3],
                    campos[4],
                    campos[5],
                    campos[6],
                    campos[7],
                    campos[8],
                    campos[9],
                    campos[10],
                    campos[11],
                    bool.TryParse(campos[12], out var isDel) && isDel
                ).ToModel())
                //.Select(dto => dto.ToModel()) // Alternativa para convertir los DTOs a Personas
                .ToList();
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