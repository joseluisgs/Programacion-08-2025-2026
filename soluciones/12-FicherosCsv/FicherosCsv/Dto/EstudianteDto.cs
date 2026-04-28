namespace FicherosCsv.Dto;

public record EstudianteDto(
    int Id,
    string Nombre,
    int Edad,
    double Nota,
    string Aprobado
);