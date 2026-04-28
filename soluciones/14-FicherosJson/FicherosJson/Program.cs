// See https://aka.ms/new-console-template for more information

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using FicherosCsv.Dto;
using FicherosCsv.Mappers;
using FicherosCsv.Models;

Console.WriteLine("Hola Ficheros Json!");


// Crear datos de prueba
var alumnos = new List<Estudiante> {
    new() { Id = 1, Nombre = "Ana Gómez", Edad = 20, Nota = 7.5, Aprobado = true },
    new() { Id = 2, Nombre = "Juan Pérez", Edad = 22, Nota = 8.0, Aprobado = true },
    new() { Id = 3, Nombre = "María López", Edad = 21, Nota = 5.0, Aprobado = false },
    new() { Id = 4, Nombre = "Carlos Sánchez", Edad = 23, Nota = 6.0, Aprobado = true },
    new() { Id = 5, Nombre = "Lucía Fernández", Edad = 20, Nota = 4.5, Aprobado = false }
};

var estudiantesJson = "estudiantes.json";

Console.WriteLine($"Exportando datos a {estudiantesJson}");
ExportarAJson(alumnos, estudiantesJson);

Console.WriteLine($"Importando datos de {estudiantesJson}");
var estudiantesImportados = ImportarDeJson(estudiantesJson);

Console.WriteLine("Estudiantes importados:");
estudiantesImportados.ForEach(estudiante =>
    Console.WriteLine(
        $"{estudiante.Id};{estudiante.Nombre};{estudiante.Edad};{estudiante.Nota};{estudiante.Aprobado}"));


return;

// Exportar datos a Json
void ExportarAJson(List<Estudiante> estudiantes, string ruta) {
    JsonSerializerOptions options = new() {
        WriteIndented = true, // Para que el JSON sea más legible, es el equivalente a "pretty print"
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Convierte las propiedades a camelCase en el JSON
        DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull, // Ignora las propiedades que son null al escribir el JSON
        //Converters = { new JsonStringEnumConverter() }, // Para serializar los enums como strings en lugar de números
        Encoder = JavaScriptEncoder
            .UnsafeRelaxedJsonEscaping // Permite caracteres especiales sin escaparlos, como acentos y eñes, lo cual es importante para el español
    };
    var dto = estudiantes.Select(e => e.ToDto());
    var json = JsonSerializer.Serialize(dto, options);
    File.WriteAllText(ruta, json);
}

List<Estudiante> ImportarDeJson(string ruta) {
    JsonSerializerOptions options = new() {
        WriteIndented = true, // Para que el JSON sea más legible, es el equivalente a "pretty print"
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Convierte las propiedades a camelCase en el JSON
        DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull, // Ignora las propiedades que son null al escribir el JSON
        //Converters = { new JsonStringEnumConverter() }, // Para serializar los enums como strings en lugar de números
        Encoder = JavaScriptEncoder
            .UnsafeRelaxedJsonEscaping // Permite caracteres especiales sin escaparlos, como acentos y eñes, lo cual es importante para el español
    };

    var json = File.ReadAllText(ruta);
    var listaDto = JsonSerializer.Deserialize<List<EstudianteDto>>(json, options);
    var estudiantes = listaDto?.Select(dto => dto.ToModel());
    return estudiantes?.ToList() ?? throw new Exception("No se pudieron importar los estudiantes");
}