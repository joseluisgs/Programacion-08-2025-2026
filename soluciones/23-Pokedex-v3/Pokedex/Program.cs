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

// =============================================================================
// NOTAS PARA EL ALUMNO - CONSULTAS LINQ V3
// =============================================================================
// Select vs SelectMany:
// - Select: transforma cada elemento en UN resultado (uno a uno)
// - SelectMany: transforma cada elemento en VARIOS resultados (uno a muchos)
//   EJEMPLO: un pokemon tiene varios tipos -> SelectMany para obtener todos los tipos
//
// GroupBy + ToDictionary vs GroupBy + Select + ToList:
// - ToDictionary<Clave, Valor>: crea un diccionario con UNA SOLA lista por clave
//   Ejemplo: type -> cantidad (int)
// - Select + ToList: crea diccionario con LISTA de elementos por clave
//   Ejemplo: habilidad -> lista de pokemons que tienen esa habilidad
//
// CONSULTA 50 - Ejemplo avanzado: 
// El campo TotalStats permite hacer consultas más complejas sin calcular cada vez
// =============================================================================

string FormatGeneration(string gen) => gen.ToUpperInvariant();

void Main()
{
    WriteLine("========================================");
    WriteLine("  POKEDEX V3 - CONSULTA DE DATOS");
    WriteLine("  1025 Pokemon - Generaciones 1-9");
    WriteLine("========================================");
    WriteLine();

    IPokedexRepository repository = PokedexRepository.Instance;
    IPokedexStorage storage = new PokedexJsonStorage();
    var service = new PokedexService(repository, storage);

    var pokemons = service.GetAll().ToList();
    
    // Diccionario para búsquedas por ID
    var pokemonPorId = pokemons.ToDictionary(p => p.Id);

    WriteLine($"Pokemons cargados: {pokemons.Count}");
    WriteLine();

    // =============================================================================
    // CONSULTA 1: Todos los pokemons
    // SQL: SELECT id, name, type FROM pokemons ORDER BY id
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 1: Todos los pokemons");
    WriteLine("========================================");
    foreach (var p in pokemons)
        WriteLine($"  #{p.Id:D3} {p.Name} - {string.Join(", ", p.Type)}");
    WriteLine();

    // =============================================================================
    // CONSULTA 2: Pokemon con id 150 (Mewtwo)
    // SQL: SELECT * FROM pokemons WHERE id = 150
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 2: Pokemon con id 150");
    WriteLine("========================================");
    var pokemonId150 = pokemons.FirstOrDefault(p => p.Id == 150);
    WriteLine($"  {pokemonId150?.Name}");
    WriteLine();

    // =============================================================================
    // CONSULTA 3: Número de pokemons
    // SQL: SELECT COUNT(*) AS total FROM pokemons
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 3: Numero de pokemons");
    WriteLine("========================================");
    WriteLine($"  Total: {pokemons.Count}");
    WriteLine();

    // =============================================================================
    // CONSULTA 4: 10 primeros pokemons
    // SQL: SELECT * FROM pokemons ORDER BY id LIMIT 10
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 4: 10 primeros pokemons");
    WriteLine("========================================");
    foreach (var p in pokemons.Take(10))
        WriteLine($"  #{p.Id:D3} {p.Name}");
    WriteLine();

    // =============================================================================
    // CONSULTA 5: Pokemon más pesado
    // SQL: SELECT * FROM pokemons ORDER BY weight DESC LIMIT 1
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 5: Pokemon mas pesado");
    WriteLine("========================================");
    var masPesado = pokemons.MaxBy(p => p.Weight);
    WriteLine($"  {masPesado?.Name}: {masPesado?.Weight} kg");
    WriteLine();

    // =============================================================================
    // CONSULTA 6: Pokemon más ligero
    // SQL: SELECT * FROM pokemons ORDER BY weight ASC LIMIT 1
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 6: Pokemon mas ligero");
    WriteLine("========================================");
    var masLigero = pokemons.MinBy(p => p.Weight);
    WriteLine($"  {masLigero?.Name}: {masLigero?.Weight} kg");
    WriteLine();

    // =============================================================================
    // CONSULTA 7: Pokemon Legendaries
    // SQL: SELECT * FROM pokemons WHERE is_legendary = true
    // LINQ: Where con campo IsLegendary (nuevo campo V3)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 7: Pokemons Legendaries");
    WriteLine("========================================");
    var legendaries = pokemons.Where(p => p.IsLegendary).ToList();
    WriteLine($"  Total: {legendaries.Count}");
        foreach (var p in legendaries)
        WriteLine($"  - {p.Name} (#{p.Id}) - {FormatGeneration(p.Generation)}");
    WriteLine();

    // =============================================================================
    // CONSULTA 8: Pokemon Mythicals
    // SQL: SELECT * FROM pokemons WHERE is_mythical = true
    // LINQ: Where con campo IsMythical (nuevo campo V3)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 8: Pokemons Mythicals");
    WriteLine("========================================");
    var mythicals = pokemons.Where(p => p.IsMythical).ToList();
    WriteLine($"  Total: {mythicals.Count}");
    foreach (var p in mythicals)
        WriteLine($"  - {p.Name} (#{p.Id}) - {FormatGeneration(p.Generation)}");
    WriteLine();

    // =============================================================================
    // CONSULTA 9: Pokemons por Generación (NUEVO V3)
    // SQL: SELECT generation, COUNT(*) FROM pokemons GROUP BY generation
    // LINQ: GroupBy + ToDictionary - muestra cuántos pokemons hay por generación
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 9: Pokemons por Generacion");
    WriteLine("========================================");
    var pokemonsPorGen = pokemons
        .GroupBy(p => p.Generation)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in pokemonsPorGen)
        WriteLine($"  {FormatGeneration(item.Key)}: {item.Value} pokemons");
    WriteLine();

    // =============================================================================
    // CONSULTA 10: Pokemons eléctricos
    // SQL: SELECT * FROM pokemons WHERE 'Electric' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 10: Pokemons electricos");
    WriteLine("========================================");
    var electricos = pokemons.Where(p => p.Type.Contains("Electric")).ToList();
    WriteLine($"  Total: {electricos.Count}");
    foreach (var p in electricos)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 11: Pikachu - INFO COMPLETA (CON DATOS FULL)
    // SQL: SELECT * FROM pokemons WHERE name = 'Pikachu'
    // LINQ: FirstOrDefault - muestra toda la información del pokemon
    // INCLUYE: descripciones de habilidades, cries, evoluciones (V3 FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 11: Pikachu (informacion FULL)");
    WriteLine("========================================");
    var pikachu = pokemons.FirstOrDefault(p => p.Name == "Pikachu");
    if (pikachu != null)
    {
        WriteLine($"  ID: #{pikachu.Id}");
        WriteLine($"  Nombre: {pikachu.Name}");
        WriteLine($"  Genero: {pikachu.Genus}");
        WriteLine($"  Tipo: {string.Join(", ", pikachu.Type)}");
        WriteLine($"  Especie: {pikachu.Species}");
        WriteLine($"  Descripcion: {pikachu.Description}");
        WriteLine($"  Altura: {pikachu.Height} m");
        WriteLine($"  Peso: {pikachu.Weight} kg");
        WriteLine($"  Genero: {pikachu.GenderRatio}");
        WriteLine($"  Huevos: {string.Join(", ", pikachu.EggGroups)}");
        WriteLine($"  Habilidades (CON DESCRIPCION):");
        foreach (var ab in pikachu.Abilities)
        {
            var oculto = ab.IsHidden ? " (oculta)" : "";
            WriteLine($"    - {ab.Name}{oculto}");
            if (!string.IsNullOrEmpty(ab.Description))
                WriteLine($"      {ab.Description}");
        }
        WriteLine($"  Habitat: {pikachu.Habitat}");
        WriteLine($"  Color: {pikachu.Color}");
        WriteLine($"  Forma: {pikachu.Shape}");
        WriteLine($"  Generacion: {pikachu.Generation}");
        WriteLine($"  Captura: {pikachu.CaptureRate}");
        WriteLine($"  Felicidad: {pikachu.BaseHappiness}");
        WriteLine($"  Legendario: {(pikachu.IsLegendary ? "Si" : "No")}");
        WriteLine($"  Mitico: {(pikachu.IsMythical ? "Si" : "No")}");
        WriteLine($"  Base XP: {pikachu.BaseExperience}");
        WriteLine($"  Grito (Cry): {pikachu.Cry}");
        WriteLine($"  Evoluciones (FULL):");
        if (pikachu.PrevEvolution?.Count > 0)
            foreach (var evo in pikachu.PrevEvolution)
                WriteLine($"    <- {evo.Name} ({evo.Condition})");
        if (pikachu.NextEvolution?.Count > 0)
            foreach (var evo in pikachu.NextEvolution)
                WriteLine($"    -> {evo.Name} ({evo.Condition})");
        if (pikachu.PrevEvolution?.Count == 0 && pikachu.NextEvolution?.Count == 0)
            WriteLine($"    (Sin evoluciones)");
        WriteLine($"  Movimientos (CON DETALLES):");
        foreach (var move in pikachu.Moves.Take(5))
        {
            WriteLine($"    - {move.Name}");
            WriteLine($"      Tipo: {move.Type}, Potencia: {move.Power}, Precision: {move.Accuracy}, PP: {move.PP}");
            if (!string.IsNullOrEmpty(move.Description))
                WriteLine($"      {move.Description}");
        }
        WriteLine($"  Base Stats:");
        WriteLine($"    HP: {pikachu.Base.HP}");
        WriteLine($"    Attack: {pikachu.Base.Attack}");
        WriteLine($"    Defense: {pikachu.Base.Defense}");
        WriteLine($"    Sp. Attack: {pikachu.Base.SpAttack}");
        WriteLine($"    Sp. Defense: {pikachu.Base.SpDefense}");
        WriteLine($"    Speed: {pikachu.Base.Speed}");
        WriteLine($"    TOTAL: {pikachu.TotalStats}");
        WriteLine($"  Sprite: {pikachu.Sprite}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 12: Número de pokemons por tipo
    // SQL: SELECT type, COUNT(*) FROM pokemons CROSS APPLY (SELECT type FROM UNNEST(types)) GROUP BY type
    // LINQ: SelectMany (un pokemon tiene varios tipos) + GroupBy + ToDictionary
    // =============================================================================
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

    // =============================================================================
    // CONSULTA 13: Pokemons por Habitat (NUEVO V3)
    // SQL: SELECT habitat, COUNT(*) FROM pokemons WHERE habitat IS NOT NULL GROUP BY habitat
    // LINQ: GroupBy + ToDictionary - distribución de pokemons por hábitat
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 13: Pokemons por Habitat");
    WriteLine("========================================");
    var pokemonsPorHabitat = pokemons
        .Where(p => !string.IsNullOrEmpty(p.Habitat))
        .GroupBy(p => p.Habitat!)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in pokemonsPorHabitat)
        WriteLine($"  {item.Key}: {item.Value} pokemons");
    WriteLine();

    // =============================================================================
    // CONSULTA 14: Pokemons por Color (NUEVO V3)
    // SQL: SELECT color, COUNT(*) FROM pokemons GROUP BY color
    // LINQ: GroupBy + ToDictionary
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 14: Pokemons por Color");
    WriteLine("========================================");
    var pokemonsPorColor = pokemons
        .Where(p => !string.IsNullOrEmpty(p.Color))
        .GroupBy(p => p.Color!)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in pokemonsPorColor)
        WriteLine($"  {item.Key}: {item.Value} pokemons");
    WriteLine();

    // =============================================================================
    // CONSULTA 15: Pokemons por Habilidad (CON DESCRIPCION - FULL)
    // SQL: SELECT ability, GROUP_CONCAT(name ORDER BY name) FROM pokemons GROUP BY ability
    // LINQ: SelectMany + GroupBy + ToDictionary<clave, List<valor>>
    // AHORA: Muestra la descripción de cada habilidad (FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 15: Pokemons por habilidad (FULL)");
    WriteLine("========================================");
    var pokemonsPorHabilidad = pokemons
        .SelectMany(p => p.Abilities.Select(a => new { Habilidad = a.Name, Pokemon = p.Name, Desc = a.Description }))
        .GroupBy(x => x.Habilidad)
        .ToDictionary(g => g.Key, g => g.Select(x => (x.Pokemon, x.Desc)).ToList());
    
    foreach (var item in pokemonsPorHabilidad.OrderBy(x => x.Key).Take(3))
    {
        WriteLine($"  {item.Key}: {item.Value.Count} pokemons");
        if (!string.IsNullOrEmpty(item.Value[0].Desc))
            WriteLine($"    Desc: {item.Value[0].Desc}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 16: Pokemons con evoluciones (V3 FULL - datos de evoluciones)
    // SQL: SELECT * FROM pokemons WHERE next_evolution IS NOT NULL
    // LINQ: Where con NextEvolution
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 16: Pokemons con evoluciones (FULL)");
    WriteLine("========================================");
    var conEvoluciones = pokemons
        .Where(p => p.NextEvolution?.Count > 0 || p.PrevEvolution?.Count > 0)
        .ToList();
    WriteLine($"  Total: {conEvoluciones.Count}");
    foreach (var p in conEvoluciones.Take(10))
    {
        var prev = p.PrevEvolution?.Select(e => e.Name).ToList();
        var next = p.NextEvolution?.Select(e => e.Name).ToList();
        WriteLine($"  - #{p.Id} {p.Name}");
        if (prev?.Count > 0)
            WriteLine($"    Prev: {string.Join(", ", prev)}");
        if (next?.Count > 0)
            WriteLine($"    Next: {string.Join(", ", next)}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 17: Pokemons tipo agua y planta
    // SQL: SELECT * FROM pokemons WHERE 'Water' IN (type1, type2) AND 'Grass' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 17: Pokemons tipo Agua-Planta");
    WriteLine("========================================");
    var aguaPlanta = pokemons
        .Where(p => p.Type.Contains("Water") && p.Type.Contains("Grass"))
        .Select(p => p.Name)
        .ToList();
    
    foreach (var nombre in aguaPlanta)
        WriteLine($"  {nombre}");
    WriteLine();

    // =============================================================================
    // CONSULTA 18: Pokemons por tipo secundario
    // SQL: SELECT type2, COUNT(*) FROM pokemons WHERE type2 IS NOT NULL GROUP BY type2
    // =============================================================================
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

    // =============================================================================
    // CONSULTA 19: Pokemons planta (Grass)
    // SQL: SELECT * FROM pokemons WHERE 'Grass' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 19: Pokemons tipo Planta");
    WriteLine("========================================");
    var pokemonsGrass = pokemons
        .Where(p => p.Type.Contains("Grass"))
        .ToList();
    WriteLine($"  Total: {pokemonsGrass.Count}");
    foreach (var p in pokemonsGrass)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 20: Top 5 pokemons más pesados
    // SQL: SELECT * FROM pokemons ORDER BY weight DESC LIMIT 5
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 20: Top 5 pokemons mas pesados");
    WriteLine("========================================");
    var top5Pesados = pokemons
        .OrderByDescending(p => p.Weight)
        .Take(5);
    foreach (var p in top5Pesados)
        WriteLine($"  - {p.Name}: {p.Weight} kg");
    WriteLine();

    // =============================================================================
    // CONSULTA 21: Top 5 pokemons más altos
    // SQL: SELECT * FROM pokemons ORDER BY height DESC LIMIT 5
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 21: Top 5 pokemons mas altos");
    WriteLine("========================================");
    var top5Altos = pokemons
        .OrderByDescending(p => p.Height)
        .Take(5);
    foreach (var p in top5Altos)
        WriteLine($"  - {p.Name}: {p.Height} m");
    WriteLine();

    // =============================================================================
    // CONSULTA 22: Pokemons tipo dual
    // SQL: SELECT * FROM pokemons WHERE type1 IS NOT NULL AND type2 IS NOT NULL
    // =============================================================================
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

    // =============================================================================
    // CONSULTA 23: Pokemons por tipo de huevo
    // SQL: SELECT egg, COUNT(*) FROM pokemons CROSS APPLY (SELECT egg FROM UNNEST(eggs)) GROUP BY egg
    // LINQ: SelectMany porque un pokemon puede tener varios huevos
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 23: Pokemons por tipo de huevo");
    WriteLine("========================================");
    var pokemonsPorEgg = pokemons
        .SelectMany(p => p.EggGroups.Select(e => new { Huevo = e, Pokemon = p.Name }))
        .GroupBy(x => x.Huevo)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in pokemonsPorEgg)
        WriteLine($"  {item.Key}: {item.Value} pokemons");
    WriteLine();

    // =============================================================================
    // CONSULTA 24: Pokemons con mayor stat total (USANDO TotalStats - NUEVO V3)
    // SQL: SELECT *, total_stats FROM pokemons ORDER BY total_stats DESC LIMIT 1
    // LINQ: MaxBy con campo TotalStats (calculado previamente)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 24: Pokemon con mayor stat total");
    WriteLine("========================================");
    var mayorStatTotal = pokemons.MaxBy(p => p.TotalStats ?? 0);
    WriteLine($"  #{mayorStatTotal?.Id} {mayorStatTotal?.Name}: {mayorStatTotal?.TotalStats} puntos");
    WriteLine($"    HP: {mayorStatTotal?.Base.HP}, Atk: {mayorStatTotal?.Base.Attack}, Def: {mayorStatTotal?.Base.Defense}");
    WriteLine($"    SpA: {mayorStatTotal?.Base.SpAttack}, SpD: {mayorStatTotal?.Base.SpDefense}, Spe: {mayorStatTotal?.Base.Speed}");
    WriteLine();

    // =============================================================================
    // CONSULTA 25: Top 10 Pokemon por stat total (USANDO TotalStats - NUEVO V3)
    // SQL: SELECT *, total_stats FROM pokemons ORDER BY total_stats DESC LIMIT 10
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 25: Top 10 Pokemon (stat total)");
    WriteLine("========================================");
    var top10 = pokemons
        .OrderByDescending(p => p.TotalStats ?? 0)
        .Take(10);
    foreach (var p in top10)
    {
        WriteLine($"  - #{p.Id:D3} {p.Name}: {p.TotalStats} puntos");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 26: Pokemon con mayor ataque
    // SQL: SELECT * FROM pokemons ORDER BY attack DESC LIMIT 1
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 26: Pokemon con mayor ataque");
    WriteLine("========================================");
    var mayorAtaque = pokemons.MaxBy(p => p.Base.Attack);
    WriteLine($"  #{mayorAtaque?.Id} {mayorAtaque?.Name}: {mayorAtaque?.Base.Attack} attack");
    WriteLine();

    // =============================================================================
    // CONSULTA 27: Pokemons con habilidad oculta
    // SQL: SELECT * FROM pokemons WHERE EXISTS (SELECT 1 FROM abilities WHERE is_hidden = true)
    // LINQ: Where con Any(IsHidden)
    // AHORA: CON DESCRIPCION DE HABILIDADES (FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 27: Pokemons con habilidad oculta (FULL)");
    WriteLine("========================================");
    var habilidadOculta = pokemons
        .Where(p => p.Abilities.Any(a => a.IsHidden))
        .ToList();
    WriteLine($"  Total: {habilidadOculta.Count}");
    foreach (var p in habilidadOculta.Take(10))
    {
        var oculta = p.Abilities.FirstOrDefault(a => a.IsHidden);
        WriteLine($"  - {p.Name}: {oculta?.Name}");
        if (!string.IsNullOrEmpty(oculta?.Description))
            WriteLine($"    {oculta.Description}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 28: Top 5 velocidad
    // SQL: SELECT * FROM pokemons ORDER BY speed DESC LIMIT 5
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 28: Top 5 velocidad");
    WriteLine("========================================");
    var top5Velocidad = pokemons
        .OrderByDescending(p => p.Base.Speed)
        .Take(5);
    foreach (var p in top5Velocidad)
        WriteLine($"  - {p.Name}: {p.Base.Speed} speed");
    WriteLine();

    // =============================================================================
    // CONSULTA 29: Pokemons por género
    // SQL: SELECT gender_ratio, COUNT(*) FROM pokemons GROUP BY gender_ratio
    // LINQ: GroupBy + ToDictionary
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 29: Pokemons por genero");
    WriteLine("========================================");
    var pokemonsPorGenero = pokemons
        .GroupBy(p => p.GenderRatio)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in pokemonsPorGenero)
    {
        WriteLine($"  {item.Key}: {item.Value} pokemons");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 30: Pokemons tipo bicho (Bug)
    // SQL: SELECT * FROM pokemons WHERE 'Bug' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 30: Pokemons tipo Bicho");
    WriteLine("========================================");
    var pokemonsBug = pokemons
        .Where(p => p.Type.Contains("Bug"))
        .ToList();
    WriteLine($"  Total: {pokemonsBug.Count}");
    foreach (var p in pokemonsBug)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 31: Top 10 defensa
    // SQL: SELECT * FROM pokemons ORDER BY defense DESC LIMIT 10
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 31: Top 10 defensa");
    WriteLine("========================================");
    var top10Defensa = pokemons
        .OrderByDescending(p => p.Base.Defense)
        .Take(10);
    foreach (var p in top10Defensa)
        WriteLine($"  - {p.Name}: {p.Base.Defense}");
    WriteLine();

    // =============================================================================
    // CONSULTA 32: Pokemons sin tipo secundario
    // SQL: SELECT * FROM pokemons WHERE type2 IS NULL
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 32: Pokemons tipo unico");
    WriteLine("========================================");
    var tipoUnico = pokemons
        .Where(p => p.Type.Count == 1)
        .ToList();
    WriteLine($"  Total: {tipoUnico.Count}");
    foreach (var p in tipoUnico)
        WriteLine($"  - {p.Name}: {p.Type[0]}");
    WriteLine();

    // =============================================================================
    // CONSULTA 33: Pokemons Legendaries por Generación (NUEVO V3)
    // SQL: SELECT generation, COUNT(*) FROM pokemons WHERE is_legendary = true GROUP BY generation
    // LINQ: Where + GroupBy + ToDictionary
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 33: Legendaries por Generacion");
    WriteLine("========================================");
    var legendariesPorGen = pokemons
        .Where(p => p.IsLegendary)
        .GroupBy(p => p.Generation)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in legendariesPorGen)
        WriteLine($"  {FormatGeneration(item.Key)}: {item.Value} legendaries");
    WriteLine();

    // =============================================================================
    // CONSULTA 34: Pokemons con mayor tasa de captura (NUEVO V3)
    // SQL: SELECT * FROM pokemons ORDER BY capture_rate DESC LIMIT 10
    // LINQ: OrderByDescending + Take
    // AHORA: CON CRY (FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 34: Pokemons mas faceis de capturar (FULL)");
    WriteLine("========================================");
    var facilCapturar = pokemons
        .Where(p => p.CaptureRate.HasValue)
        .OrderByDescending(p => p.CaptureRate)
        .Take(10);
    foreach (var p in facilCapturar)
    {
        WriteLine($"  - {p.Name}: tasa {p.CaptureRate}");
        if (!string.IsNullOrEmpty(p.Cry))
            WriteLine($"    Cry: {p.Cry}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 35: Pokemons con menor tasa de captura (mas raros)
    // SQL: SELECT * FROM pokemons ORDER BY capture_rate ASC LIMIT 10
    // LINQ: OrderBy + Take
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 35: Pokemons mas raros (difciles de capturar)");
    WriteLine("========================================");
    var raros = pokemons
        .Where(p => p.CaptureRate.HasValue)
        .OrderBy(p => p.CaptureRate)
        .Take(10);
    foreach (var p in raros)
        WriteLine($"  - {p.Name}: tasa {p.CaptureRate}");
    WriteLine();

    // =============================================================================
    // CONSULTA 36: Top 5 HP
    // SQL: SELECT * FROM pokemons ORDER BY HP DESC LIMIT 5
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 36: Top 5 HP");
    WriteLine("========================================");
    var top5HP = pokemons
        .OrderByDescending(p => p.Base.HP)
        .Take(5);
    foreach (var p in top5HP)
        WriteLine($"  - {p.Name}: {p.Base.HP}");
    WriteLine();

    // =============================================================================
    // CONSULTA 37: Promedio de stats por tipo
    // SQL: SELECT type, AVG(HP), AVG(Attack), AVG(Speed) FROM pokemons CROSS APPLY GROUP BY type
    // LINQ: SelectMany + GroupBy + Average
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 37: Promedio de stats por tipo");
    WriteLine("========================================");
    var promedioStatsPorTipo = pokemons
        .SelectMany(p => p.Type.Select(t => new { Tipo = t, Pokemon = p }))
        .GroupBy(x => x.Tipo)
        .ToDictionary(
            g => g.Key, 
            g => new {
                Count = g.Count(),
                AvgHP = g.Average(x => x.Pokemon.Base.HP),
                AvgAttack = g.Average(x => x.Pokemon.Base.Attack),
                AvgSpeed = g.Average(x => x.Pokemon.Base.Speed)
            })
        .OrderByDescending(x => x.Value.Count);
    
    foreach (var item in promedioStatsPorTipo.Take(5))
    {
        WriteLine($"  {item.Key}: {item.Value.Count} pokemons");
        WriteLine($"    HP: {item.Value.AvgHP:F1}, Atk: {item.Value.AvgAttack:F1}, Spe: {item.Value.AvgSpeed:F1}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 38: Pokemons tipo fuego (Fire)
    // SQL: SELECT * FROM pokemons WHERE 'Fire' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 38: Pokemons tipo Fuego");
    WriteLine("========================================");
    var pokemonsFire = pokemons
        .Where(p => p.Type.Contains("Fire"))
        .ToList();
    WriteLine($"  Total: {pokemonsFire.Count}");
    foreach (var p in pokemonsFire)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 39: Pokemons tipo agua (Water)
    // SQL: SELECT * FROM pokemons WHERE 'Water' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 39: Pokemons tipo Agua");
    WriteLine("========================================");
    var pokemonsWater = pokemons
        .Where(p => p.Type.Contains("Water"))
        .ToList();
    WriteLine($"  Total: {pokemonsWater.Count}");
    foreach (var p in pokemonsWater)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 40: Pokemons con variedades
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 40: Pokemons con variedades");
    WriteLine("========================================");
    var conVariedades = pokemons
        .Where(p => p.Varieties.Count > 1)
        .ToList();
    WriteLine($"  Total: {conVariedades.Count}");
    foreach (var p in conVariedades.Take(10))
    {
        WriteLine($"  - {p.Name}: {string.Join(", ", p.Varieties)}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 40B: Movimientos con potencia maxima (V3 FULL)
    // SQL: SELECT * FROM pokemons.moves WHERE power IS NOT NULL ORDER BY power DESC
    // LINQ: SelectMany + OrderByDescending (nuevo en FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 40B: Movimientos mas potentes (FULL)");
    WriteLine("========================================");
    var movimientosPoderosos = pokemons
        .SelectMany(p => p.Moves.Select(m => new { Pokemon = p.Name, Move = m }))
        .Where(x => x.Move.Power.HasValue)
        .OrderByDescending(x => x.Move.Power)
        .Take(10);
    foreach (var m in movimientosPoderosos)
    {
        WriteLine($"  - {m.Move.Name} (Potencia: {m.Move.Power}) - {m.Pokemon}");
        WriteLine($"    Tipo: {m.Move.Type}, Precision: {m.Move.Accuracy}, PP: {m.Move.PP}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 41: Pokemons con dos habilidades
    // SQL: SELECT * FROM pokemons WHERE array_length(abilities) = 2
    // LINQ: Where con Abilities.Count == 2
    // AHORA: CON DESCRIPCION DE HABILIDADES (FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 41: Pokemons con dos habilidades (FULL)");
    WriteLine("========================================");
    var dosHabilidades = pokemons
        .Where(p => p.Abilities.Count == 2)
        .ToList();
    WriteLine($"  Total: {dosHabilidades.Count}");
    foreach (var p in dosHabilidades.Take(5))
    {
        WriteLine($"  - {p.Name} (#{p.Id}):");
        foreach (var ab in p.Abilities)
        {
            var oculto = ab.IsHidden ? " (oculta)" : "";
            WriteLine($"    - {ab.Name}{oculto}");
            if (!string.IsNullOrEmpty(ab.Description))
                WriteLine($"      {ab.Description}");
        }
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 42: Pokemons con tres habilidades
    // SQL: SELECT * FROM pokemons WHERE array_length(abilities) = 3
    // AHORA: CON DESCRIPCION (FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 42: Pokemons con tres habilidades (FULL)");
    WriteLine("========================================");
    var tresHabilidades = pokemons
        .Where(p => p.Abilities.Count == 3)
        .ToList();
    WriteLine($"  Total: {tresHabilidades.Count}");
    foreach (var p in tresHabilidades.Take(5))
    {
        WriteLine($"  - {p.Name} (#{p.Id}):");
        foreach (var ab in p.Abilities)
        {
            var oculto = ab.IsHidden ? " (oculta)" : "";
            WriteLine($"    - {ab.Name}{oculto}");
            if (!string.IsNullOrEmpty(ab.Description))
                WriteLine($"      {ab.Description}");
        }
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 43: Top 5 defensa especial (Sp. Defense)
    // SQL: SELECT * FROM pokemons ORDER BY sp_defense DESC LIMIT 5
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 43: Top 5 defensa especial");
    WriteLine("========================================");
    var top5SpDef = pokemons
        .OrderByDescending(p => p.Base.SpDefense)
        .Take(5);
    foreach (var p in top5SpDef)
        WriteLine($"  - {p.Name} (#{p.Id}): {p.Base.SpDefense}");
    WriteLine();

    // =============================================================================
    // CONSULTA 44: Top 5 ataque especial (Sp. Attack)
    // SQL: SELECT * FROM pokemons ORDER BY sp_attack DESC LIMIT 5
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 44: Top 5 ataque especial");
    WriteLine("========================================");
    var top5SpAtk = pokemons
        .OrderByDescending(p => p.Base.SpAttack)
        .Take(5);
    foreach (var p in top5SpAtk)
        WriteLine($"  - {p.Name} (#{p.Id}): {p.Base.SpAttack}");
    WriteLine();

    // =============================================================================
    // CONSULTA 45: Pokemon con mejor ratio ataque/defensa
    // SQL: SELECT *, (attack / defense) AS ratio FROM pokemons ORDER BY ratio DESC LIMIT 1
    // LINQ: OrderByDescending con división
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 45: Mejor ratio Ataque/Defensa");
    WriteLine("========================================");
    var mejorRatio = pokemons.MaxBy(p => p.Base.Attack / (double)p.Base.Defense);
    if (mejorRatio != null)
    {
        var ratio = mejorRatio.Base.Attack / (double)mejorRatio.Base.Defense;
        WriteLine($"  #{mejorRatio.Id} {mejorRatio.Name}: {ratio:F2}");
        WriteLine($"    Ataque: {mejorRatio.Base.Attack}, Defensa: {mejorRatio.Base.Defense}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 46: Pokemons tipo volador (Flying)
    // SQL: SELECT * FROM pokemons WHERE 'Flying' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 46: Pokemons tipo Volador");
    WriteLine("========================================");
    var pokemonsVolador = pokemons
        .Where(p => p.Type.Contains("Flying"))
        .ToList();
    WriteLine($"  Total: {pokemonsVolador.Count}");
    foreach (var p in pokemonsVolador)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 47: Pokemons tipo Psiquico (Psychic)
    // SQL: SELECT * FROM pokemons WHERE 'Psychic' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 47: Pokemons tipo Psiquico");
    WriteLine("========================================");
    var pokemonsPsychic = pokemons
        .Where(p => p.Type.Contains("Psychic"))
        .ToList();
    WriteLine($"  Total: {pokemonsPsychic.Count}");
    foreach (var p in pokemonsPsychic)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 48: Pokemons tipo Dragon
    // SQL: SELECT * FROM pokemons WHERE 'Dragon' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 48: Pokemons tipo Dragon");
    WriteLine("========================================");
    var pokemonsDragon = pokemons
        .Where(p => p.Type.Contains("Dragon"))
        .ToList();
    WriteLine($"  Total: {pokemonsDragon.Count}");
    foreach (var p in pokemonsDragon)
        WriteLine($"  - {p.Name} (#{p.Id})");
    WriteLine();

    // =============================================================================
    // CONSULTA 49: Pokemon con mejor media de stats
    // SQL: SELECT *, AVG(all_stats) FROM pokemons ORDER BY AVG DESC LIMIT 1
    // LINQ: Calcular promedio de stats usando TotalStats
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 49: Pokemon con mejor media de stats");
    WriteLine("========================================");
    var mejorMedia = pokemons.MaxBy(p => (p.TotalStats ?? 0) / 6.0);
    if (mejorMedia != null)
    {
        var media = (mejorMedia.TotalStats ?? 0) / 6.0;
        WriteLine($"  #{mejorMedia.Id} {mejorMedia.Name}: media {media:F1}");
        WriteLine($"    Total: {mejorMedia.TotalStats}, Media: {media:F1}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 50: Pokemons por tipo (agrupados con lista)
    // SQL: SELECT type, GROUP_CONCAT(name) FROM pokemons GROUP BY type
    // LINQ: SelectMany + GroupBy + ToDictionary<clave, List<valor>>
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 50: Pokemons por tipo (con lista)");
    WriteLine("========================================");
    var pokemonsPorTipoLista = pokemons
        .SelectMany(p => p.Type.Select(t => new { Tipo = t, Pokemon = p.Name }))
        .GroupBy(x => x.Tipo)
        .ToDictionary(g => g.Key, g => g.Select(x => x.Pokemon).ToList());
    
    foreach (var item in pokemonsPorTipoLista.OrderByDescending(x => x.Value.Count).Take(5))
    {
        WriteLine($"  {item.Key}: {item.Value.Count} pokemons");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 51: Pokemons por tipo de movimiento (V3 FULL - solo disponible en full)
    // SQL: SELECT damage_class, COUNT(*) FROM moves GROUP BY damage_class
    // LINQ: SelectMany de moves + GroupBy (nuevo en FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 51: Movimientos por tipo (FULL)");
    WriteLine("========================================");
    var movimientosPorTipo = pokemons
        .SelectMany(p => p.Moves)
        .Where(m => !string.IsNullOrEmpty(m.Type))
        .GroupBy(m => m.Type)
        .ToDictionary(g => g.Key!, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in movimientosPorTipo.Take(10))
    {
        WriteLine($"  {item.Key}: {item.Value} movimientos");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 52: Pokemons con movimientos de tipo Fuego mas potentes (V3 FULL)
    // SQL: SELECT * FROM moves WHERE type = 'fire' ORDER BY power DESC
    // LINQ: SelectMany + Where + OrderByDescending (nuevo en FULL)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 52: Movimientos de Fuego mas potentes (FULL)");
    WriteLine("========================================");
    var movimientosFuego = pokemons
        .SelectMany(p => p.Moves.Select(m => new { Pokemon = p.Name, Move = m }))
        .Where(x => x.Move.Type == "Fire" && x.Move.Power.HasValue)
        .OrderByDescending(x => x.Move.Power)
        .Take(10);
    foreach (var m in movimientosFuego)
    {
        WriteLine($"  - {m.Move.Name} (Potencia: {m.Move.Power}) - {m.Pokemon}");
        WriteLine($"    PP: {m.Move.PP}, Precision: {m.Move.Accuracy}, Clase: {m.Move.DamageClass}");
    }
    WriteLine();

    WriteLine("========================================");
    WriteLine("  PROGRAMA FINALIZADO");
    WriteLine("========================================");
}
