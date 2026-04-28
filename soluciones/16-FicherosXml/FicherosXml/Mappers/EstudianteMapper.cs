using FicherosCsv.Dto;
using FicherosCsv.Models;

namespace FicherosCsv.Mappers;

// funciones de extension para convertir entre Estudiante y EstudianteDto
public static class EstudianteMapper {
    public static EstudianteDto ToDto(this Estudiante estudiante) {
        return new EstudianteDto(
            estudiante.Id,
            estudiante.Nombre,
            estudiante.Edad,
            estudiante.Nota,
            estudiante.Aprobado ? "Sí" : "No",
            Random.Shared.Next(0, 2) == 1 ? "No Null" : null // solo para ver si sale o no
        );
    }

    public static Estudiante ToModel(this EstudianteDto dto) {
        return new Estudiante {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Edad = dto.Edad,
            Nota = dto.Nota,
            Aprobado = dto.Aprobado.Equals("Sí", StringComparison.OrdinalIgnoreCase)
        };
    }
}