using System.Text;
using GestionAcademica.Config;
using GestionAcademica.Dto;
using GestionAcademica.Mappers.Personas;
using GestionAcademica.Models;
using GestionAcademica.Models.Personas;
using GestionAcademica.Storage.Common;
using Serilog;

namespace GestionAcademica.Storage.Binary;

public class AcademiaBinStorage : IAcademiaBinStorage
{
    private readonly ILogger _logger = Log.ForContext<AcademiaBinStorage>();

    public AcademiaBinStorage()
    {
        _logger.Debug("Inicializando la clase AcademiaBinStorage");
        InitStorage();
    }

    /// <summary>
    ///     Guarda una colección de personas en un archivo binario.
    ///     ALGORITMO:
    ///     1. Escribimos el número total de personas (count) al inicio del archivo
    ///     2. Recorremos cada persona y la convertimos a DTO
    ///     3. Escribimos campo a campo del DTO en binario (no como JSON, es más eficiente)
    ///     NOTA PARA EL ALUMNO: Al escribir el count al inicio, al leer sabemos exactamente
    ///     cuántas personas hay y evitamos leer más allá del archivo.
    /// </summary>
    public void Salvar(IEnumerable<Persona> items, string path)
    {
        _logger.Debug("Guardando los items en el archivo binario '{path}'", path);
        using var stream = File.Create(path);
        using var writer = new BinaryWriter(stream, Encoding.UTF8);

        var dtos = items.Select(p => p.ToDto()).ToList();
        writer.Write(dtos.Count);

        foreach (var dto in dtos)
        {
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
        }
    }

    /// <summary>
    ///     Carga una colección de personas desde un archivo binario.
    ///     ALGORITMO:
    ///     1. Leemos el primer entero que es el count (número de personas)
    ///     2. Con un for leemos exactamente count personas
    ///     3. Cada persona la leemos campo a campo en el mismo orden que se escribió
    ///     4. Convertimos cada DTO a modelo y lo añadimos a la lista
    ///     NOTA PARA EL ALUMNO: Al saber de antemano el número de personas,
    ///     usamos un for en lugar de while. Es más seguro y eficiente.
    /// </summary>
    public IEnumerable<Persona> Cargar(string path)
    {
        _logger.Debug("Cargando los items del archivo binario '{path}'", path);
        if (!File.Exists(path))
        {
            _logger.Warning("El archivo '{path}' no existe. No se puede cargar nada.", path);
            throw new FileNotFoundException($"El archivo '{path}' no existe.");
        }

        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream, Encoding.UTF8);

        var count = reader.ReadInt32();
        var personas = new List<Persona>();

        for (var i = 0; i < count; i++)
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
            personas.Add(dto.ToModel());
        }

        return personas;
    }

    private void InitStorage()
    {
        if (Directory.Exists(Configuracion.DataFolder))
            return;
        _logger.Debug("El directorio 'data' no existe. Creándolo...");
        Directory.CreateDirectory(Configuracion.DataFolder);
    }
}
