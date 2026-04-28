using Productos.Models;

namespace Productos.Storages;

/// <summary>
///     Contrato para el almacenamiento de productos en CSV.
///     Define las operaciones de lectura y escritura.
/// </summary>
public interface IProductoStorage
{
    /// <summary>
    ///     Lee productos desde un archivo CSV.
    /// </summary>
    /// <param name="path">Ruta del archivo CSV</param>
    /// <returns>Lista de productos leídos</returns>
    IEnumerable<Producto> Leer(string path);

    /// <summary>
    ///     Escribe productos en un archivo CSV.
    /// </summary>
    /// <param name="path">Ruta del archivo CSV</param>
    /// <param name="productos">Lista de productos a escribir</param>
    void Escribir(string path, IEnumerable<Producto> productos);
}
