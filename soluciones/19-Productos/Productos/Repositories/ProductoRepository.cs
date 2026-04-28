using Productos.Models;
using Serilog;

namespace Productos.Repositories;

/// <summary>
///     Implementación del repositorio de productos.
///     Usa Dictionary para búsquedas O(1).
///     Solo implementa los métodos necesarios: GetAll y Save.
/// </summary>
public sealed class ProductoRepository : IProductoRepository
{
    /// <summary>
    ///     Singleton: Lazy<T> garantiza una única instancia thread-safe.
    /// </summary>
    private static readonly Lazy<ProductoRepository> Lazy = new(() => new ProductoRepository());
    
    public static ProductoRepository Instance => Lazy.Value;

    /// <summary>
    ///     Diccionario: ID -> Producto. Búsqueda O(1).
    /// </summary>
    private readonly Dictionary<int, Producto> _productos = new();

    private int _idContador = 0;

    private readonly ILogger _logger = Log.ForContext<ProductoRepository>();

    private ProductoRepository()
    {
        _logger.Information("Repositorio inicializado");
    }

    /// <inheritdoc />
    public IEnumerable<Producto> GetAll()
    {
        _logger.Debug("Obteniendo todos los productos");
        return _productos.Values;
    }

    /// <inheritdoc />
    public Producto? GetById(int id)
    {
        _logger.Debug("Buscando producto con ID: {Id}", id);
        return _productos.TryGetValue(id, out var producto) ? producto : null;
    }

    /// <inheritdoc />
    public Producto Save(Producto producto)
    {
        _logger.Debug("Guardando producto: {Nombre}", producto.Nombre);
        
        var nuevoId = producto.Id == 0 ? ++_idContador : producto.Id;
        
        var productoConId = nuevoId != producto.Id 
            ? producto with { Id = nuevoId } 
            : producto;

        _productos[nuevoId] = productoConId;
        
        _logger.Information("Producto guardado con ID: {Id}", nuevoId);
        
        return productoConId;
    }
}
