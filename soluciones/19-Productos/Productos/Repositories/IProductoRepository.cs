using Productos.Models;

namespace Productos.Repositories;

/// <summary>
///     Contrato específico para el repositorio de productos.
/// </summary>
public interface IProductoRepository : IRepository<Producto, int>
{
}
