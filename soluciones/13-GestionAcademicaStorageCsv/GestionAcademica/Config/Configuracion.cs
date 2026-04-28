using System.Globalization;

namespace GestionAcademica.Config;

/// <summary>
///     Clase estática que contiene las configuraciones globales para la gestión académica.
/// </summary>
public static class Configuracion {
    public static readonly double NotaAprobado = 5.00;
    public static readonly CultureInfo Locale = CultureInfo.GetCultureInfo("es-ES");
    public static readonly string DataFolder = Path.Combine(Environment.CurrentDirectory, "data");
    public static readonly string AcademiaFile = Path.Combine(DataFolder, "academia.csv");
}