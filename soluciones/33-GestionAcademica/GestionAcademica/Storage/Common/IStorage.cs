namespace GestionAcademica.Storage.Common;

/// <summary>
///     Interface para el almacenamiento de datos en un archivo.
/// </summary>
/// <typeparam name="T">Tipo de Dato</typeparam>
public interface IStorage<T> {
    /// <summary>
    ///     Salva una colección de elementos en un archivo.
    /// </summary>
    /// <param name="items">Colección de elementos a guardar.</param>
    /// <param name="path">Ruta del archivo donde se guardarán los datos.</param>
    public void Salvar(IEnumerable<T> items, string path);

    /// <summary>
    ///     Carga una colección de elementos desde un archivo.
    /// </summary>
    /// <param name="path">Ruta del archivo desde donde se cargarán los datos.</param>
    /// <returns>Colección de elementos cargados desde el archivo.</returns>
    public IEnumerable<T> Cargar(string path);
}