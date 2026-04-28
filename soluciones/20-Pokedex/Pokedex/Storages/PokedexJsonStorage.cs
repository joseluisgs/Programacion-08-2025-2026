using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pokedex.Dto;
using Pokedex.Mappers;
using Pokedex.Models;
using Serilog;

namespace Pokedex.Storages;

public class PokedexJsonStorage : IPokedexStorage
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly ILogger _logger = Log.ForContext<PokedexJsonStorage>();

    public IEnumerable<Pokemon> Cargar(string path)
    {
        _logger.Information("Cargando pokemons desde: {Path}", path);

        if (!File.Exists(path))
        {
            _logger.Error("Archivo no encontrado: {Path}", path);
            throw new FileNotFoundException($"Archivo no encontrado: {path}");
        }

        try
        {
            // NOTA PARA EL ALUMNO: 
            // Usamos File.OpenRead() que devuelve un Stream (flujo de bytes).
            // Es más eficiente que ReadAllText() porque el JsonSerializer lee directamente del "grifo" del fichero,
            // evitando crear un string gigante en la memoria RAM con todo el contenido del JSON.
            using var stream = File.OpenRead(path);
            var pokedexDto = JsonSerializer.Deserialize<PokedexDto>(stream, _options);
            
            // Devolvemos el IEnumerable para mantener la evaluación perezosa (Lazy)
            return pokedexDto?.Pokemons.Select(p => p.ToModel())
                ?? throw new InvalidOperationException("No se pudieron deserializar los pokemons");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al cargar JSON: {Message}", ex.Message);
            throw;
        }
    }
}
