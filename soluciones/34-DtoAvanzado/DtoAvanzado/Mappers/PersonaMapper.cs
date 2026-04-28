using DtoAvanzado.Dtos;
using DtoAvanzado.Models;

namespace DtoAvanzado.Mappers;

public static class PersonaMapper {
    public static PersonaDto ToDto(this Persona persona) {
        return new PersonaDto(
            persona.Nombre,
            persona.Apellido,
            persona.Edad,
            persona.FechaNacimiento,
            persona.Aficiones.Select(a => a.ToDto()).ToList(),
            // Si queremos incluir la propiedad calculada en el DTO, la asignamos aquí
            persona.IsMayorDeEdad
        );
    }
    
    public static Persona ToModel(this PersonaDto personaDto) {
        return new Persona(
            personaDto.Nombre,
            personaDto.Apellido,
            personaDto.Edad,
            personaDto.FechaNacimiento,
            personaDto.Aficiones.Select(a => a.ToModel()).ToList()
            // La propiedad IsMayorDeEdad no se asigna al modelo, ya que es calculada
            // a partir de la fecha de nacimiento y la edad
        );
    }
}