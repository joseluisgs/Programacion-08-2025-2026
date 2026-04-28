namespace Pokedex.Storages;

public interface IStorage<T>
{
    IEnumerable<T> Cargar(string path);
}
