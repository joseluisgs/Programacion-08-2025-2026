using Productos.Models;
using Productos.Repositories;
using Productos.Storages;
using Serilog;

namespace Productos.Services;

public class ProductoService(
    IProductoStorage storage,
    IProductoRepository repository
) : IProductoService
{
    private readonly ILogger _logger = Log.ForContext<ProductoService>();

    public int Importar(string path)
    {
        _logger.Information("Importando productos desde: {Path}", path);
        
        try
        {
            var productos = storage.Leer(path);
            var contador = 0;
            
            foreach (var producto in productos)
            {
                repository.Save(producto);
                contador++;
            }
            
            _logger.Information("Importados {Cantidad} productos", contador);
            return contador;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al importar: {Message}", ex.Message);
            throw;
        }
    }

    public int Exportar(string path)
    {
        _logger.Information("Exportando productos a: {Path}", path);
        
        try
        {
            var productos = repository.GetAll();
            storage.Escribir(path, productos);
            
            var cantidad = productos.Count();
            _logger.Information("Exportación finalizada. Procesados {Cantidad} productos", cantidad);
            return cantidad;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al exportar: {Message}", ex.Message);
            throw;
        }
    }

    public IEnumerable<Producto> GetAll()
    {
        _logger.Debug("Obteniendo todos los productos");
        return repository.GetAll();
    }

    public Producto? GetById(int id)
    {
        _logger.Debug("Buscando producto con ID: {Id}", id);
        return repository.GetById(id);
    }

    public Producto Save(Producto producto)
    {
        _logger.Debug("Guardando producto: {Nombre}", producto.Nombre);
        return repository.Save(producto);
    }
}
