namespace GestionAcademica.Factories.Repositories;

/// <summary>
///     Enumerado que define los tipos de repositorio disponibles en el sistema.
///     NOTA PARA EL ALUMNO: Este enum permite añadir nuevos tipos de repositorio
///     sin modificar la lógica existente. Es un ejemplo del patrón "Strategy".
/// </summary>
public enum RepositoryType {
    /// <summary>Repositorio en memoria (Dictionary). Datos se pierden al cerrar.</summary>
    Memory,

    /// <summary>
    ///     Repositorio binario con índices. Persiste en archivos .dat, .idx, .frag.
    ///     Implementa un motor de base de datos simplificado con acceso aleatorio.
    /// </summary>
    Binary,

    /// <summary>Repositorio JSON. Persiste en archivo academia.json en cada operación.</summary>
    Json
}
