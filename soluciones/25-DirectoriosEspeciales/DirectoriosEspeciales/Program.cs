// See https://aka.ms/new-console-template for more information

Console.WriteLine("Directorios especiales en C#");

// Directorio de Ejecución
string directorioEjecucion = Environment.CurrentDirectory;
Console.WriteLine($"Directorio de Ejecución: {directorioEjecucion}");

// Directorio Base
string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
Console.WriteLine($"Directorio Base: {directorioBase}");

// Carpeta De Datos de Aplicación Local
string carpetaDatosLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
Console.WriteLine($"Carpeta de Datos de Aplicación Local: {carpetaDatosLocal}");

// Carpeta De Datos de Aplicación Roaming
string carpetaDatosRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
Console.WriteLine($"Carpeta de Datos de Aplicación Roaming: {carpetaDatosRoaming}");

// Carpeta De Documentos
string carpetaDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
Console.WriteLine($"Carpeta de Documentos: {carpetaDocumentos}");

// Carpeta De Escritorio
string carpetaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
Console.WriteLine($"Carpeta de Escritorio: {carpetaEscritorio}");

// Carpeta del Usuario
string carpetaUsuario = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
Console.WriteLine($"Carpeta del Usuario: {carpetaUsuario}");

// Carpeta De Imágenes
string carpetaImagenes = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
Console.WriteLine($"Carpeta de Imágenes: {carpetaImagenes}");

// Carpeta De Descargas
string carpetaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
Console.WriteLine($"Carpeta de Descargas: {carpetaDescargas}");

// Carpeta De Música
string carpetaMusica = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
Console.WriteLine($"Carpeta de Música: {carpetaMusica}");

// Carpeta De Videos
string carpetaVideos = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
Console.WriteLine($"Carpeta de Videos: {carpetaVideos}");

// Carpeta De Programas (Archivos de Programa)
string carpetaProgramas = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
Console.WriteLine($"Carpeta de Programas: {carpetaProgramas}");

// Carpeta temporal
string carpetaTemporal = Path.GetTempPath();
Console.WriteLine($"Carpeta Temporal: {carpetaTemporal}");

// Internet Cache
string carpetaCacheInternet = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
Console.WriteLine($"Carpeta de Cache de Internet: {carpetaCacheInternet}");

// Fuentes
string carpetaFuentes = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
Console.WriteLine($"Carpeta de Fuentes: {carpetaFuentes}");

// Menú inicio
string carpetaMenuInicio = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
Console.WriteLine($"Carpeta del Menú de Inicio: {carpetaMenuInicio}");

// Carpeta de Programas del Usuario
string carpetaProgramasUsuario = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
Console.WriteLine($"Carpeta de Programas del Usuario: {carpetaProgramasUsuario}");
