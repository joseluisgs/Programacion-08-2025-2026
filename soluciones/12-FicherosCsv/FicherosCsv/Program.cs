// See https://aka.ms/new-console-template for more information

using FicherosCsv.Dto;
using FicherosCsv.Mappers;
using FicherosCsv.Models;

Console.WriteLine("Hola Ficheros CSV!");


// Crear datos de prueba
var alumnos = new List<Estudiante> {
    new() { Id = 1, Nombre = "Ana Gómez", Edad = 20, Nota = 7.5, Aprobado = true },
    new() { Id = 2, Nombre = "Juan Pérez", Edad = 22, Nota = 8.0, Aprobado = true },
    new() { Id = 3, Nombre = "María López", Edad = 21, Nota = 5.0, Aprobado = false },
    new() { Id = 4, Nombre = "Carlos Sánchez", Edad = 23, Nota = 6.0, Aprobado = true },
    new() { Id = 5, Nombre = "Lucía Fernández", Edad = 20, Nota = 4.5, Aprobado = false }
};

var rutaCsv = "estudiantes.csv";

Console.WriteLine($"Exportando datos a {rutaCsv}");
ExportaraCsv(alumnos, rutaCsv);

Console.WriteLine("Importando datos de {rutaCsv}");
var estudiantesImportados = ImportarDeCsv(rutaCsv);

Console.WriteLine("Estudiantes importados:");
estudiantesImportados.ForEach(estudiante =>
    Console.WriteLine(
        $"{estudiante.Id};{estudiante.Nombre};{estudiante.Edad};{estudiante.Nota};{estudiante.Aprobado}"));


return;

// Exportar datos a CSV
void ExportaraCsv(List<Estudiante> estudiantes, string ruta) {
    using var writer = new StreamWriter(ruta);
    writer.WriteLine("Id;Nombre;Edad;Nota;Aprobado");
    estudiantes
        // Convertir cada Estudiante a EstudianteDto
        .Select(estudiante => estudiante.ToDto())
        .ToList()
        .ForEach(estudiante =>
            writer.WriteLine(
                $"{estudiante.Id};{estudiante.Nombre};{estudiante.Edad};{estudiante.Nota};{estudiante.Aprobado}"));
}

List<Estudiante> ImportarDeCsv(string ruta) {
    return File.ReadLines(ruta)
        .Skip(1) // Omitir la primera línea (encabezados)
        .Select(linea => linea.Split(';')) // Dividir cada línea en campos
        // Convertir cada array de campos en un EstudianteDto
        .Select(campos => new EstudianteDto(
            int.Parse(campos[0]),
            campos[1],
            int.Parse(campos[2]),
            double.Parse(campos[3]),
            campos[4]
        ))
        // Convertir cada EstudianteDto en un Estudiante
        .Select(dto => dto.ToModel())
        .ToList();
}