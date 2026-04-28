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
// NOTAS PARA EL ALUMNO - CONSULTAS LINQ
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
// =============================================================================

void Main()
{
    WriteLine("========================================");
    WriteLine("  POKEDEX V2 - CONSULTA DE DATOS");
    WriteLine("========================================");
    WriteLine();

    IPokedexRepository repository = PokedexRepository.Instance;
    IPokedexStorage storage = new PokedexJsonStorage();
    var service = new PokedexService(repository, storage);

    var pokemons = service.GetAll().ToList();
    
    // Diccionario para búsquedas por ID (útil para mostrar nombres en evoluciones)
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
    // CONSULTA 2: Pokemon con id 10
    // SQL: SELECT * FROM pokemons WHERE id = 10
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 2: Pokemon con id 10");
    WriteLine("========================================");
    var pokemonId10 = pokemons.FirstOrDefault(p => p.Id == 10);
    WriteLine($"  {pokemonId10?.Name}");
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
    // CONSULTA 7: Pokemon con más evoluciones
    // SQL: SELECT * FROM pokemons WHERE next_evolution IS NOT NULL ORDER BY (SELECT COUNT(*)) DESC LIMIT 1
    // LINQ: MaxBy con Count de NextEvolution - muestra ID y NOMBRE de cada evolucion
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 7: Pokemon con mas evoluciones");
    WriteLine("========================================");
    var masEvoluciones = pokemons.MaxBy(p => p.NextEvolution?.Count ?? 0);
    WriteLine($"  #{masEvoluciones?.Id} {masEvoluciones?.Name}: {masEvoluciones?.NextEvolution?.Count ?? 0} evoluciones");
    if (masEvoluciones?.NextEvolution?.Count > 0)
    {
        WriteLine("  Evoluciones:");
        foreach (var ev in masEvoluciones.NextEvolution!)
        {
            var nombreEvol = pokemonPorId.TryGetValue(ev.Id ?? 0, out var pE) ? pE.Name : "Desconocido";
            WriteLine($"    - #{ev.Id} {nombreEvol} ({ev.Condition})");
        }
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 8: Pokemon con menos evoluciones
    // SQL: SELECT * FROM pokemons ORDER BY (SELECT COUNT(*) FROM next_evolution) ASC LIMIT 1
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 8: Pokemon con menos evoluciones");
    WriteLine("========================================");
    var menosEvoluciones = pokemons.MinBy(p => p.NextEvolution?.Count ?? 0);
    WriteLine($"  #{menosEvoluciones?.Id} {menosEvoluciones?.Name}: {menosEvoluciones?.NextEvolution?.Count ?? 0} evoluciones");
    WriteLine();

    // =============================================================================
    // CONSULTA 9: Pokemon con más habilidades
    // SQL: SELECT * FROM pokemons ORDER BY (SELECT COUNT(*) FROM abilities) DESC LIMIT 1
    // LINQ: MaxBy con Count de Abilities - muestra habilidades normales y ocultas
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 9: Pokemon con mas habilidades");
    WriteLine("========================================");
    var masHabilidades = pokemons.MaxBy(p => p.Abilities.Count);
    WriteLine($"  #{masHabilidades?.Id} {masHabilidades?.Name}: {masHabilidades?.Abilities.Count} habilidades");
    foreach (var ab in masHabilidades?.Abilities ?? [])
    {
        var oculto = ab.IsHidden ? " (oculta/hidden)" : "";
        WriteLine($"    - {ab.Name}{oculto}");
    }
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
    // CONSULTA 11: Pikachu - INFO COMPLETA
    // SQL: SELECT * FROM pokemons WHERE name = 'Pikachu'
    // LINQ: FirstOrDefault - muestra toda la información del pokemon
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 11: Pikachu (informacion completa)");
    WriteLine("========================================");
    var pikachu = pokemons.FirstOrDefault(p => p.Name == "Pikachu");
    if (pikachu != null)
    {
        WriteLine($"  ID: #{pikachu.Id}");
        WriteLine($"  Nombre: {pikachu.Name}");
        WriteLine($"  Tipo: {string.Join(", ", pikachu.Type)}");
        WriteLine($"  Especie: {pikachu.Species}");
        WriteLine($"  Descripcion: {pikachu.Description}");
        WriteLine($"  Altura: {pikachu.Height} m");
        WriteLine($"  Peso: {pikachu.Weight} kg");
        WriteLine($"  Genero: {pikachu.GenderRatio}");
        WriteLine($"  Huevos: {string.Join(", ", pikachu.Egg)}");
        WriteLine($"  Habilidades:");
        foreach (var ab in pikachu.Abilities)
        {
            var oculto = ab.IsHidden ? " (oculta)" : "";
            WriteLine($"    - {ab.Name}{oculto}");
        }
        WriteLine($"  Base Stats:");
        WriteLine($"    HP: {pikachu.Base.HP}");
        WriteLine($"    Attack: {pikachu.Base.Attack}");
        WriteLine($"    Defense: {pikachu.Base.Defense}");
        WriteLine($"    Sp. Attack: {pikachu.Base.SpAttack}");
        WriteLine($"    Sp. Defense: {pikachu.Base.SpDefense}");
        WriteLine($"    Speed: {pikachu.Base.Speed}");
        var totalStats = pikachu.Base.HP + pikachu.Base.Attack + pikachu.Base.Defense +
                         pikachu.Base.SpAttack + pikachu.Base.SpDefense + pikachu.Base.Speed;
        WriteLine($"    TOTAL: {totalStats}");
        if (pikachu.PrevEvolution?.Count > 0)
        {
            var prev = pikachu.PrevEvolution[0];
            var nombrePrev = pokemonPorId.TryGetValue(prev.Id ?? 0, out var pP) ? pP.Name : "Desconocido";
            WriteLine($"  Evoluciona de: #{prev.Id} {nombrePrev} ({prev.Condition})");
        }
        if (pikachu.NextEvolution?.Count > 0)
        {
            WriteLine($"  Evoluciona a:");
            foreach (var ev in pikachu.NextEvolution)
            {
                var nombreNext = pokemonPorId.TryGetValue(ev.Id ?? 0, out var pN) ? pN.Name : "Desconocido";
                WriteLine($"    - #{ev.Id} {nombreNext} ({ev.Condition})");
            }
        }
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
    // CONSULTA 13: Habilidades con número de pokemons
    // SQL: SELECT ability, COUNT(*) FROM pokemons CROSS APPLY (SELECT ability FROM UNNEST(abilities)) GROUP BY ability
    // LINQ: SelectMany (un pokemon tiene varias habilidades) + GroupBy + ToDictionary
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 13: Habilidades con conteo");
    WriteLine("========================================");
    var habilidadesConteo = pokemons
        .SelectMany(p => p.Abilities.Select(a => a.Name))
        .GroupBy(a => a)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in habilidadesConteo.Take(10))
        WriteLine($"  {item.Key}: {item.Value}");
    WriteLine();

    // =============================================================================
    // CONSULTA 14: Pokemons tipo agua y planta
    // SQL: SELECT * FROM pokemons WHERE 'Water' IN (type1, type2) AND 'Grass' IN (type1, type2)
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 14: Pokemons tipo Agua-Planta");
    WriteLine("========================================");
    var aguaPlanta = pokemons
        .Where(p => p.Type.Contains("Water") && p.Type.Contains("Grass"))
        .Select(p => p.Name)
        .ToList();
    
    foreach (var nombre in aguaPlanta)
        WriteLine($"  {nombre}");
    WriteLine();

    // =============================================================================
    // CONSULTA 15: Pokemons por habilidad (CON LISTA)
    // SQL: SELECT ability, GROUP_CONCAT(name ORDER BY name) FROM pokemons GROUP BY ability
    // LINQ: SelectMany + GroupBy + ToDictionary<clave, List<valor>>
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 15: Pokemons por habilidad");
    WriteLine("========================================");
    var pokemonsPorHabilidad = pokemons
        .SelectMany(p => p.Abilities.Select(a => new { Habilidad = a.Name, Pokemon = p.Name }))
        .GroupBy(x => x.Habilidad)
        .ToDictionary(g => g.Key, g => g.Select(x => x.Pokemon).ToList());
    
    foreach (var item in pokemonsPorHabilidad.OrderBy(x => x.Key).Take(5))
    {
        WriteLine($"  {item.Key}: {item.Value.Count} pokemons");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 16: Pokemons sin evoluciones
    // SQL: SELECT * FROM pokemons WHERE next_evolution IS NULL AND prev_evolution IS NULL
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 16: Pokemons sin evoluciones");
    WriteLine("========================================");
    var sinEvoluciones = pokemons
        .Where(p => (p.NextEvolution == null || p.NextEvolution.Count == 0) && 
                    (p.PrevEvolution == null || p.PrevEvolution.Count == 0))
        .ToList();
    WriteLine($"  Total: {sinEvoluciones.Count}");
    foreach (var p in sinEvoluciones.Take(10))
        WriteLine($"  - #{p.Id} {p.Name}");
    WriteLine();

    // =============================================================================
    // CONSULTA 17: Pokemons con evolución previa
    // SQL: SELECT * FROM pokemons WHERE prev_evolution IS NOT NULL
    // LINQ: Where con PrevEvolution - muestra NOMBRE (ID) del pokemon origen
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 17: Pokemons con evolucion previa");
    WriteLine("========================================");
    var conEvolucionPrevia = pokemons
        .Where(p => p.PrevEvolution != null && p.PrevEvolution.Count > 0)
        .ToList();
    WriteLine($"  Total: {conEvolucionPrevia.Count}");
    foreach (var p in conEvolucionPrevia)
    {
        var evolucion = p.PrevEvolution?[0];
        var nombreOrigen = pokemonPorId.TryGetValue(evolucion?.Id ?? 0, out var pO) ? pO.Name : "Desconocido";
        var condicion = evolucion?.Condition ?? "N/A";
        WriteLine($"  - {p.Name} (#{p.Id}) viene de {nombreOrigen} (#{evolucion?.Id}) ({condicion})");
    }
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
        .SelectMany(p => p.Egg.Select(e => new { Huevo = e, Pokemon = p.Name }))
        .GroupBy(x => x.Huevo)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in pokemonsPorEgg)
        WriteLine($"  {item.Key}: {item.Value} pokemons");
    WriteLine();

    // =============================================================================
    // CONSULTA 24: Cadenas de evolución completas
    // SQL: SELECT * FROM pokemons WHERE prev_evolution IS NOT NULL AND next_evolution IS NOT NULL
    // LINQ: Where - muestra la cadena completa con NOMBRE (ID) de cada pokemon
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 24: Cadenas de evolucion completas");
    WriteLine("========================================");
    var cadenasEvolucion = pokemons
        .Where(p => p.PrevEvolution != null && p.PrevEvolution.Count > 0 && 
                    p.NextEvolution != null && p.NextEvolution.Count > 0)
        .ToList();
    WriteLine($"  Total en cadenas: {cadenasEvolucion.Count}");
    foreach (var p in cadenasEvolucion)
    {
        // Pokemon origen
        var idOrigen = p.PrevEvolution?[0].Id ?? 0;
        var nombreOrigen = pokemonPorId.TryGetValue(idOrigen, out var pO) ? pO.Name : "Desconocido";
        
        // Pokemon destino(s)
        var destinos = p.NextEvolution?.Select(e => {
            var nombreDest = pokemonPorId.TryGetValue(e.Id ?? 0, out var pD) ? pD.Name : "Desconocido";
            return $"{nombreDest} (#{e.Id}) ({e.Condition})";
        }).ToList() ?? [];
        
        WriteLine($"  - {nombreOrigen} (#{idOrigen}) -> {p.Name} (#{p.Id}) -> {string.Join(", ", destinos)}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 25: Pokemon con mayor stat total
    // SQL: SELECT *, (HP + Attack + Defense + Sp_Attack + Sp_Defense + Speed) AS total FROM pokemons ORDER BY total DESC LIMIT 1
    // LINQ: MaxBy con suma de Base Stats
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 25: Pokemon con mayor stat total");
    WriteLine("========================================");
    var mayorStatTotal = pokemons.MaxBy(p => 
        p.Base.HP + p.Base.Attack + p.Base.Defense + 
        p.Base.SpAttack + p.Base.SpDefense + p.Base.Speed);
    var statTotal = mayorStatTotal != null ? 
        mayorStatTotal.Base.HP + mayorStatTotal.Base.Attack + mayorStatTotal.Base.Defense + 
        mayorStatTotal.Base.SpAttack + mayorStatTotal.Base.SpDefense + mayorStatTotal.Base.Speed : 0;
    WriteLine($"  #{mayorStatTotal?.Id} {mayorStatTotal?.Name}: {statTotal} puntos");
    WriteLine($"    HP: {mayorStatTotal?.Base.HP}, Atk: {mayorStatTotal?.Base.Attack}, Def: {mayorStatTotal?.Base.Defense}");
    WriteLine($"    SpA: {mayorStatTotal?.Base.SpAttack}, SpD: {mayorStatTotal?.Base.SpDefense}, Spe: {mayorStatTotal?.Base.Speed}");
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
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 27: Pokemons con habilidad oculta");
    WriteLine("========================================");
    var habilidadOculta = pokemons
        .Where(p => p.Abilities.Any(a => a.IsHidden))
        .ToList();
    WriteLine($"  Total: {habilidadOculta.Count}");
    foreach (var p in habilidadOculta.Take(10))
    {
        var oculta = p.Abilities.FirstOrDefault(a => a.IsHidden)?.Name ?? "N/A";
        WriteLine($"  - {p.Name}: {oculta}");
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
    // CONSULTA 29: Pokemons por género (FORMATO LEGIBLE)
    // SQL: SELECT gender_ratio, COUNT(*) FROM pokemons GROUP BY gender_ratio
    // LINQ: GroupBy + ToDictionary - formato legible: "87.5% hembra, 12.5% macho"
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 29: Pokemons por genero");
    WriteLine("========================================");
    
    string FormatearGenero(string ratio)
    {
        if (ratio == "Genderless")
            return "Sin genero (Genderless)";
        
        var partes = ratio.Split(':');
        if (partes.Length != 2) return ratio;
        
        if (!double.TryParse(partes[0], out var hembra) || !double.TryParse(partes[1], out var macho))
            return ratio;
        
        if (hembra == 0 && macho == 100)
            return "0% hembra, 100% macho (solo machos)";
        if (hembra == 100 && macho == 0)
            return "100% hembra, 0% macho (solo hembras)";
        
        if (hembra <= 100 && macho <= 100)
            return $"{hembra}% hembra, {macho}% macho";
        
        hembra = hembra / 10;
        macho = macho / 10;
        return $"{hembra}% hembra, {macho}% macho";
    }
    
    var pokemonsPorGenero = pokemons
        .GroupBy(p => p.GenderRatio)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderBy(x => x.Key);
    
    foreach (var item in pokemonsPorGenero)
    {
        WriteLine($"  {FormatearGenero(item.Key)}: {item.Value} pokemons");
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
    // CONSULTA 33: Evolución por nivel
    // SQL: SELECT * FROM pokemons WHERE next_evolution.condition LIKE '%Level%'
    // LINQ: Where con Any(Condition.Contains("Level"))
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 33: Evolucion por nivel");
    WriteLine("========================================");
    var evolucionNivel = pokemons
        .Where(p => p.NextEvolution?.Any(e => e.Condition?.Contains("Level") == true) == true)
        .ToList();
    WriteLine($"  Total: {evolucionNivel.Count}");
    foreach (var p in evolucionNivel)
    {
        var niveles = p.NextEvolution?.Where(e => e.Condition?.Contains("Level") == true)
            .Select(e => e.Condition).ToList();
        WriteLine($"  - {p.Name}: {string.Join(", ", niveles ?? [])}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 34: Evolución con piedra
    // SQL: SELECT * FROM pokemons WHERE next_evolution.condition LIKE '%Stone%'
    // LINQ: Where con Any(Condition.Contains("Stone"))
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 34: Evolucion con piedra");
    WriteLine("========================================");
    var evolucionPiedra = pokemons
        .Where(p => p.NextEvolution?.Any(e => e.Condition?.Contains("Stone") == true) == true)
        .ToList();
    WriteLine($"  Total: {evolucionPiedra.Count}");
    foreach (var p in evolucionPiedra)
    {
        var piedras = p.NextEvolution?.Where(e => e.Condition?.Contains("Stone") == true)
            .Select(e => e.Condition).ToList();
        WriteLine($"  - {p.Name}: {string.Join(", ", piedras ?? [])}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 35: Pokemons por especie
    // SQL: SELECT species, COUNT(*) FROM pokemons GROUP BY species
    // LINQ: GroupBy + ToDictionary
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 35: Pokemons por especie");
    WriteLine("========================================");
    var pokemonsPorEspecie = pokemons
        .GroupBy(p => p.Species)
        .ToDictionary(g => g.Key, g => g.Count())
        .OrderByDescending(x => x.Value);
    
    foreach (var item in pokemonsPorEspecie.Take(10))
        WriteLine($"  {item.Key}: {item.Value}");
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
    // CONSULTA 38: Top 10 Pokemon (por stat total)
    // SQL: SELECT *, (HP + Attack + Defense + Sp_Attack + Sp_Defense + Speed) AS total FROM pokemons ORDER BY total DESC LIMIT 10
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 38: Top 10 Pokemon (stat total)");
    WriteLine("========================================");
    var top10 = pokemons
        .OrderByDescending(p => 
            p.Base.HP + p.Base.Attack + p.Base.Defense + 
            p.Base.SpAttack + p.Base.SpDefense + p.Base.Speed)
        .Take(10);
    foreach (var p in top10)
    {
        var total = p.Base.HP + p.Base.Attack + p.Base.Defense + 
                    p.Base.SpAttack + p.Base.SpDefense + p.Base.Speed;
        WriteLine($"  - #{p.Id} {p.Name}: {total} puntos");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 39: Pokemons que evolucionan por amistad (Friendship)
    // SQL: SELECT * FROM pokemons WHERE next_evolution.condition LIKE '%Friendship%'
    // LINQ: Where con Any(Condition.Contains("Friendship"))
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 39: Evolucion por amistad (Friendship)");
    WriteLine("========================================");
    var evolucionAmistad = pokemons
        .Where(p => p.NextEvolution?.Any(e => e.Condition?.Contains("Friendship") == true) == true)
        .ToList();
    WriteLine($"  Total: {evolucionAmistad.Count}");
    foreach (var p in evolucionAmistad)
    {
        var condiciones = p.NextEvolution?.Where(e => e.Condition?.Contains("Friendship") == true)
            .Select(e => e.Condition).ToList();
        WriteLine($"  - {p.Name} (#{p.Id}): {string.Join(", ", condiciones ?? [])}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 40: Pokemons que evolucionan por intercambio (Trade)
    // SQL: SELECT * FROM pokemons WHERE next_evolution.condition LIKE '%Trade%'
    // LINQ: Where con Any(Condition.Contains("Trade"))
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 40: Evolucion por intercambio (Trade)");
    WriteLine("========================================");
    var evolucionTrade = pokemons
        .Where(p => p.NextEvolution?.Any(e => e.Condition?.Contains("Trade") == true) == true)
        .ToList();
    WriteLine($"  Total: {evolucionTrade.Count}");
    foreach (var p in evolucionTrade)
    {
        var condiciones = p.NextEvolution?.Where(e => e.Condition?.Contains("Trade") == true)
            .Select(e => e.Condition).ToList();
        WriteLine($"  - {p.Name} (#{p.Id}): {string.Join(", ", condiciones ?? [])}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 41: Pokemons con dos habilidades
    // SQL: SELECT * FROM pokemons WHERE array_length(abilities) = 2
    // LINQ: Where con Abilities.Count == 2
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 41: Pokemons con dos habilidades");
    WriteLine("========================================");
    var dosHabilidades = pokemons
        .Where(p => p.Abilities.Count == 2)
        .ToList();
    WriteLine($"  Total: {dosHabilidades.Count}");
    foreach (var p in dosHabilidades.Take(10))
    {
        WriteLine($"  - {p.Name} (#{p.Id}): {string.Join(", ", p.Abilities.Select(a => a.Name))}");
    }
    WriteLine();

    // =============================================================================
    // CONSULTA 42: Pokemons con tres habilidades
    // SQL: SELECT * FROM pokemons WHERE array_length(abilities) = 3
    // LINQ: Where con Abilities.Count == 3
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 42: Pokemons con tres habilidades");
    WriteLine("========================================");
    var tresHabilidades = pokemons
        .Where(p => p.Abilities.Count == 3)
        .ToList();
    WriteLine($"  Total: {tresHabilidades.Count}");
    foreach (var p in tresHabilidades)
    {
        WriteLine($"  - {p.Name} (#{p.Id}): {string.Join(", ", p.Abilities.Select(a => a.Name))}");
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
    // LINQ: OrderByDescending con división - indica si es ofensivo o defensivo
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
    // CONSULTA 49: Pokemon con mayor velocidad media (stats totales / 6)
    // SQL: SELECT *, AVG(all_stats) FROM pokemons ORDER BY AVG DESC LIMIT 1
    // LINQ: Calcular promedio de stats
    // =============================================================================
    WriteLine("========================================");
    WriteLine("  CONSULTA 49: Pokemon con mejor media de stats");
    WriteLine("========================================");
    var mejorMedia = pokemons.MaxBy(p => 
        (p.Base.HP + p.Base.Attack + p.Base.Defense + 
         p.Base.SpAttack + p.Base.SpDefense + p.Base.Speed) / 6.0);
    if (mejorMedia != null)
    {
        var total = mejorMedia.Base.HP + mejorMedia.Base.Attack + mejorMedia.Base.Defense + 
                    mejorMedia.Base.SpAttack + mejorMedia.Base.SpDefense + mejorMedia.Base.Speed;
        var media = total / 6.0;
        WriteLine($"  #{mejorMedia.Id} {mejorMedia.Name}: media {media:F1}");
        WriteLine($"    Total: {total}, Media: {media:F1}");
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

    WriteLine("========================================");
    WriteLine("  PROGRAMA FINALIZADO");
    WriteLine("========================================");
}
