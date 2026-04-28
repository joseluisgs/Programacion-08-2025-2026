using System.Globalization;

namespace GestionAcademica.Config;

/// <summary>
///     Clase estática que contiene las configuraciones globales para la gestión académica.
/// </summary>
public static class Configuracion {
    public static readonly double NotaAprobado = 5.00;
    public static readonly CultureInfo Locale = CultureInfo.GetCultureInfo("es-ES");
    public static readonly string DataFolder = Path.Combine(Environment.CurrentDirectory, "data");

    // Diccionario con los archivos de almacenamiento disponibles
    // La clave es el tipo de archivo y el valor es la ruta completa
    private static readonly Dictionary<string, string> Files = new() {
        { "txt", Path.Combine(DataFolder, "academia.txt") },
        { "csv", Path.Combine(DataFolder, "academia.csv") },
        { "json", Path.Combine(DataFolder, "academia.json") },
        { "xml", Path.Combine(DataFolder, "academia.xml") },
        { "csv-alt", Path.Combine(DataFolder, "academia-alt.csv") }
    };

    // Propiedad que devuelve el archivo por defecto (csv)
    public static string AcademiaFile => Files["xml"];

    // Método para obtener la ruta de un archivo por su clave
    // public static string GetFile(string key) => Files[key];
}