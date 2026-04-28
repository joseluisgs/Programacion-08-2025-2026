namespace GestionAcademica.Dto;

public record PersonaDto(
    int Id,
    string Dni,
    string Nombre,
    string Apellidos,
    string Tipo,
    string? Experiencia,
    string? Especialidad,
    string Ciclo,
    string? Curso,
    string? Calificacion,
    string CreatedAt,
    string UpdatedAt,
    bool IsDeleted
);