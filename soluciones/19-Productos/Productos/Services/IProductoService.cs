using Productos.Models;

namespace Productos.Services;

public interface IProductoService
{
    int Importar(string path);
    int Exportar(string path);
    IEnumerable<Producto> GetAll();
    Producto? GetById(int id);
    Producto Save(Producto producto);
}
