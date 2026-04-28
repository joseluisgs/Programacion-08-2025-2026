using Pokedex.Models;
using Serilog;

namespace Pokedex.Repositories;

public sealed class PokedexRepository : IPokedexRepository {
    private static readonly Lazy<PokedexRepository> Lazy = new(() => new PokedexRepository());
    private readonly ILogger _logger = Log.ForContext<PokedexRepository>();

    private readonly Dictionary<int, Pokemon> _pokemons = new();

    private PokedexRepository() {
        _logger.Information("Repositorio inicializado");
    }

    public static PokedexRepository Instance => Lazy.Value;

    public IEnumerable<Pokemon> GetAll() {
        _logger.Debug("Obteniendo todos los pokemons");
        return _pokemons.Values;
    }

    public Pokemon? GetById(int id) {
        _logger.Debug("Buscando pokemon con ID: {Id}", id);
        return _pokemons.TryGetValue(id, out var pokemon) ? pokemon : null;
    }

    public Pokemon Save(Pokemon pokemon) {
        _pokemons[pokemon.Id] = pokemon;
        return pokemon;
    }
}
