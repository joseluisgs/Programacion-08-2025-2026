using Pokedex.Models;
using Pokedex.Repositories;
using Pokedex.Storages;
using Serilog;

namespace Pokedex.Services;

public class PokedexService : IPokedexService {
    private readonly ILogger _logger = Log.ForContext<PokedexService>();
    private readonly IPokedexRepository _repository;
    private readonly IPokedexStorage _storage;

    public PokedexService(IPokedexRepository repository, IPokedexStorage storage) {
        _repository = repository;
        _storage = storage;

        _logger.Information("Cargando pokemons desde JSON");
        var pokemons = _storage.Cargar("data/pokedex.json");
        foreach (var pokemon in pokemons) _repository.Save(pokemon);
    }

    public IEnumerable<Pokemon> GetAll() {
        _logger.Debug("Obteniendo todos los pokemons");
        return _repository.GetAll();
    }
}