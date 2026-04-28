namespace AccidentesMadrid.Repositories;

public interface IRepository<T, in TKey> where T : class {
    IEnumerable<T> GetAll();
    T? GetByKey(TKey key);
    void Save(T item);
}