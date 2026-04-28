// See https://aka.ms/new-console-template for more information

using SerializacionObjetosBinario.Models;

Console.WriteLine("Hola Objetos serializados en binario en C#!");

const string RutaArchivo = "persona.bin";

var personas = new List<Persona> {
    new("Juan", 30),
    new("María", 25),
    new("Pedro", 40)
};

EscribirObjetosBinarios(RutaArchivo, personas);
LeerObjetosBinarios(RutaArchivo);
BorrarArchivo(RutaArchivo);

void EscribirObjetosBinarios(string rutaArchivo, List<Persona> listaPersonas) {
    using var stream = new FileStream(rutaArchivo, FileMode.Create);
    using var writer = new BinaryWriter(stream);

    // Escribimos la cantidad de personas para saber cuántas leer después
    writer.Write(listaPersonas.Count);

    // Escribimos cada persona, campo por campo
    foreach (var persona in listaPersonas) {
        writer.Write(persona.Nombre);
        writer.Write(persona.Edad);
    }

    Console.WriteLine($"Personas escritas en el archivo '{rutaArchivo}' en formato binario.");
}

void LeerObjetosBinarios(string rutaArchivo) {
    using var stream = new FileStream(rutaArchivo, FileMode.Open);
    using var reader = new BinaryReader(stream);

    // Leemos la cantidad de personas
    var cantidadPersonas = reader.ReadInt32();

    Console.WriteLine($"Cantidad de personas: {cantidadPersonas} leídas del archivo '{rutaArchivo}'.");

    var personasLeidas = new List<Persona>();

    // Leemos cada persona, campo por campo
    for (var i = 0; i < cantidadPersonas; i++) {
        var nombre = reader.ReadString();
        var edad = reader.ReadInt32();
        Console.WriteLine($"Persona {i + 1}: Nombre={nombre}, Edad={edad}");
        personasLeidas.Add(new Persona(nombre, edad));
    }

    Console.WriteLine("Personas leidas:");
    foreach (var persona in personasLeidas)
        Console.WriteLine($"Nombre={persona.Nombre}, Edad={persona.Edad}");
}

void BorrarArchivo(string rutaArchivo) {
    if (File.Exists(rutaArchivo)) {
        File.Delete(rutaArchivo);
        Console.WriteLine($"Archivo '{rutaArchivo}' borrado.");
    }
    else {
        Console.WriteLine($"Archivo '{rutaArchivo}' no encontrado para borrar.");
    }
}