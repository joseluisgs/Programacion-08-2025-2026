using System.Xml.Serialization;

namespace GestionAcademica.Dto;

[XmlRoot("Academia")]
[XmlType("Persona")]
public record PersonaDto(
    [property: XmlAttribute("id")] int Id,
    [property: XmlElement("dni")] string Dni,
    [property: XmlElement("nombre")] string Nombre,
    [property: XmlElement("apellidos")] string Apellidos,
    [property: XmlElement("tipo")] string Tipo,
    [property: XmlElement("experiencia")] string? Experiencia,
    [property: XmlElement("especialidad")] string? Especialidad,
    [property: XmlElement("ciclo")] string Ciclo,
    [property: XmlElement("curso")] string? Curso,
    [property: XmlElement("calificacion")] string? Calificacion,
    [property: XmlElement("createdAt")] string CreatedAt,
    [property: XmlElement("updatedAt")] string UpdatedAt,
    [property: XmlElement("isDeleted")] bool IsDeleted
) {
    // Constructor sin parámetros para la deserialización esto es necesario para que la deserialización funcione correctamente,
    // ya que algunas bibliotecas de deserialización requieren un constructor sin parámetros (XML)
    public PersonaDto() : this(0, "", "", "", "", null, null, "", null, null, "", "", false) { }
}