using GestionAcademica.Models.Personas;

namespace GestionAcademica.Services;

/// <summary>
///     Contrato para el servicio de backup y restauración del sistema.
/// </summary>
public interface IBackupService {
    /// <summary>
    ///     Realiza una copia de seguridad de los datos proporcionados.
    /// </summary>
    /// <param name="personas">Colección de personas a respaldar.</param>
    /// <returns>La ruta del archivo ZIP creado.</returns>
    string RealizarBackup(IEnumerable<Persona> personas);

    /// <summary>
    ///     Restaura los datos desde un archivo de backup.
    /// </summary>
    /// <param name="archivoBackup">Ruta del archivo ZIP de backup.</param>
    /// <returns>Colección de personas restauradas.</returns>
    IEnumerable<Persona> RestaurarBackup(string archivoBackup);

    /// <summary>
    ///     Obtiene la lista de archivos de backup disponibles.
    /// </summary>
    /// <returns>Enumerable con las rutas de los archivos de backup.</returns>
    IEnumerable<string> ListarBackups();
}
