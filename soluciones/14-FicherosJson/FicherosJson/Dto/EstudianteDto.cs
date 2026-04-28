using System.Text.Json.Serialization;

namespace FicherosCsv.Dto;

public record EstudianteDto(
    int Id,
    string Nombre,
    int Edad,
    [property: JsonPropertyName("calificacion")]
    double Nota,
    string Aprobado,
    string? IsAprobado = null
);