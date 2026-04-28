using System.Xml.Serialization;

namespace FicherosCsv.Dto;

[XmlType("estudiante")]
public record EstudianteDto(
    [property: XmlAttribute("id")] int Id,
    [property: XmlElement("nombre")] string Nombre,
    [property: XmlElement("edad")] int Edad,
    [property: XmlElement("calificacion")] double Nota,
    [property: XmlElement("aprobado")] string Aprobado,
    [property: XmlElement("is-aprobado")]
    string? IsAprobado = null
) {
    // El constructor secundario es necesario para la deserialización XML
    public EstudianteDto() : this(0, string.Empty, 0, 0.0, "No") { }
}