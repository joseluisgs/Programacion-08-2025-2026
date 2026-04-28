// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hola Directorios!");

string ruta = "Directorio";

CrearDirectorio(ruta);

InformacionDirectorio(ruta);

CrearFicheros(ruta);

InformacionDirectorio(ruta);

BorrarDirectorio(ruta);


void CrearDirectorio(string ruta) {
    // Directorio en donde estamos ejecutando el programa
    string currentDirectory = Directory.GetCurrentDirectory();

    // Ruta completa del directorio a crear
    string fullPath = Path.Combine(currentDirectory, ruta);

    // Crea el directorio si no existe, de lo contrario, no hace nada
    try {
        Directory.CreateDirectory(fullPath);
        Console.WriteLine($"El directorio '{fullPath}' ha sido creado exitosamente.");
    }
    catch (Exception e) {
        Console.WriteLine($"Error al crear el directorio '{fullPath}': {e.Message}");
    }
}

void InformacionDirectorio(string ruta) {
    string currentDirectory = Directory.GetCurrentDirectory();
    string fullPath = Path.Combine(currentDirectory, ruta);

    if (Directory.Exists(fullPath)) {
        DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
        Console.WriteLine($"Información del directorio '{fullPath}':");
        Console.WriteLine($"- Nombre: {dirInfo.Name}");
        Console.WriteLine($"- Ruta completa: {dirInfo.FullName}");
        Console.WriteLine($"- Fecha de creación: {dirInfo.CreationTime}");
        Console.WriteLine($"- Última modificación: {dirInfo.LastWriteTime}");
        Console.WriteLine($"- Numero de archivos: {dirInfo.GetFiles().Length}");
    } else {
        Console.WriteLine($"El directorio '{fullPath}' no existe.");
    }
}

void CrearFicheros(string ruta) {
    string currentDirectory = Directory.GetCurrentDirectory();
    string fullPath = Path.Combine(currentDirectory, ruta);

    if (Directory.Exists(fullPath)) {
        DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
        
        // Creamos diez archivos de texto dentro del directorio
        for (int i = 1; i <= 10; i++) {
            string fileName = $"Archivo{i}.txt";
            string filePath = Path.Combine(fullPath, fileName);
            try {
                // Crea el archivo y escribe algo de texto
                File.WriteAllText(filePath, $"Este es el contenido del {fileName}");
                Console.WriteLine($"Archivo '{fileName}' creado exitosamente.");
            }
            catch (Exception e) {
                Console.WriteLine($"Error al crear el archivo '{fileName}': {e.Message}");
            }
        }
    } else {
        Console.WriteLine($"El directorio '{fullPath}' no existe.");
    }
}

void BorrarDirectorio(string ruta) {
    string currentDirectory = Directory.GetCurrentDirectory();
    string fullPath = Path.Combine(currentDirectory, ruta);

    if (Directory.Exists(fullPath)) {
        try {
            // Borramos el directorio y todo su contenido
            Directory.Delete(fullPath, true);
            Console.WriteLine($"El directorio '{fullPath}' ha sido eliminado exitosamente.");
        }
        catch (Exception e) {
            Console.WriteLine($"Error al eliminar el directorio '{fullPath}': {e.Message}");
        }
    } else {
        Console.WriteLine($"El directorio '{fullPath}' no existe.");
    }
}