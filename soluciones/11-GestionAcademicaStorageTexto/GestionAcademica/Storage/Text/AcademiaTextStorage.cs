using GestionAcademica.Config;
using GestionAcademica.Models;
using Serilog;

namespace GestionAcademica.Storage.Text;

public class AcademiaTextStorage : IAcademiaTextStorage {
    private readonly ILogger _logger = Log.ForContext<AcademiaTextStorage>();


    public AcademiaTextStorage() {
        _logger.Debug("Inicializando la clase AcademiaTextStorage");
        // si no existe el directorio data, debemos crearlo para evitar errores al guardar archivos
        InitStorage();
    }


    /// <inheritdoc cref="IAcademiaTextStorage.Salvar" />
    public void Salvar(IEnumerable<Persona> items, string path) {
        _logger.Debug("Guardando los items en el archivo '{path}'", path);
        // Uso de StreamWriter para escribir línea por línea, lo que es más eficiente para archivos grandes.
        using var writer = new StreamWriter(path);
        items.ToList().ForEach(p => writer.WriteLine(ObteneLineaDePersona(p)));
    }

    /// <inheritdoc cref="IAcademiaTextStorage.Cargar" />
    public IEnumerable<Persona> Cargar(string path) {
        _logger.Debug("Cargando los items del archivo '{path}'", path);
        if (!Path.Exists(path)) {
            _logger.Warning("El archivo '{path}' no existe. No se puede cargar nada.", path);
            return [];
        }

        // Uso ReadLines para leer línea por línea, de manera Lazy
        // lo que es más eficiente para archivos grandes.
        return File.ReadLines(path)
            .Select(ObtenerPersonaDeLinea);
    }

    /// <summary>
    ///     Inicializa el directorio de datos si no existe.
    /// </summary>
    /// <param name="linea">Línea de texto con los datos de una persona, separados por punto y coma.</param>
    /// <returns>Una instancia de <see cref="Persona" /> creada a partir de los datos de la línea.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si el tipo de persona en la línea no es reconocido.</exception>
    private Persona ObtenerPersonaDeLinea(string linea) {
        try {
            //_logger.Debug("Obteniendo datos de la línea: {linea}", linea);
            var partes = linea.Split(';');
            return partes[7] switch {
                "Estudiante" => new Estudiante {
                    Id = int.Parse(partes[0]),
                    Dni = partes[1],
                    Nombre = partes[2],
                    Apellidos = partes[3],
                    CreatedAt = DateTime.Parse(partes[4]),
                    UpdatedAt = DateTime.Parse(partes[5]),
                    IsDeleted = bool.Parse(partes[6]),
                    Calificacion = double.Parse(partes[8]),
                    Ciclo = Enum.Parse<Ciclo>(partes[9]),
                    Curso = Enum.Parse<Curso>(partes[10])
                },
                "Docente" => new Docente {
                    Id = int.Parse(partes[0]),
                    Dni = partes[1],
                    Nombre = partes[2],
                    Apellidos = partes[3],
                    CreatedAt = DateTime.Parse(partes[4]),
                    UpdatedAt = DateTime.Parse(partes[5]),
                    IsDeleted = bool.Parse(partes[6]),
                    Experiencia = int.Parse(partes[8]),
                    Especialidad = partes[9],
                    Ciclo = Enum.Parse<Ciclo>(partes[10])
                },
                _ => throw new InvalidOperationException($"Tipo de persona desconocido en la línea: {linea}")
            };
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al convertir la línea a persona: {message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    ///     Genera una línea de texto con los datos de la persona.
    /// </summary>
    /// <param name="persona">Instancia de la persona a convertir.</param>
    /// <returns>Una cadena de texto con los datos de la persona separados por punto y coma.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si el tipo de persona no es reconocido.</exception>
    private string ObteneLineaDePersona(Persona persona) {
        try {
            var datosComunes =
                $"{persona.Id};{persona.Dni};{persona.Nombre};{persona.Apellidos};{persona.CreatedAt};{persona.UpdatedAt};{persona.IsDeleted}";

            var datosPropios = persona switch {
                Estudiante e => $"Estudiante;{e.Calificacion};{e.Ciclo};{e.Curso}",
                Docente d => $"Docente;{d.Experiencia};{d.Especialidad};{d.Ciclo}",
                _ => throw new InvalidOperationException("Tipo de persona desconocido")
            };
            var linea = $"{datosComunes};{datosPropios}";
            return linea;
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al convertir la persona a línea de texto: {message}", ex.Message);
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