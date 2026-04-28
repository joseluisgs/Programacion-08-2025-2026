namespace Accidentes.Storages;

public interface IStorage<T> {
    IEnumerable<T> Cargar(string path);
}