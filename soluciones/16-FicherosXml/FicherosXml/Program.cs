// See https://aka.ms/new-console-template for more information

using System.Xml.Serialization;
using FicherosCsv.Dto;
using FicherosCsv.Mappers;
using FicherosCsv.Models;

Console.WriteLine("Hola Ficheros XML!");


// Crear datos de prueba
var alumnos = new List<Estudiante> {
    new() { Id = 1, Nombre = "Ana Gómez", Edad = 20, Nota = 7.5, Aprobado = true },
    new() { Id = 2, Nombre = "Juan Pérez", Edad = 22, Nota = 8.0, Aprobado = true },
    new() { Id = 3, Nombre = "María López", Edad = 21, Nota = 5.0, Aprobado = false },
    new() { Id = 4, Nombre = "Carlos Sánchez", Edad = 23, Nota = 6.0, Aprobado = true },
    new() { Id = 5, Nombre = "Lucía Fernández", Edad = 20, Nota = 4.5, Aprobado = false }
};

var estudiantesXml = "estudiantes.xml";

Console.WriteLine($"Exportando datos a {estudiantesXml}");
ExportarAXml(alumnos, estudiantesXml);

Console.WriteLine($"Importando datos de {estudiantesXml}");
var estudiantesImportados = ImportarDeXml(estudiantesXml);

Console.WriteLine("Estudiantes importados:");
estudiantesImportados.ForEach(estudiante =>
    Console.WriteLine(
        $"{estudiante.Id};{estudiante.Nombre};{estudiante.Edad};{estudiante.Nota};{estudiante.Aprobado}"));


return;

// Exportar datos a Json
void ExportarAXml(List<Estudiante> estudiantes, string ruta) {
    var dtos = estudiantes
        .Select(e => e.ToDto())
        .ToList();

    var serializer = new XmlSerializer(typeof(List<EstudianteDto>));
    using var stream = new StreamWriter(ruta);
    serializer.Serialize(stream, dtos);
}

List<Estudiante> ImportarDeXml(string ruta) {
    var serializer = new XmlSerializer(typeof(List<EstudianteDto>));
    using var stream = new StreamReader(ruta);
    var dtos = (List<EstudianteDto>)serializer.Deserialize(stream)!;
    return dtos.Select(dto => dto.ToModel()).ToList();
}