// See https://aka.ms/new-console-template for more information

using DtoAvanzado.Mappers;
using DtoAvanzado.Models;

Console.WriteLine("Dto Avanzado");
Console.WriteLine("==============");

var listaAficiones = new List<Aficion>() {
    new Aficion("Futbol", "Deporte de equipo que se juega con una pelota"),
    new Aficion("Cocina", "Arte de preparar alimentos"),
    new Aficion("Viajar", "Actividad de desplazarse a diferentes lugares")
};

var lucia = new Persona(
    "Lucia",
    "Gomez",
    19,
    new DateTime(1994, 5, 15),
    listaAficiones
);

Console.WriteLine(lucia);

var dto = lucia.ToDto();
Console.WriteLine(dto);

var persona = dto.ToModel();
Console.WriteLine(persona);