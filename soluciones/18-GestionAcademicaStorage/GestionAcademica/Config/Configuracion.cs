using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace GestionAcademica.Config;

/// <summary>
///     Clase estática que contiene las configuraciones globales para la gestión académica.
///     Ahora lee los parámetros desde appsettings.json para mayor flexibilidad.
/// </summary>
public static class Configuracion {
    private static readonly IConfiguration Config;

    static Configuracion() {
        // NOTA PARA EL ALUMNO: Cargamos la configuración desde el archivo JSON externo.
        // Esto permite cambiar el tipo de almacenamiento o la ruta sin recompilar el código.
        Config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static double NotaAprobado => Config.GetValue<double>("Academica:NotaAprobado");
    public static CultureInfo Locale => CultureInfo.GetCultureInfo("es-ES");
    public static string DataFolder => Path.Combine(Environment.CurrentDirectory, "data");

    // Propiedad que devuelve el tipo de almacenamiento configurado
    public static string StorageType => Config.GetValue<string>("Storage:Type") ?? "json";

    // Propiedad que devuelve la ruta del archivo configurado deduciendo la extensión según el tipo
    public static string AcademiaFile {
        get {
            var extension = StorageType.ToLower() switch {
                "json" => "json",
                "xml" => "xml",
                "csv" or "csv-alt" => "csv",
                "txt" or "text" => "txt",
                "binary" => "bin",
                _ => "json" // valor por defecto
            };
            return Path.Combine(DataFolder, $"academia.{extension}");
        }
    }
}