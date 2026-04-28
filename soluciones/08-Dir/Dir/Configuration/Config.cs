namespace Dir.Configuration;

public static class Config {
    // Si no se cambia, usamos el directorio donde se ejecuta el programa
    public static string DirectoryPath { get; set; } = Directory.GetCurrentDirectory(); 
    public static bool Force { get; set; } = false;
}