using GestionAcademica.Exceptions.Common;

namespace GestionAcademica.Exceptions.Backup;

/// <summary>
///     Contenedor de excepciones específicas para el dominio de Backup.
/// </summary>
public abstract class BackupException(string message) : DomainException(message) {
    /// <summary>Se lanza cuando el archivo de backup no existe.</summary>
    public sealed class FileNotFound(string filePath)
        : BackupException($"No se ha encontrado el archivo de backup: {filePath}");

    /// <summary>Se lanza cuando el archivo de backup está corrupto o es inválido.</summary>
    public sealed class InvalidBackupFile(string details)
        : BackupException($"El archivo de backup es inválido o está corrupto: {details}");

    /// <summary>Se lanza cuando hay errores al crear el archivo ZIP de backup.</summary>
    public sealed class CreationError(string details)
        : BackupException($"Error al crear el backup: {details}");

    /// <summary>Se lanza cuando hay errores al restaurar desde un backup.</summary>
    public sealed class RestorationError(string details)
        : BackupException($"Error al restaurar el backup: {details}");

    /// <summary>Se lanza cuando el directorio de backup no está disponible o no se puede crear.</summary>
    public sealed class DirectoryError(string details)
        : BackupException($"Error con el directorio de backup: {details}");
}
