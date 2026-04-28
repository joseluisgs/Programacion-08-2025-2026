// See https://aka.ms/new-console-template for more information

var fichero = "Hola.txt";


Console.WriteLine("Hola File!");

ExisteArchivo(fichero);
EscribirArchivo(fichero, "Hola, mundo!");
ExisteArchivo(fichero);
AñadirAlArchivo(fichero, "¡Bienvenido/a a la programación de archivos en C#!");
LeerArchivoCompleto(fichero);
LeerArchivoLineaPorLinea(fichero);
LeerArchivoInumerable(fichero);
CopiarArchivo(fichero, "Copia.txt");
MetadatosArchivo(fichero);
RenombrarArchivo(fichero, "Hola_Mundo.txt");
EliminarArchivo(fichero);


void EscribirArchivo(string fileName, string content) {
    // Crea o sobrescribe el archivo con el contenido proporcionado
    try {
        File.WriteAllText(fileName, content);
        Console.WriteLine($"El archivo '{fileName}' ha sido creado con el contenido: {content}");
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al escribir el archivo '{fileName}': {ex.Message}");
    }
}

void ExisteArchivo(string fileName) {
    Console.WriteLine(
        File.Exists(fileName) ? $"El archivo '{fileName}' existe." : $"El archivo '{fileName}' no existe.");
}

void AñadirAlArchivo(string fileName, string content) {
    try {
        // Añade el contenido al final del archivo
        // Environ.NewLine se utiliza para agregar una nueva línea después del contenido añadido
        // Es independiente del sistema operativo, por lo que funcionará correctamente tanto en Windows como en Linux o macOS
        File.AppendAllText(fileName, content + Environment.NewLine);
        Console.WriteLine($"El contenido ha sido añadido al archivo '{fileName}': {content}");
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al añadir al archivo '{fileName}': {ex.Message}");
    }
}

void LeerArchivoCompleto(string fileName) {
    try {
        // Lee el contenido del archivo y lo muestra en la consola
        var content = File.ReadAllText(fileName);
        Console.WriteLine($"Contenido del archivo '{fileName}':");
        Console.WriteLine(content);
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al leer el archivo '{fileName}': {ex.Message}");
    }
}

void LeerArchivoLineaPorLinea(string fileName) {
    try {
        // Lee el contenido del archivo línea a línea y lo muestra en la consola
        Console.WriteLine($"Contenido del archivo '{fileName}' línea por línea:");
        foreach (var line in File.ReadLines(fileName))
            Console.WriteLine(line);
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al leer el archivo '{fileName}': {ex.Message}");
    }
}

void LeerArchivoInumerable(string fileName) {
    try {
        // Lee el contenido del archivo como un enumerable de líneas y lo muestra en la consola
        Console.WriteLine($"Contenido del archivo '{fileName}' como enumerable:");
        File.ReadLines(fileName)
            .Select(line => line.ToUpper())
            .ToList()
            .ForEach(Console.WriteLine);
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al leer el archivo '{fileName}': {ex.Message}");
    }
}

void EliminarArchivo(string fileName) {
    // Elimina el archivo si existe
    if (File.Exists(fileName))
        try {
            File.Delete(fileName);
            Console.WriteLine($"El archivo '{fileName}' ha sido eliminado.");
        }
        catch (Exception ex) {
            Console.WriteLine($"Error al eliminar el archivo '{fileName}': {ex.Message}");
        }
    else
        Console.WriteLine($"El archivo '{fileName}' no existe, no se puede eliminar.");
}

void CopiarArchivo(string sourceFileName, string destinationFileName) {
    // Copia el archivo de origen al destino
    try {
        File.Copy(sourceFileName, destinationFileName, true);
        Console.WriteLine($"El archivo '{sourceFileName}' ha sido copiado a '{destinationFileName}'.");
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al copiar el archivo '{sourceFileName}' a '{destinationFileName}': {ex.Message}");
    }
}

void RenombrarArchivo(string fileName, string newFileName) {
    // Renombra el archivo
    try {
        if (File.Exists(newFileName))
            File.Delete(newFileName); // Si el nuevo nombre ya existe, lo elimina para renombrarlo
        File.Move(fileName, newFileName);
        Console.WriteLine($"El archivo '{fileName}' ha sido renombrado a '{newFileName}'.");
        File.Move(newFileName, fileName); // Vuelve a renombrarlo para evitar conflictos
    }
    catch (Exception ex) {
        Console.WriteLine($"Error al renombrar el archivo '{fileName}' a '{newFileName}': {ex.Message}");
    }
}

void MetadatosArchivo(string fileName) {
    var fileInfo = File.GetAttributes(fileName);
    Console.WriteLine($"Metadatos del archivo '{fileName}':");
    Console.WriteLine($"- Tamaño: {new FileInfo(fileName).Length} bytes");
    Console.WriteLine($"- Fecha de creación: {File.GetCreationTime(fileName)}");
    Console.WriteLine($"- Fecha de última modificación: {File.GetLastWriteTime(fileName)}");
    Console.WriteLine($"- Atributos: {fileInfo}");
    Console.WriteLine();

    // Cambia los atributos del archivo
    Console.WriteLine("Es Oculto? " + fileInfo.HasFlag(FileAttributes.Hidden));
    Console.WriteLine("Es Solo Lectura? " + fileInfo.HasFlag(FileAttributes.ReadOnly));
    Console.WriteLine("Es Archivo? " + fileInfo.HasFlag(FileAttributes.Archive));
    Console.WriteLine("Es Directorio? " + fileInfo.HasFlag(FileAttributes.Directory));
    Console.WriteLine("Es Sistema? " + fileInfo.HasFlag(FileAttributes.System));
    Console.WriteLine("Es Temporal? " + fileInfo.HasFlag(FileAttributes.Temporary));
    Console.WriteLine();
}