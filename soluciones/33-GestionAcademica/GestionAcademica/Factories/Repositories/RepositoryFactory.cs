using GestionAcademica.Factories.Repositories;
using GestionAcademica.Repositories;
using GestionAcademica.Repositories.Json;
using GestionAcademica.Repositories.Personas.Base;
using GestionAcademica.Repositories.Personas.Binary;
using GestionAcademica.Repositories.Personas.Memory;

namespace GestionAcademica.Factories.Repositories;

/// <summary>
///     FACTORY PATTERN: Esta clase es una fábrica de repositorios.
///     NOTA PARA EL ALUMNO: Una fábrica (Factory) es un patrón de diseño creacional
///     que encapsula la lógica de creación de objetos. El cliente (Program.cs)
///     no sabe qué implementación se está usando, solo sabe que implements IPersonasRepository.
///     Esto permite cambiar el comportamiento sin modificar el código cliente (SOLID - DIP).
/// </summary>
public static class RepositoryFactory {
    /// <summary>
    ///     Crea un repositorio según el tipo especificado.
    ///     NOTA PARA EL ALUMNO: Usamos "switch expression" (C# 8+) que es más concisa que switch/case tradicional.
    /// </summary>
    /// <param name="type">Tipo de repositorio a crear.</param>
    /// <returns>Instancia del repositorio solicitado.</returns>
    /// <exception cref="ArgumentException">Si el tipo no es válido.</exception>
    public static IPersonasRepository GetRepository(RepositoryType type) {
        return type switch {
            RepositoryType.Memory => PersonasMemoryRepository.Instance,
            RepositoryType.Binary => PersonasBinaryRepository.Instance,
            RepositoryType.Json => PersonasJsonRepository.Instance,
            _ => throw new ArgumentException($"Tipo de repositorio desconocido: {type}")
        };
    }

    /// <summary>
    ///     Crea un repositorio a partir de la configuración en appsettings.json.
    ///     NOTA PARA EL ALUMNO: Este método hace de "puente" entre el string del config
    ///     y el enum RepositoryType. Permite flexibilidad sin recompilar.
    /// </summary>
    /// <param name="configType">Tipo de repositorio leído del archivo de configuración.</param>
    /// <returns>Instancia del repositorio configurado.</returns>
    /// <exception cref="ArgumentException">Si el tipo configurado no es válido.</exception>
    public static IPersonasRepository GetDefaultRepository(string configType) {
        // CONVERSIÓN: Transformamos el string del config a enum.
        // Esto permite que el usuario escriba "json" en lugar de RepositoryType.Json.
        var type = configType.ToLower() switch {
            "memory" => RepositoryType.Memory,
            "binary" => RepositoryType.Binary,
            "json" => RepositoryType.Json,
            _ => throw new ArgumentException($"Tipo configurado desconocido: {configType}")
        };
        return GetRepository(type);
    }
}
