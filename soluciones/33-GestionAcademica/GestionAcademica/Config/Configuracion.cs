using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace GestionAcademica.Config;

/// <summary>
///     Clase estática que contiene las configuraciones globales para la gestión académica.
///     NOTA PARA EL ALUMNO: Esta clase implementa el patrón "Service Locator" simplificado.
///     Centraliza toda la configuración del sistema en un solo lugar, leyendo de appsettings.json.
///     Permite cambiar comportamiento sin recompilar (ej: tipo de repositorio, storage, etc.).
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

    /// <summary>Nota mínima para aprobar (leyenda de la plataforma).</summary>
    public static double NotaAprobado => Config.GetValue<double>("Academica:NotaAprobado");

    /// <summary>Cultura/región para formatos (fechas, números).</summary>
    public static CultureInfo Locale => CultureInfo.GetCultureInfo("es-ES");

    /// <summary>
    ///     Directorio donde se almacenan los datos del repositorio.
    ///     NOTA PARA EL ALUMNO: Se lee de "Repository:Directory" en appsettings.json.
    ///     Valor por defecto: "data"
    /// </summary>
    public static string DataFolder => Path.Combine(Environment.CurrentDirectory, Config.GetValue<string>("Repository:Directory") ?? "data");

    /// <summary>
    ///     Tipo de almacenamiento para operaciones Import/Export.
    ///     NOTA PARA EL ALUMNO: Se usa en StorageFactory para crear el storage correcto.
    ///     Valores posibles: Json, Xml, Csv, CsvAlt, Text, Bin
    /// </summary>
    public static string StorageType => Config.GetValue<string>("Storage:Type") ?? "json";

    /// <summary>
    ///     Tipo de repositorio para persistencia de datos.
    ///     NOTA PARA EL ALUMNO: Se usa en RepositoryFactory para crear el repositorio correcto.
    ///     Valores posibles: Memory (volátil), Binary (ficheros binarios), Json (fichero JSON)
    ///     Se valida el valor y si es desconocido se usa "memory" por defecto.
    /// </summary>
    public static string RepositoryType {
        get {
            // Leemos el tipo del config
            var type = Config.GetValue<string>("Repository:Type") ?? "memory";
            
            // Validamos y convertimos a minúsculas
            // NOTA PARA EL ALUMNO: Usamos switch expression con valores válidos y default
            return type.ToLower() switch
            {
                "memory" => "memory",
                "binary" => "binary",
                "json" => "json",  // Nuevo: repositorio JSON
                _ => "memory"      // Si el valor no es válido, usamos memoria por defecto
            };
        }
    }

    /// <summary>
    ///     Ruta completa del archivo de datos según el tipo de storage.
    ///     NOTA PARA EL ALUMNO: Deduce la extensión según el StorageType configurado.
    ///     Ejemplo: Si StorageType = "json", devuelve "data/academia.json"
    /// </summary>
    public static string AcademiaFile {
        get {
            var extension = StorageType.ToLower() switch
            {
                "json" => "json",
                "xml" => "xml",
                "csv" or "csv-alt" => "csv",
                "txt" or "text" => "txt",
                "bin" => "bin",
                _ => "json" // valor por defecto
            };
            return Path.Combine(DataFolder, $"academia.{extension}");
        }
    }

    /// <summary>
    ///     Directorio donde se guardan los archivos de backup (ZIP).
    ///     NOTA PARA EL ALUMNO: Por defecto es "back" relativo al ejecutable.
    /// </summary>
    public static string BackupDirectory => Path.Combine(AppContext.BaseDirectory, Config.GetValue<string>("Backup:Directory") ?? "back");

    /// <summary>
    ///     Formato de archivo para los backups.
    ///     NOTA PARA EL ALUMNO: Permite elegir el tipo de storage para el backup.
    ///     Puede ser diferente del storage principal (ej: principal=bin, backup=json).
    /// </summary>
    public static string BackupFormat {
        get {
            var format = Config.GetValue<string>("Backup:Format") ?? "json";
            return format.ToLower() switch
            {
                "json" => "json",
                "xml" => "xml",
                "csv" or "csv-alt" => "csv",
                "txt" or "text" => "txt",
                "bin" => "bin",
                _ => "json"
            };
        }
    }
}
