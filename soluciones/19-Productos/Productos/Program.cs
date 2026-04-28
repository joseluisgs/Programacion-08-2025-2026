using System.Globalization;
using System.Text;
using Productos.Models;
using Productos.Repositories;
using Productos.Services;
using Productos.Storages;
using Serilog;
using static System.Console;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

Log.Logger = loggerConfiguration;

Title = "Sistema de Productos - C# .NET";
OutputEncoding = Encoding.UTF8;

Main();

Log.CloseAndFlush();
WriteLine("\nPresiona una tecla para salir...");
return;

void Main()
{
    WriteLine("========================================");
    WriteLine("  PRACTICA: PROCESADOR DE PRODUCTOS");
    WriteLine("========================================");
    WriteLine();

    IProductoStorage storage = new ProductoCsvStorage();
    IProductoRepository repository = ProductoRepository.Instance;
    IProductoService service = new ProductoService(storage, repository);

    const string rutaCsv = "data/products.csv";
    
    WriteLine($"Importando productos desde: {rutaCsv}");
    var cantidad = service.Importar(rutaCsv);
    WriteLine($"Importados: {cantidad} productos");
    WriteLine();

    var productos = service.GetAll().ToList();

    // CONSULTA 1: Todos los productos
    // SQL: SELECT * FROM products
    WriteLine("========================================");
    WriteLine("  CONSULTA 1: Todos los productos");
    WriteLine("========================================");
    foreach (var p in productos)
        WriteLine($"  {p}");
    WriteLine();

    // CONSULTA 2: Nombre de los productos
    // SQL: SELECT productName FROM products
    WriteLine("========================================");
    WriteLine("  CONSULTA 2: Nombre de los productos");
    WriteLine("========================================");
    foreach (var p in productos)
        WriteLine($"  {p.Nombre}");
    WriteLine();

    // CONSULTA 3: Productos con stock < 10
    // SQL: SELECT productName FROM products WHERE unitsInStock < 10
    WriteLine("========================================");
    WriteLine("  CONSULTA 3: Stock < 10");
    WriteLine("========================================");
    var stockMenor10 = productos
        .Where(p => p.UnidadesEnStock < 10)
        .Select(p => p.Nombre);
    
    foreach (var nombre in stockMenor10)
        WriteLine($"  {nombre}");
    WriteLine();

    // CONSULTA 4: Stock < 5 ordenado por stock ASC
    // SQL: SELECT productName, unitsInStock FROM products WHERE unitsInStock < 5 ORDER BY unitsInStock ASC
    WriteLine("========================================");
    WriteLine("  CONSULTA 4: Stock < 5 ordenado ASC");
    WriteLine("========================================");
    var stockMenor5Ordenado = productos
        .Where(p => p.UnidadesEnStock < 5)
        .OrderBy(p => p.UnidadesEnStock);
    
    foreach (var item in stockMenor5Ordenado)
        WriteLine($"  {item.Nombre} (stock: {item.UnidadesEnStock})");
    WriteLine();

    // CONSULTA 5: Número de proveedores únicos
    // SQL: SELECT COUNT(DISTINCT supplierID) FROM products
    WriteLine("========================================");
    WriteLine("  CONSULTA 5: Proveedores unicos");
    WriteLine("========================================");
    var numeroProveedores = productos.Select(p => p.IdProveedor).Distinct().Count();
    WriteLine($"  Proveedores unicos: {numeroProveedores}");
    WriteLine();

    // CONSULTA 6: Existencias por producto
    // SQL: SELECT productName, unitsInStock FROM products
    WriteLine("========================================");
    WriteLine("  CONSULTA 6: Existencias por producto");
    WriteLine("========================================");
    foreach (var p in productos)
        WriteLine($"  {p.Nombre}: {p.UnidadesEnStock} unidades");
    WriteLine();

    // CONSULTA 7: Productos por proveedor
    // SQL: SELECT supplierID, COUNT(*) FROM products GROUP BY supplierID
    WriteLine("========================================");
    WriteLine("  CONSULTA 7: Productos por proveedor");
    WriteLine("========================================");
    var productosPorProveedor = productos
        .GroupBy(p => p.IdProveedor)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in productosPorProveedor)
        WriteLine($"  Proveedor {item.Key}: {item.Value} productos");
    WriteLine();

    // CONSULTA 8: Media de precio por proveedor
    // SQL: SELECT supplierID, AVG(unitPrice) FROM products GROUP BY supplierID
    WriteLine("========================================");
    WriteLine("  CONSULTA 8: Media precio por proveedor");
    WriteLine("========================================");
    var mediaPorProveedor = productos
        .GroupBy(p => p.IdProveedor)
        .ToDictionary(g => g.Key, g => g.Average(p => p.PrecioUnidad))
        .OrderByDescending(x => x.Value);
    
    foreach (var item in mediaPorProveedor)
        WriteLine($"  Proveedor {item.Key}: {item.Value:F2} EUR");
    WriteLine();

    // CONSULTA 9: Producto más caro
    // SQL: SELECT * FROM products ORDER BY unitPrice DESC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 9: Producto mas caro");
    WriteLine("========================================");
    var masCaro = productos.MaxBy(p => p.PrecioUnidad);
    WriteLine($"  {masCaro?.Nombre}: {masCaro?.PrecioUnidad:F2} EUR");
    WriteLine();

    // CONSULTA 10: Proveedores con >5 productos
    // SQL: SELECT supplierID FROM products GROUP BY supplierID HAVING COUNT(*) > 5
    WriteLine("========================================");
    WriteLine("  CONSULTA 10: Proveedores >5 productos");
    WriteLine("========================================");
    var proveedoresMas5 = productos
        .GroupBy(p => p.IdProveedor)
        .Where(g => g.Count() > 5)
        .ToDictionary(g => g.Key, g => g.Count());
    
    foreach (var item in proveedoresMas5)
        WriteLine($"  Proveedor {item.Key}: {item.Value} productos");
    WriteLine();

    // CONSULTA 11: Proveedores suma precios >100
    // SQL: SELECT supplierID FROM products GROUP BY supplierID HAVING SUM(unitPrice) > 100
    WriteLine("========================================");
    WriteLine("  CONSULTA 11: Suma precios >100");
    WriteLine("========================================");
    var proveedoresSumaMayor100 = productos
        .GroupBy(p => p.IdProveedor)
        .Where(g => g.Sum(p => p.PrecioUnidad) > 100)
        .ToDictionary(g => g.Key, g => g.Sum(p => p.PrecioUnidad));
    
    foreach (var item in proveedoresSumaMayor100)
        WriteLine($"  Proveedor {item.Key}: {item.Value:F2} EUR");
    WriteLine();

    // CONSULTA 12: Productos por categoría
    // SQL: SELECT categoryID, COUNT(*) FROM products GROUP BY categoryID
    WriteLine("========================================");
    WriteLine("  CONSULTA 12: Productos por categoria");
    WriteLine("========================================");
    var productosPorCategoria = productos
        .GroupBy(p => p.IdCategoria)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in productosPorCategoria)
        WriteLine($"  Categoria {item.Key}: {item.Value} productos");
    WriteLine();

    // CONSULTA 13: Categoría más cara
    // SQL: SELECT categoryID FROM products GROUP BY categoryID ORDER BY AVG(unitPrice) DESC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 13: Categoria mas cara");
    WriteLine("========================================");
    var categoriaMasCara = productos
        .GroupBy(p => p.IdCategoria)
        .ToDictionary(g => g.Key, g => g.Average(p => p.PrecioUnidad))
        .MaxBy(x => x.Value);
    
    WriteLine($"  Categoria {categoriaMasCara.Key}: media {categoriaMasCara.Value:F2} EUR");
    WriteLine();

    // CONSULTA 14: Stats por categoría
    // SQL: SELECT categoryID, MAX(unitPrice), MIN(unitPrice), AVG(unitPrice), COUNT(*) FROM products GROUP BY categoryID
    WriteLine("========================================");
    WriteLine("  CONSULTA 14: Stats por categoria");
    WriteLine("========================================");
    var statsPorCategoria = productos
        .GroupBy(p => p.IdCategoria)
        .ToDictionary(
            g => g.Key,
            g => new
            {
                Maximo = g.Max(p => p.PrecioUnidad),
                Minimo = g.Min(p => p.PrecioUnidad),
                Media = g.Average(p => p.PrecioUnidad),
                Cantidad = g.Count()
            }
        )
        .OrderBy(x => x.Key);
    
    foreach (var item in statsPorCategoria)
    {
        WriteLine($"  Categoria {item.Key}:");
        WriteLine($"    - Maximo: {item.Value.Maximo:F2} EUR");
        WriteLine($"    - Minimo: {item.Value.Minimo:F2} EUR");
        WriteLine($"    - Media:  {item.Value.Media:F2} EUR");
        WriteLine($"    - Cant:   {item.Value.Cantidad} productos");
    }
    WriteLine();

    // EXTRA: Proveedor con más productos
    WriteLine("========================================");
    WriteLine("  EXTRA: Proveedor con mas productos");
    WriteLine("========================================");
    var proveedorTop = productos
        .GroupBy(p => p.IdProveedor)
        .MaxBy(g => g.Count());
    
    WriteLine($"  Proveedor {proveedorTop?.Key}: {proveedorTop?.Count()} productos");
    WriteLine();

    // Exportar a CSV
    const string rutaExport = "data/products_back.csv";
    
    WriteLine("========================================");
    WriteLine("  EXPORTANDO A CSV");
    WriteLine("========================================");
    var exportados = service.Exportar(rutaExport);
    WriteLine($"  Exportados: {exportados} productos");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  PROGRAMA FINALIZADO");
    WriteLine("========================================");
}
