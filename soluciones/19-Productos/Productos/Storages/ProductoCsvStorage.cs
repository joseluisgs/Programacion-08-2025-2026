using System.Globalization;
using System.Text;
using Productos.Models;
using Serilog;

namespace Productos.Storages;

public class ProductoCsvStorage : IProductoStorage
{
    private readonly ILogger _logger = Log.ForContext<ProductoCsvStorage>();

    private const string HeaderCsv = "productID,productName,supplierID,categoryID,quantityPerUnit,unitPrice,unitsInStock,unitsOnOrder,reorderLevel,discontinued";

    public IEnumerable<Producto> Leer(string path)
    {
        _logger.Information("Leyendo productos desde: {Path}", path);

        if (!File.Exists(path))
        {
            _logger.Error("Archivo no encontrado: {Path}", path);
            throw new FileNotFoundException($"Archivo no encontrado: {path}");
        }

        try
        {
            // Usamos File.ReadLines para procesamiento perezoso (Lazy) línea a línea
            return File.ReadLines(path, Encoding.UTF8)
                .Skip(1) // Saltamos cabecera
                .Select(ParseLineToProducto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al leer CSV: {Message}", ex.Message);
            throw;
        }
    }

    public void Escribir(string path, IEnumerable<Producto> productos)
    {
        _logger.Information("Escribiendo productos en: {Path}", path);

        try
        {
            var directorio = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            // Creamos un IEnumerable que incluye la cabecera y luego los productos transformados
            var lineas = Enumerable.Repeat(HeaderCsv, 1)
                .Concat(productos.Select(ProductoToLinea));

            File.WriteAllLines(path, lineas, Encoding.UTF8);

            _logger.Information("Proceso de escritura finalizado");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al escribir CSV: {Message}", ex.Message);
            throw;
        }
    }

    private static Producto ParseLineToProducto(string linea)
    {
        var partes = linea.Split(',');

        return new Producto(
            int.Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            int.Parse(partes[3]),
            partes[4],
            double.Parse(partes[5], CultureInfo.InvariantCulture),
            int.Parse(partes[6]),
            int.Parse(partes[7]),
            int.Parse(partes[8]),
            partes[9] == "1" || partes[9].ToLower() == "true"
        );
    }

    private static string ProductoToLinea(Producto p) =>
        $"{p.Id},{p.Nombre},{p.IdProveedor},{p.IdCategoria},{p.CantidadPorUnidad},{p.PrecioUnidad.ToString(CultureInfo.InvariantCulture)},{p.UnidadesEnStock},{p.UnidadesEnPedido},{p.NivelReabastecimiento},{(p.Descontinuado ? 1 : 0)}";
}
