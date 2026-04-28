// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hola Streams");


WriteToFile();

ReadFromFileLineByLine();

ReadFromFileAllAtOnce();


void WriteToFile() {
    // Stream: secuencia de bytes que se pueden leer o escribir de forma secuencial
    // StreamWriter: clase que permite escribir en un stream de texto forma sencilla
    // Sobre el fichero, si no existe se crea, si existe se sobreescribe entero!
    using var writer = new StreamWriter("file.txt");
    writer.WriteLine("Hola StreamWriter");
    writer.WriteLine("Adiós StreamWriter");
}

void ReadFromFileLineByLine() {
// StreamReader: clase que permite leer de un stream de texto forma sencilla
    using var reader = new StreamReader("file.txt");

    Console.WriteLine("Líneas leídas de file.txt:");

// Leer línea a línea y mostrarlas en la consola
// Mientras no se haya llegado al final del stream,
// se lee una línea y se muestra en la consola
    while (!reader.EndOfStream) {
        var line = reader.ReadLine();
        Console.WriteLine(line);
    }
}

void ReadFromFileAllAtOnce() {
    using var reader = new StreamReader("file.txt");
// Otra forma es leer todo el contenido del stream de una sola vez
// y luego mostrarlo en la consola

    Console.WriteLine("Contenido de todo file.txt:");
    var content = reader.ReadToEnd();
    Console.WriteLine(content);
}