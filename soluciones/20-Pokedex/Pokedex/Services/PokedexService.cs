using Pokedex.Models;
using Pokedex.Repositories;
using Pokedex.Storages;
using Serilog;

namespace Pokedex.Services;

public class PokedexService : IPokedexService
{
    private readonly IPokedexRepository _repository;
    private readonly ILogger _logger = Log.ForContext<PokedexService>();

    public PokedexService(IPokedexRepository repository, IPokedexStorage storage)
    {
        _repository = repository;

        _logger.Information("Cargando pokemons desde JSON");
        var pokemons = storage.Cargar("data/pokemons.json");
        foreach (var pokemon in pokemons)
        {
            _repository.Save(pokemon);
        }
    }

    public IEnumerable<Pokemon> GetAll()
    {
        _logger.Debug("Obteniendo todos los pokemons");
        return _repository.GetAll();
    }

    public Pokemon? GetById(int id)
    {
        _logger.Debug("Buscando pokemon con ID: {Id}", id);
        return _repository.GetById(id);
    }
}
