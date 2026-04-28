namespace Productos.Repositories;

/// <summary>
///     Contrato genérico para repositorio.
///     Solo define los métodos necesarios: GetAll, GetById, Save.
/// </summary>
public interface IRepository<T, in TKey> where T : class
{
    IEnumerable<T> GetAll();
    T? GetById(TKey key);
    T Save(T item);
}
