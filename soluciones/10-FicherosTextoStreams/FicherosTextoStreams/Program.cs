// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hola Ficheros Texto con Streams");

var path = "texto.txt";
EscribirArchivo(path);
LeerArchivo(path);
AñadirArchivo(path);
LeerArchivo(path);
ExperimentoBorrarLinea(path);
LeerArchivo(path);


void EscribirArchivo(string fichero) {
    // ideales para ficheros grandes
    using var stream = new StreamWriter(fichero);
    stream.WriteLine("Hola 1 DAW");
    stream.WriteLine("Linea 1");
    stream.WriteLine("Linea 2");
    stream.Write("Linea 3" + Environment.NewLine);
}

void LeerArchivo(string fichero) {
    //  Ideales para leer ficheros grandes
    using var stream = new StreamReader(fichero);
   
    // Leemos el archivo línea por línea hasta el final del archivo
    Console.WriteLine("Contenido del archivo hasta EndOfStream:");
    while (!stream.EndOfStream) {
        Console.WriteLine(stream.ReadLine());
    }
    
    using var stream2 = new StreamReader(fichero);
    
    // O leerlo todo de una vez (no recomendado para archivos grandes)
    Console.WriteLine("Contenido del archivo línea a línea:");
    while (stream2.ReadLine() is { } linea) {
        Console.WriteLine(linea);
        // Opcionalmente, podemos procesar la línea antes de mostrarla
        //...
    }
    
    using var stream3 = new StreamReader(fichero);
    
    // O leerlo todo de una vez (no recomendado para archivos grandes)
    Console.WriteLine("Contenido del archivo todo:");
    var contenido = stream3.ReadToEnd();
    Console.WriteLine(contenido);
}

void AñadirArchivo(string fichero) {
    // Para añadir contenido a un archivo existente, podemos usar el modo de apertura Append
    using var stream = new StreamWriter(fichero, append: true);
    stream.WriteLine("Linea añadida");
}

void ExperimentoBorrarLinea(string fichero) {
    var tempFile = "temp.txt";

    // Creamos un bloque para que al finalizar, se cierren los archivos
    {
        using var reader = new StreamReader(fichero);
        using var writer = new StreamWriter(tempFile);
        
        while (reader.ReadLine() is { } linea) {
            if (!linea.Contains("DAW", StringComparison.CurrentCultureIgnoreCase)) {
                writer.WriteLine(linea);
            }
        }
    } // <-- Aquí el reader y el writer se cierran automáticamente
    // Si no debes poner los .Close() o .Dispose() explícitamente, el bloque using se encarga de eso por nosotros

    // Ahora que están cerrados, ya podemos mover/reemplazar
    if (File.Exists(fichero)) {
        File.Delete(fichero); // Es más seguro borrarlo o usar el overload de Move con overwrite
    }
    File.Move(tempFile, fichero);
}

