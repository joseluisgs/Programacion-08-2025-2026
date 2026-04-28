using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pokedex.Dto;
using Pokedex.Mappers;
using Pokedex.Models;
using Serilog;

namespace Pokedex.Storages;

public class PokedexJsonStorage : IPokedexStorage {
    private readonly ILogger _logger = Log.ForContext<PokedexJsonStorage>();

    private readonly JsonSerializerOptions _options = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public IEnumerable<Pokemon> Cargar(string path) {
        _logger.Information("Cargando pokemons desde: {Path}", path);

        if (!File.Exists(path)) {
            _logger.Error("Archivo no encontrado: {Path}", path);
            throw new FileNotFoundException($"Archivo no encontrado: {path}");
        }

        try {
            using var stream = File.OpenRead(path);
            var pokemonsDto = JsonSerializer.Deserialize<List<PokemonDto>>(stream, _options);

            return pokemonsDto?.Select(p => p.ToModel())
                   ?? throw new InvalidOperationException("No se pudieron deserializar los pokemons");
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al cargar JSON: {Message}", ex.Message);
            throw;
        }
    }

    public void Guardar(string path, IEnumerable<Pokemon> items) {
        _logger.Information("Guardando pokemons en: {Path}", path);
        // No implementado para este proyecto
    }
}