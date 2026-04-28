using Pokedex.Models;
using Serilog;

namespace Pokedex.Repositories;

public sealed class PokedexRepository : IPokedexRepository
{
    private static readonly Lazy<PokedexRepository> Lazy = new(() => new PokedexRepository());
    public static PokedexRepository Instance => Lazy.Value;

    private readonly Dictionary<int, Pokemon> _pokemons = new();
    private readonly ILogger _logger = Log.ForContext<PokedexRepository>();

    private PokedexRepository()
    {
        _logger.Information("Repositorio inicializado");
    }

    public IEnumerable<Pokemon> GetAll()
    {
        _logger.Debug("Obteniendo todos los pokemons");
        return _pokemons.Values;
    }

    public Pokemon? GetById(int id)
    {
        _logger.Debug("Buscando pokemon con ID: {Id}", id);
        return _pokemons.TryGetValue(id, out var pokemon) ? pokemon : null;
    }

    public Pokemon Save(Pokemon pokemon)
    {
        _logger.Debug("Guardando pokemon: {Name}", pokemon.Name);
        _pokemons[pokemon.Id] = pokemon;
        _logger.Information("Pokemon guardado con ID: {Id}", pokemon.Id);
        return pokemon;
    }
}
