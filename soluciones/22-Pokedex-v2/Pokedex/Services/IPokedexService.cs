using Pokedex.Models;

namespace Pokedex.Services;

public interface IPokedexService {
    IEnumerable<Pokemon> GetAll();
}