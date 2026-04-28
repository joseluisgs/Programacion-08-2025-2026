namespace DtoAvanzado.Dtos;

public record PersonaDto(
    string Nombre,
    string Apellido,
    int Edad,
    DateTime FechaNacimiento,
    List<AficionDto> Aficiones,
    bool IsMayorDeEdad
) {
        public override string ToString() => $"Nombre: {Nombre} {Apellido}, Edad: {Edad}, Fecha de Nacimiento: {FechaNacimiento.ToShortDateString()}, Aficiones:[{string.Join(", ", Aficiones)}], {IsMayorDeEdad}";
}

