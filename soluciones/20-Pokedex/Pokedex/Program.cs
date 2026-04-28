using System.Text;
using Pokedex.Models;
using Pokedex.Repositories;
using Pokedex.Services;
using Pokedex.Storages;
using Serilog;
using static System.Console;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

Log.Logger = loggerConfiguration;

Title = "Pokedex - C# .NET";
OutputEncoding = Encoding.UTF8;

Main();

Log.CloseAndFlush();
return;

void Main()
{
    WriteLine("========================================");
    WriteLine("  POKEDEX - CONSULTA DE DATOS");
    WriteLine("========================================");
    WriteLine();

    IPokedexRepository repository = PokedexRepository.Instance;
    IPokedexStorage storage = new PokedexJsonStorage();
    var service = new PokedexService(repository, storage);

    var pokemons = service.GetAll().ToList();
    WriteLine($"Pokemons cargados: {pokemons.Count}");
    WriteLine();

    // CONSULTA 1: Todos los pokemons
    // SQL: SELECT * FROM pokemons
    WriteLine("========================================");
    WriteLine("  CONSULTA 1: Todos los pokemons");
    WriteLine("========================================");
    foreach (var p in pokemons)
        WriteLine($"  {p.Name} - {string.Join(", ", p.Type)}");
    WriteLine();

    // CONSULTA 2: Pokemon con id 10
    // SQL: SELECT * FROM pokemons WHERE id = 10
    WriteLine("========================================");
    WriteLine("  CONSULTA 2: Pokemon con id 10");
    WriteLine("========================================");
    var pokemonId10 = pokemons.FirstOrDefault(p => p.Id == 10);
    WriteLine($"  {pokemonId10?.Name}");
    WriteLine();

    // CONSULTA 3: Numero de pokemons
    // SQL: SELECT COUNT(*) FROM pokemons
    WriteLine("========================================");
    WriteLine("  CONSULTA 3: Numero de pokemons");
    WriteLine("========================================");
    WriteLine($"  Total: {pokemons.Count}");
    WriteLine();

    // CONSULTA 4: 10 primeros pokemons
    // SQL: SELECT * FROM pokemons LIMIT 10
    WriteLine("========================================");
    WriteLine("  CONSULTA 4: 10 primeros pokemons");
    WriteLine("========================================");
    var primeros10 = pokemons.Take(10);
    foreach (var p in primeros10)
        WriteLine($"  {p.Name}");
    WriteLine();

    // CONSULTA 5: Pokemon mas pesado
    // SQL: SELECT * FROM pokemons ORDER BY weight DESC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 5: Pokemon mas pesado");
    WriteLine("========================================");
    var masPesado = pokemons.MaxBy(p => p.Weight);
    WriteLine($"  {masPesado?.Name}: {masPesado?.Weight} kg");
    WriteLine();

    // CONSULTA 6: Pokemon mas ligero
    // SQL: SELECT * FROM pokemons ORDER BY weight ASC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 6: Pokemon mas ligero");
    WriteLine("========================================");
    var masLigero = pokemons.MinBy(p => p.Weight);
    WriteLine($"  {masLigero?.Name}: {masLigero?.Weight} kg");
    WriteLine();

    // CONSULTA 7: Pokemon con mas evoluciones
    // SQL: SELECT * FROM pokemons ORDER BY next_evolution COUNT DESC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 7: Pokemon con mas evoluciones");
    WriteLine("========================================");
    var masEvoluciones = pokemons.MaxBy(p => p.NextEvolution?.Count ?? 0);
    WriteLine($"  {masEvoluciones?.Name}: {masEvoluciones?.NextEvolution?.Count ?? 0} evoluciones");
    WriteLine();

    // CONSULTA 8: Pokemon con menos evoluciones
    // SQL: SELECT * FROM pokemons ORDER BY next_evolution COUNT ASC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 8: Pokemon con menos evoluciones");
    WriteLine("========================================");
    var menosEvoluciones = pokemons.MinBy(p => p.NextEvolution?.Count ?? 0);
    WriteLine($"  {menosEvoluciones?.Name}: {menosEvoluciones?.NextEvolution?.Count ?? 0} evoluciones");
    WriteLine();

    // CONSULTA 9: Pokemon con mas debilidades
    // SQL: SELECT * FROM pokemons ORDER BY weaknesses COUNT DESC LIMIT 1
    WriteLine("========================================");
    WriteLine("  CONSULTA 9: Pokemon con mas debilidades");
    WriteLine("========================================");
    var masDebilidades = pokemons.MaxBy(p => p.Weaknesses.Count);
    WriteLine($"  {masDebilidades?.Name}: {masDebilidades?.Weaknesses.Count} debilidades");
    WriteLine();

    // CONSULTA 10: Pokemons electricos
    // SQL: SELECT * FROM pokemons WHERE type LIKE '%Electric%'
    WriteLine("========================================");
    WriteLine("  CONSULTA 10: Pokemons electricos");
    WriteLine("========================================");
    var electricos = pokemons.Where(p => p.Type.Contains("Electric")).ToList();
    WriteLine($"  Total: {electricos.Count}");
    foreach (var p in electricos)
        WriteLine($"  - {p.Name}");
    WriteLine();

    // CONSULTA 11: Pikachu
    // SQL: SELECT * FROM pokemons WHERE name = 'Pikachu'
    WriteLine("========================================");
    WriteLine("  CONSULTA 11: Pikachu");
    WriteLine("========================================");
    var pikachu = pokemons.FirstOrDefault(p => p.Name == "Pikachu");
    WriteLine($"  {pikachu?.Name} - {string.Join(", ", pikachu?.Type ?? [])}");
    WriteLine();

    // CONSULTA 12: Numero de pokemons por tipo
    // SQL: SELECT type, COUNT(*) FROM pokemons GROUP BY type
    WriteLine("========================================");
    WriteLine("  CONSULTA 12: Pokemons por tipo");
    WriteLine("========================================");
    var pokemonsPorTipo = pokemons
        .SelectMany(p => p.Type, (p, t) => new { Pokemon = p, Tipo = t })
        .GroupBy(x => x.Tipo)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in pokemonsPorTipo)
        WriteLine($"  {item.Key}: {item.Value}");
    WriteLine();

    // CONSULTA 13: Debilidades con numero de pokemons
    // SQL: SELECT weakness, COUNT(*) FROM pokemons GROUP BY weakness
    WriteLine("========================================");
    WriteLine("  CONSULTA 13: Debilidades con conteo");
    WriteLine("========================================");
    var debilidadesConteo = pokemons
        .SelectMany(p => p.Weaknesses)
        .GroupBy(w => w)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in debilidadesConteo)
        WriteLine($"  {item.Key}: {item.Value}");
    WriteLine();

    // CONSULTA 14: Pokemons electricos debiles a Ground
    // SQL: SELECT * FROM pokemons WHERE type LIKE '%Electric%' AND weaknesses LIKE '%Ground%'
    WriteLine("========================================");
    WriteLine("  CONSULTA 14: Electricos debiles a Ground");
    WriteLine("========================================");
    var electricosGround = pokemons
        .Where(p => p.Type.Contains("Electric") && p.Weaknesses.Contains("Ground"))
        .Select(p => p.Name)
        .ToList();
    
    foreach (var nombre in electricosGround)
        WriteLine($"  {nombre}");
    WriteLine();

    // CONSULTA 15: Pokemons por debilidades (agrupados)
    // SQL: SELECT weakness, GROUP_CONCAT(name) FROM pokemons GROUP BY weakness
    WriteLine("========================================");
    WriteLine("  CONSULTA 15: Pokemons por debilidad");
    WriteLine("========================================");
    var pokemonsPorDebilidad = pokemons
        .SelectMany(p => p.Weaknesses.Select(w => new { Debilidad = w, Pokemon = p.Name }))
        .GroupBy(x => x.Debilidad)
        .ToDictionary(g => g.Key, g => g.Select(x => x.Pokemon).ToList());
    
    foreach (var item in pokemonsPorDebilidad.OrderBy(x => x.Key).Take(5))
    {
        WriteLine($"  {item.Key}: {item.Value.Count} pokemons");
    }
    WriteLine();

    // CONSULTA 16: Pokemons sin evoluciones
    // SQL: SELECT * FROM pokemons WHERE next_evolution IS NULL AND prev_evolution IS NULL
    WriteLine("========================================");
    WriteLine("  CONSULTA 16: Pokemons sin evoluciones");
    WriteLine("========================================");
    var sinEvoluciones = pokemons
        .Where(p => (p.NextEvolution == null || p.NextEvolution.Count == 0) && 
                    (p.PrevEvolution == null || p.PrevEvolution.Count == 0))
        .ToList();
    WriteLine($"  Total: {sinEvoluciones.Count}");
    foreach (var p in sinEvoluciones.Take(10))
        WriteLine($"  - {p.Name}");
    WriteLine();

    // CONSULTA 17: Pokemons con evolucion previa
    // SQL: SELECT * FROM pokemons WHERE prev_evolution IS NOT NULL
    WriteLine("========================================");
    WriteLine("  CONSULTA 17: Pokemons con evolucion previa");
    WriteLine("========================================");
    var conEvolucionPrevia = pokemons
        .Where(p => p.PrevEvolution != null && p.PrevEvolution.Count > 0)
        .ToList();
    WriteLine($"  Total: {conEvolucionPrevia.Count}");
    foreach (var p in conEvolucionPrevia)
    {
        var origen = p.PrevEvolution?[0].Name ?? "N/A";
        WriteLine($"  - {p.Name} (de {origen})");
    }
    WriteLine();

    // CONSULTA 18: Pokemons por tipo secundario
    // SQL: SELECT tipo2, COUNT(*) FROM pokemons GROUP BY tipo2
    WriteLine("========================================");
    WriteLine("  CONSULTA 18: Pokemons por tipo secundario");
    WriteLine("========================================");
    var pokemonsPorTipoSecundario = pokemons
        .Where(p => p.Type.Count > 1)
        .GroupBy(p => p.Type[1])
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in pokemonsPorTipoSecundario)
        WriteLine($"  {item.Key}: {item.Value}");
    WriteLine();

    // CONSULTA 19: Pokemons leyenda
    // SQL: SELECT * FROM pokemons WHERE avg_spawns < 0.01
    WriteLine("========================================");
    WriteLine("  CONSULTA 19: Pokemons leyenda (muy raros)");
    WriteLine("========================================");
    var pokemonsLeyenda = pokemons
        .Where(p => p.AvgSpawns < 0.01)
        .ToList();
    WriteLine($"  Total: {pokemonsLeyenda.Count}");
    foreach (var p in pokemonsLeyenda)
        WriteLine($"  - {p.Name} (avg: {p.AvgSpawns})");
    WriteLine();

    // CONSULTA 20: Top 5 pokemons mas pesados
    // SQL: SELECT * FROM pokemons ORDER BY weight DESC LIMIT 5
    WriteLine("========================================");
    WriteLine("  CONSULTA 20: Top 5 mas pesados");
    WriteLine("========================================");
    var top5Pesados = pokemons
        .OrderByDescending(p => p.Weight)
        .Take(5);
    foreach (var p in top5Pesados)
        WriteLine($"  - {p.Name}: {p.Weight} kg");
    WriteLine();

    // CONSULTA 21: Top 5 pokemons mas altos
    // SQL: SELECT * FROM pokemons ORDER BY height DESC LIMIT 5
    WriteLine("========================================");
    WriteLine("  CONSULTA 21: Top 5 mas altos");
    WriteLine("========================================");
    var top5Altos = pokemons
        .OrderByDescending(p => p.Height)
        .Take(5);
    foreach (var p in top5Altos)
        WriteLine($"  - {p.Name}: {p.Height} m");
    WriteLine();

    // CONSULTA 22: Pokemons tipo dual
    // SQL: SELECT * FROM pokemons WHERE type COUNT = 2
    WriteLine("========================================");
    WriteLine("  CONSULTA 22: Pokemons tipo dual");
    WriteLine("========================================");
    var tipoDual = pokemons
        .Where(p => p.Type.Count == 2)
        .ToList();
    WriteLine($"  Total: {tipoDual.Count}");
    foreach (var p in tipoDual)
        WriteLine($"  - {p.Name}: {string.Join(", ", p.Type)}");
    WriteLine();

    // CONSULTA 23: Pokemons por egg
    // SQL: SELECT egg, COUNT(*) FROM pokemons GROUP BY egg
    WriteLine("========================================");
    WriteLine("  CONSULTA 23: Pokemons por tipo de huevo");
    WriteLine("========================================");
    var pokemonsPorEgg = pokemons
        .GroupBy(p => p.Egg)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in pokemonsPorEgg)
        WriteLine($"  {item.Key}: {item.Value} pokemons");
    WriteLine();

    // CONSULTA 24: Cadenas de evolucion completas
    // SQL: SELECT * FROM pokemons WHERE prev_evolution IS NOT NULL AND next_evolution IS NOT NULL
    WriteLine("========================================");
    WriteLine("  CONSULTA 24: Cadenas de evolucion");
    WriteLine("========================================");
    var cadenasEvolucion = pokemons
        .Where(p => p.PrevEvolution != null && p.NextEvolution != null)
        .ToList();
    WriteLine($"  Total en cadenas: {cadenasEvolucion.Count}");
    foreach (var p in cadenasEvolucion)
    {
        var origen = p.PrevEvolution?[0].Name ?? "";
        var destino = p.NextEvolution?[0].Name ?? "";
        WriteLine($"  - {origen} -> {p.Name} -> {destino}");
    }
    WriteLine();

    // CONSULTA 25: Pokemons sin debilidades
    // SQL: SELECT * FROM pokemons WHERE weaknesses IS EMPTY
    WriteLine("========================================");
    WriteLine("  CONSULTA 25: Pokemons sin debilidades");
    WriteLine("========================================");
    var sinDebilidades = pokemons
        .Where(p => p.Weaknesses.Count == 0)
        .ToList();
    WriteLine($"  Total: {sinDebilidades.Count}");
    foreach (var p in sinDebilidades)
        WriteLine($"  - {p.Name}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  PROGRAMA FINALIZADO");
    WriteLine("========================================");
}
