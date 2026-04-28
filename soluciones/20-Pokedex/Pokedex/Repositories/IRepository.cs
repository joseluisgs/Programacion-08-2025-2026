namespace Pokedex.Repositories;

public interface IRepository<T, in TKey> where T : class
{
    IEnumerable<T> GetAll();
    T? GetById(TKey key);
    T Save(T item);
}
