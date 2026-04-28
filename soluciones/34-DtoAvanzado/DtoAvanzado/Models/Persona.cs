namespace DtoAvanzado.Models;

public record Persona(
    string Nombre,
    string Apellido,
    int Edad,
    DateTime FechaNacimiento,
    List<Aficion> Aficiones
) {
    public bool IsMayorDeEdad => FechaNacimiento.AddYears(Edad) < DateTime.Now;
    
    public override string ToString() => $"Nombre: {Nombre} {Apellido}, Edad: {Edad}, Fecha de Nacimiento: {FechaNacimiento.ToShortDateString()}, Aficiones:[{string.Join(", ", Aficiones)}], {IsMayorDeEdad}";
}