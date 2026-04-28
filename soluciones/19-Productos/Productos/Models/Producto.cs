namespace Productos.Models;

/// <summary>
///     Representa un producto de la tienda.
/// </summary>
public sealed record Producto(
    int Id,
    string Nombre,
    int IdProveedor,
    int IdCategoria,
    string CantidadPorUnidad,
    double PrecioUnidad,
    int UnidadesEnStock,
    int UnidadesEnPedido,
    int NivelReabastecimiento,
    bool Descontinuado
);
