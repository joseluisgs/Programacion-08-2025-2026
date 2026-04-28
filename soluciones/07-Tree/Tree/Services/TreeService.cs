using System.Text;

namespace Tree.Services;

public static class TreeService {
    public static void Run() {
        // Recorremos el directorio y procesamos los archivos
        if (string.IsNullOrEmpty(Configuration.Config.DirectoryPath)) {
            Console.WriteLine(
                "No se ha establecido un directorio para procesar. Por favor, establezca un directorio válido.");
            return;
        }

        Console.WriteLine($"Processing directory: {Configuration.Config.DirectoryPath}");
        // Procesamos recursivamente el comando tree, si existe el fichero lo guardamos en el fichero
        // Si no existe el fichero de salida de config lo mostramos en consola
        var outputPath = Configuration.Config.FileOutputPath;
        // Es una búsqueda recursiva, recorremos el contenido del directorio y sus subdirectorios
        var treeOutput = ProcessDirectory(Configuration.Config.DirectoryPath, "");
        
        if (!string.IsNullOrEmpty(outputPath)) {
            try {
                File.WriteAllText(outputPath, treeOutput);
                Console.WriteLine($"Salida salvada en: {outputPath}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error escribiendo fichero: {ex.Message}");
                return;
            }
        }
        else {
            Console.WriteLine(treeOutput);
        }

        Console.WriteLine("Finalizando Tree.");
    }

    private static string ProcessDirectory(string directoryPath, string indent) {
        var output = new StringBuilder();

        var files = Directory.GetFiles(directoryPath);
        var directories = Directory.GetDirectories(directoryPath);

        // Procesar Archivos
        foreach (var file in files) {
            var fileName = Path.GetFileName(file);
            if (!string.IsNullOrEmpty(fileName)) {
                output.AppendLine($"{indent}├── {fileName}"); // Añadí el prefijo para que visualmente sigan la rama
            }
        }

        // Procesar Directorios
        foreach (var directory in directories) {
            var directoryName = Path.GetFileName(directory);
            if (!string.IsNullOrEmpty(directoryName)) {
                output.AppendLine($"{indent}├── {directoryName}");
            }
        
            // ¡CAMBIO AQUÍ! Usamos Append en lugar de AppendLine
            output.Append(ProcessDirectory(directory, indent + "│   "));
        }
    
        return output.ToString();
    }
}
