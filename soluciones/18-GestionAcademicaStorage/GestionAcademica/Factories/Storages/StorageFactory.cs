using GestionAcademica.Models;
using GestionAcademica.Storage.Binary;
using GestionAcademica.Storage.Common;
using GestionAcademica.Storage.Csv;
using GestionAcademica.Storage.CsvAlt;
using GestionAcademica.Storage.Json;
using GestionAcademica.Storage.Text;
using GestionAcademica.Storage.Xml;

namespace GestionAcademica.Factories;

/// <summary>
///     Factory para crear instancias de almacenamiento según el tipo solicitado.
///     Optimizado para crear solo la instancia necesaria (Lazy Loading).
/// </summary>
public static class StorageFactory {
    /// <summary>
    ///     Obtiene una instancia de almacenamiento según el tipo especificado.
    ///     NOTA PARA EL ALUMNO: Ya no creamos todos los storages al principio.
    ///     Usamos un switch para instanciar solo el que realmente vamos a usar.
    /// </summary>
    /// <param name="type">Tipo de almacenamiento deseado.</param>
    /// <returns>Instancia del almacenamiento correspondiente.</returns>
    public static IStorage<Persona> GetStorage(StorageType type) {
        return type switch {
            StorageType.Text => new AcademiaTextStorage(),
            StorageType.Csv => new AcademiaCsvStorage(),
            StorageType.CsvAlt => new AcademiaCsvAltStorage(),
            StorageType.Json => new AcademiaJsonStorage(),
            StorageType.Xml => new AcademiaXmlStorage(),
            StorageType.Binary => new AcademiaBinStorage(),
            _ => throw new ArgumentException($"Tipo de almacenamiento desconocido: {type}")
        };
    }

    /// <summary>
    ///     Obtiene el storage por defecto configurado en appsettings.json.
    /// </summary>
    public static IStorage<Persona> GetDefaultStorage(string configType) {
        var type = configType.ToLower() switch {
            "txt" or "text" => StorageType.Text,
            "csv" => StorageType.Csv,
            "json" => StorageType.Json,
            "xml" => StorageType.Xml,
            "csv-alt" => StorageType.CsvAlt,
            "binary" => StorageType.Binary,
            _ => throw new ArgumentException($"Tipo configurado desconocido: {configType}")
        };
        return GetStorage(type);
    }
}
