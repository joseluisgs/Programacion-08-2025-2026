var rutaArchivo = "documento_completo.txt";

// Crear archivo
CrearFichero(rutaArchivo);
// Mostrar información del archivo
MostrarInformacionFichero(rutaArchivo);
// Copiar archivo
CopiarFichero(rutaArchivo, "copia_documento.txt");
// Mover archivo
MoverFichero(rutaArchivo, "archivo_movido.txt");
// Abrir para lectura
AbrirFichero("documento_movido.txt");
// Eliminar archivo
EliminarFichero("documento_movido.txt");
EliminarFichero("copia_documento.txt");

Console.WriteLine("\n>>> FIN DE PROGRAMA");
Console.WriteLine("El documento de ejemplo se ha creado correctamente.");
Console.WriteLine("\n═══════════════════════════════════════════");

void CrearFichero(string archivo) {
// Crear ficheo de prueba
    File.WriteAllText(archivo, "Este es un documento de ejemplo.\nSegunda línea.");
}

void MostrarInformacionFichero(string archivo) {
// Crear instancia de FileInfo
    var fileInfo = new FileInfo(archivo);

    Console.WriteLine("═══════════════════════════════════════════");
    Console.WriteLine("  INFORMACIÓN COMPLETA DEL FICHERO");
    Console.WriteLine("═══════════════════════════════════════════\n");

// PROPIEDADES BÁSICAS
    Console.WriteLine(">>> PROPIEDADES BÁSICAS");
    Console.WriteLine($"Nombre:               {fileInfo.Name}");
    Console.WriteLine($"Nombre sin extensión: {Path.GetFileNameWithoutExtension(fileInfo.Name)}");
    Console.WriteLine($"Extensión:           {fileInfo.Extension}");
    Console.WriteLine($"Ruta completa:       {fileInfo.FullName}");
    Console.WriteLine($"Directorio:          {fileInfo.DirectoryName}");

// TAMAÑO Y FECHAS
    Console.WriteLine("\n>>> TAMAÑO Y FECHAS");
    Console.WriteLine($"Tamaño:              {fileInfo.Length} bytes");
    Console.WriteLine($"Creado:              {fileInfo.CreationTime: dd/MM/yyyy HH:mm:ss}");
    Console.WriteLine($"Modificado:          {fileInfo.LastWriteTime:dd/MM/yyyy HH:mm:ss}");
    Console.WriteLine($"Último acceso:       {fileInfo.LastAccessTime:dd/MM/yyyy HH:mm:ss}");

// ATRIBUTOS
    Console.WriteLine("\n>>> ATRIBUTOS");
    Console.WriteLine($"¿Es solo lectura?    {fileInfo.IsReadOnly}");
    Console.WriteLine($"Atributos completos: {fileInfo.Attributes}");

// Verificar atributos específicos
    var esOculto = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
    var esSistema = (fileInfo.Attributes & FileAttributes.System) == FileAttributes.System;

    Console.WriteLine($"¿Es oculto?          {esOculto}");
    Console.WriteLine($"¿Es de sistema?      {esSistema}");

// OPERACIONES
    Console.WriteLine("\n>>> OPERACIONES");
}

// Copiar

void CopiarFichero(string origen, string destino) {
    var fileInfo = new FileInfo(origen);
    var copia = fileInfo.CopyTo("copia_documento.txt", true);
    Console.WriteLine($"✓ Copiado a:          {copia.FullName}");
}

void MoverFichero(string origen, string destino) {
// Mover
    var fileInfo = new FileInfo(origen);
    fileInfo.MoveTo("documento_movido.txt");
    Console.WriteLine($"✓ Movido a:          {fileInfo.FullName}");
}

// Abrir para lectura, te devuelve un Stream
void AbrirFichero(string archivo) {
    var fileInfo = new FileInfo(archivo);
    using var stream = fileInfo.OpenRead();

    // Cogemos el stream y lo leemos con un StreamReader para mostrar la primera línea
    using var reader = new StreamReader(stream);
    var primeraLinea = reader.ReadLine() ?? "";
    Console.WriteLine($"Primera línea:        {primeraLinea}");
}

// Eliminar
void EliminarFichero(string archivo) {
    var fileInfo = new FileInfo(archivo);
    // Eliminar el fichero
    fileInfo.Delete();
    Console.WriteLine("✓ Fichero eliminado");
    // Limpiar copia
    File.Delete("copia_documento.txt");
}