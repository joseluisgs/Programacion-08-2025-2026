// See https://aka.ms/new-console-template for more information

Console.WriteLine("Ficheros y directorios temporales");

// Obtener directorio temporal
var tempPath = Path.GetTempPath();
Console.WriteLine($"Temporal: {tempPath}");

// Crear nombre de fichero temporal único
var tempFile = Path.GetTempFileName();
Console.WriteLine($"Temp: {tempFile}");

// Escribir y usar
File.WriteAllText(tempFile, "Datos temporales");
var contenido = File.ReadAllText(tempFile);
Console.WriteLine($"Contenido: {contenido}");

// Eliminar
if (File.Exists(tempFile))
    File.Delete(tempFile);

// Nombre aleatorio sin crear el fichero
var nombre = Path.Combine(
    Path.GetTempPath(),
    $"proceso_{Guid.NewGuid()}.tmp"
);
Console.WriteLine($"Nombre: {nombre}");

// Eliminar
if (File.Exists(nombre))
    File.Delete(nombre);

// Crear directorio temporal
var tempDir = Path.Combine(Path.GetTempPath(), $"dir_{Guid.NewGuid()}");
Directory.CreateDirectory(tempDir);
Console.WriteLine($"Directorio: {tempDir}");

// Eliminar
if (Directory.Exists(tempDir))
    Directory.Delete(tempDir);

{
    var tempFileName = Path.GetTempFileName();

// Al usar FileOptions.DeleteOnClose, el archivo se borra al cerrar el flujo
    using var stream = new FileStream(tempFileName,
        FileMode.Open,
        FileAccess.ReadWrite,
        FileShare.None,
        4096,
        FileOptions.DeleteOnClose);
    stream.WriteByte(0x42); // Escribir algo para usar el archivo
    Console.WriteLine($"Archivo temporal en uso: {tempFileName}");
// Al salir del bloque using, el archivo se eliminará automáticamente
}

// Luego de este bloque, el archivo se ha borrado
Console.WriteLine("Fin");