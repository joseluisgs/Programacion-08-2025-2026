// See https://aka.ms/new-console-template for more information

using System.IO.Compression;

Console.WriteLine("Programa de compresión y descompresión de archivos ZIP.");

// --- Flujo Principal ---
const string RutaZip = "ficheros.zip";
const string RutaDescomprimida = "ficheros_descomprimidos";

PrepararEntorno(RutaZip, RutaDescomprimida);

CrearArchivosDePrueba();
CrearZip(RutaZip);
// Environment.Exit(0); // Salir después de crear el ZIP para evitar errores de archivos bloqueados en ejecuciones posteriores
ExtraerZipCompleto(RutaZip, RutaDescomprimida);
ExtraerArchivoEspecifico(RutaZip, "fichero1.txt", "fichero1_extraido.txt");
LeerArchivoInterno(RutaZip, "fichero2.txt");
ModificarZip(RutaZip, "fichero3.txt", "Este es el contenido del fichero 3.");
EliminarArchivoDeZip(RutaZip, "fichero2.txt");
AñadirArchivoAdicional(RutaZip, "fichero4.txt", "Este es el contenido del fichero 4.");
ReemplazarArchivoEnZip(RutaZip, "fichero1.txt", "Este es el nuevo contenido del fichero 1.");
ObtenerListaDeArchivosEnZip(RutaZip);

LimpiarTodo(RutaZip, RutaDescomprimida);

// --- Funciones ---

void PrepararEntorno(string zip, string directorio) {
    // Evita el error de "File already exists" borrando lo anterior
    if (File.Exists(zip))
        File.Delete(zip);
    if (Directory.Exists(directorio))
        Directory.Delete(directorio, true);
    Console.WriteLine("Entorno preparado.");
}

void CrearArchivosDePrueba() {
    File.WriteAllText("fichero1.txt", "Este es el contenido del fichero 1.");
    File.WriteAllText("fichero2.txt", "Este es el contenido del fichero 2.");
    Console.WriteLine("Archivos de prueba creados.");
}

void CrearZip(string ficheroZip) {
    using var zipArchivo = ZipFile.Open(ficheroZip, ZipArchiveMode.Create);
    var ficherosTexto = Directory.GetFiles(Directory.GetCurrentDirectory(), "fichero*.txt");
    foreach (var ruta in ficherosTexto)
        zipArchivo.CreateEntryFromFile(ruta, Path.GetFileName(ruta));

    Console.WriteLine("ZIP creado con ficheros de texto.");
}

void ExtraerZipCompleto(string ficheroZip, string destino) {
    ZipFile.ExtractToDirectory(ficheroZip, destino);
    Console.WriteLine($"ZIP descomprimido en: {destino}");
}

void ExtraerArchivoEspecifico(string ficheroZip, string nombreInterno, string destinoFisico) {
    using var zip = ZipFile.OpenRead(ficheroZip);
    var entry = zip.GetEntry(nombreInterno);
    if (entry != null) {
        if (File.Exists(destinoFisico))
            File.Delete(destinoFisico);
        entry.ExtractToFile(destinoFisico);
        Console.WriteLine($"{nombreInterno} extraído individualmente.");
    }
}

void LeerArchivoInterno(string ficheroZip, string nombreInterno) {
    using var zip = ZipFile.OpenRead(ficheroZip);
    var entry = zip.GetEntry(nombreInterno);
    if (entry != null) {
        using var stream = entry.Open();
        using var lector = new StreamReader(stream);
        Console.WriteLine($"Contenido de {nombreInterno} leído desde el ZIP: {lector.ReadToEnd()}");
    }
}

void ModificarZip(string ficheroZip, string nuevoNombre, string contenido) {
    // Crear archivo temporal para añadirlo
    File.WriteAllText(nuevoNombre, contenido);

    using var zipAppend = ZipFile.Open(ficheroZip, ZipArchiveMode.Update);
    zipAppend.CreateEntryFromFile(nuevoNombre, nuevoNombre);

    Console.WriteLine($"{nuevoNombre} añadido al ZIP.");
}

void EliminarArchivoDeZip(string ficheroZip, string nombreBorrar) {
    using var zip = ZipFile.Open(ficheroZip, ZipArchiveMode.Update);
    var entry = zip.GetEntry(nombreBorrar);
    entry?.Delete();
    Console.WriteLine($"{nombreBorrar} eliminado del ZIP.");
}

void AñadirArchivoAdicional(string zip, string nombreAdicional, string contenido) {
    // Crear un nuevo archivo temporal con el contenido adicional
    File.WriteAllText(nombreAdicional, contenido);

    // Abrir el ZIP en modo actualización para añadir el nuevo archivo
    using var zipAppend = ZipFile.Open(zip, ZipArchiveMode.Update);
    zipAppend.CreateEntryFromFile(nombreAdicional, nombreAdicional);

    Console.WriteLine($"{nombreAdicional} añadido al ZIP.");
}

void ReemplazarArchivoEnZip(string zip, string nombreReemplazar, string nuevoContenido) {
    // Crear un nuevo archivo temporal con el nuevo contenido
    File.WriteAllText(nombreReemplazar, nuevoContenido);

    // Abrir el ZIP en modo actualización para reemplazar el archivo
    using var zipAppend = ZipFile.Open(zip, ZipArchiveMode.Update);
    var entry = zipAppend.GetEntry(nombreReemplazar);
    entry?.Delete(); // Eliminar la entrada antigua si existe
    zipAppend.CreateEntryFromFile(nombreReemplazar, nombreReemplazar); // Añadir la nueva entrada

    Console.WriteLine($"{nombreReemplazar} reemplazado en el ZIP.");
}

void ObtenerListaDeArchivosEnZip(string zip) {
    using var zipArchivo = ZipFile.OpenRead(zip);
    Console.WriteLine("Archivos en el ZIP:");
    foreach (var entry in zipArchivo.Entries)
        Console.WriteLine($"- {entry.FullName}");
}

void LimpiarTodo(string zip, string directorio) {
    // Borramos los archivos sueltos y el zip para dejar la carpeta limpia
    File.Delete("fichero1.txt");
    File.Delete("fichero2.txt");
    File.Delete("fichero3.txt");
    File.Delete("fichero1_extraido.txt");
    File.Delete("fichero4.txt");
    File.Delete(zip);
    if (Directory.Exists(directorio))
        Directory.Delete(directorio, true); // Elimina la carpeta descomprimida y su contenido
    Console.WriteLine("Limpieza finalizada.");
}