// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hola Path!");

string path = @"C:\Users\Usuario\Desktop\Curso C#\Path\Path\bin\Debug\net6.0\file.txt";
Console.WriteLine("DirectoryName: " + Path.GetDirectoryName(path));
Console.WriteLine("FileName: " + Path.GetFileName(path));
Console.WriteLine("FileNameWithoutExtension: " + Path.GetFileNameWithoutExtension(path));
Console.WriteLine("Extension: " + Path.GetExtension(path));
Console.WriteLine("GetTempPath: " + Path.GetTempPath());
Console.WriteLine("GetTempFileName: " + Path.GetTempFileName());
Console.WriteLine("Raíz: " + Path.GetPathRoot(path));

// Combinar rutas
string path1 = Path.Combine(Environment.CurrentDirectory, "file.txt");
Console.WriteLine("Combined Path: " + path1);