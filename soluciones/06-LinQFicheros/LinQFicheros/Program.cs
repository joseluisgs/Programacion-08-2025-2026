// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hola LinQ Ficheros!");

Directory.CreateDirectory("Archivos");
File.WriteAllText("Archivos/pequeño.txt", "x");           // 1 byte
File.WriteAllText("Archivos/mediano.txt", new string('x', 1000)); // 1 KB
File.WriteAllText("Archivos/grande.txt", new string('x', 10000)); // 10 KB

// Ficheros mayores de 5 KB
var grandes = new DirectoryInfo("Archivos")
    .GetFiles("*", SearchOption.AllDirectories)
    .Where(f => f.Length > 5 * 1024)
    .Select(f => new { f.Name, SizeKB = f.Length / 1024.0 });

Console.WriteLine(">>> Ficheros mayores de 5 KB:");
grandes.ToList().ForEach(g => Console.WriteLine($"{g.Name}: {g.SizeKB:N2} KB"));


// Ficheros modificados en los últimos 7 días
var recientes = new DirectoryInfo("Archivos")
    .GetFiles("*", SearchOption.AllDirectories)
    .Where(f => f.LastWriteTime > DateTime.Now.AddDays(-7))
    .OrderByDescending(f => f.LastWriteTime);

Console.WriteLine(">>> Ficheros modificados en los últimos 7 días:");
    
// BÚSQUEDA COMPLEJA: 10 Imágenes JPG mayores de 1 MB, ordenadas por tamaño descendente
var imagenesGrandes = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
    .GetFiles("*.jpg", SearchOption.AllDirectories)
    .Where(f => f.Length > 1 * 1024 * 1024)
    .OrderByDescending(f => f.Length)
    .Take(10)
    .Select(f => new 
    { 
        f.Name, 
        SizeMB = f.Length / (1024.0 * 1024.0),
        f.LastWriteTime 
    });

Console.WriteLine(">>> Top 10 imágenes JPG mayores de 1 MB:");
imagenesGrandes.ToList().ForEach(img => 
    Console.WriteLine($"{img.Name}: {img.SizeMB:N2} MB, Modificado: {img.LastWriteTime}"));
    
// AGRUPAR FICHEROS POR EXTENSIÓN
var grouped = new DirectoryInfo("Archivos")
    .GetFiles("*", SearchOption.AllDirectories)
    .GroupBy(f => f.Extension.ToUpper())
    .OrderByDescending(g => g.Sum(f => f.Length));

Console.WriteLine(">>> Espacios por extensión:");
grouped.ToList().ForEach(g => 
    Console.WriteLine($"{g.Key}: {g.Sum(f => f.Length) / 1024.0:N2} KB, Count: {g.Count()}"));