using System.Text.Json.Serialization;

namespace GestionAcademica.Dto;

public record PersonaDto(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("dni")]
    string Dni,
    [property: JsonPropertyName("nombre")]
    string Nombre,
    [property: JsonPropertyName("apellidos")]
    string Apellidos,
    [property: JsonPropertyName("tipo")]
    string Tipo,
    [property: JsonPropertyName("experiencia"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Experiencia,
    [property: JsonPropertyName("especialidad"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Especialidad,
    [property: JsonPropertyName("ciclo")]
    string Ciclo,
    [property: JsonPropertyName("curso"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Curso,
    [property: JsonPropertyName("calificacion"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Calificacion,
    [property: JsonPropertyName("createdAt")]
    string CreatedAt,
    [property: JsonPropertyName("updatedAt")]
    string UpdatedAt,
    [property: JsonPropertyName("isDeleted")]
    bool IsDeleted
);