// See https://aka.ms/new-console-template for more information

using Tree.Configuration;
using Tree.Services;

Console.WriteLine("Comenzando con el comando Tree DAW...");
AnalizarArgumentos(args);
TreeService.Run();

void AnalizarArgumentos(string[] strings) {
    if (strings.Length > 0) {
        var directory = strings[0];
        if (Directory.Exists(directory)) {
            Console.WriteLine($"Leyendo directorio: {directory}");
            // Aquí puedes agregar la lógica para cargar y procesar el directorio
            Config.DirectoryPath = directory;
        } else {
            Console.WriteLine($"El directorio '{directory}' no existe. Por favor, introduzca un directorio válido:");
        }
        // Existe args[1] para la ruta de salida del archivo
        if (strings.Length > 1) {
            var outputPath = strings[1];
            Console.WriteLine($"El fichero de salida es: {outputPath}");
            Config.FileOutputPath = outputPath;
        }
    } else {
        Console.WriteLine("No se han proporcionado argumentos, introduzca un directorio válido:");
    }
}