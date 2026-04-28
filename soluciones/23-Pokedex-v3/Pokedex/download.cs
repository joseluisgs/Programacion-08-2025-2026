// =============================================================================
// SCRIPT DE DESCARGA DE DATOS DESDE pokeapi.co
// =============================================================================
// Este script descarga TODOS los Pokemon (1025) de la API pública pokeapi.co
// y los guarda en formato JSON para poder usarlos en el proyecto.
//
// INSTRUCCIONES PARA EL ALUMNO:
// 1. Este archivo NO debe incluirse en la compilación del proyecto
// 2. Para ejecutar este script:
//    - Comentar la línea <Compile Include="download.cs" /> en Pokedex.csproj
//    - Descomentar la línea <Compile Include="download.cs" /> en Pokedex.csproj  
//    - Ejecutar: dotnet run
//    - Luego restaurar Pokedex.csproj a su estado original
//
// DATOS EXTRAÍDOS:
// - id, name, displayName: identificadores
// - type: tipos del pokemon (puede tener 1 o 2)
// - base stats: HP, Attack, Defense, SpAttack, SpDefense, Speed
// - species, genus: información de la especie
// - description: descripción del pokemon
// - height, weight: altura y peso
// - eggGroups: grupos de huevo
// - abilities: habilidades (normales y ocultas)
// - genderRatio: proporción de género
// - sprite, thumbnail, hires: URLs de imágenes
// - generation: generación (Gen I, Gen II, etc.)
// - habitat: hábitat (grassland, cave, water, etc.)
// - color: color del pokemon
// - shape: forma del pokemon
// - captureRate: tasa de captura (1-255)
// - baseHappiness: felicidad base
// - isLegendary, isMythical: indicadores de pokemon especial
// - varieties: variedades de la especie
// - moves: movimientos (solo nombres)
// - totalStats: suma de todos los stats base
// =============================================================================

using System.Net.Http;
using System.Text.Json;
using Pokedex.Models;

// Cliente HTTP para hacer peticiones a la API
var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
var pokemons = new List<Pokemon>();

Console.WriteLine("Descargando TODOS los pokemons de pokeapi.co...");

// pokeapi.co tiene 1025 Pokemon únicos (sin contar formas alternativas)
int total = 1025;
for (int i = 1; i <= total; i++)
{
    try
    {
        Console.Write($"Descargando {i}/{total}... ");
        
        // Hacemos dos peticiones: una para datos del pokemon y otra para datos de la especie
        var pokemonJson = await httpClient.GetStringAsync($"pokemon/{i}");
        var speciesJson = await httpClient.GetStringAsync($"pokemon-species/{i}");
        
        // Parseamos el JSON usando JsonDocument (más eficiente que deserializar a clases)
        using var pokemonDoc = JsonDocument.Parse(pokemonJson);
        using var speciesDoc = JsonDocument.Parse(speciesJson);
        
        var p = pokemonDoc.RootElement;
        var s = speciesDoc.RootElement;
        
        // ============ DATOS BÁSICOS ============
        var id = p.GetProperty("id").GetInt32();
        var name = Capitalize(p.GetProperty("name").GetString()!);
        var height = p.GetProperty("height").GetInt32() / 10.0;  // La API devuelve hectogramos
        var weight = p.GetProperty("weight").GetInt32() / 10.0; // La API devuelve hectogramos
        
        // Base experience
        int baseExperience = 0;
        if (p.TryGetProperty("base_experience", out var be) && be.ValueKind == JsonValueKind.Number)
            baseExperience = be.GetInt32();
        
        // ============ TIPOS ============
        // Un pokemon puede tener 1 o 2 tipos, los ordenamos por slot
        var types = p.GetProperty("types").EnumerateArray()
            .OrderBy(t => t.GetProperty("slot").GetInt32())
            .Select(t => Capitalize(t.GetProperty("type").GetProperty("name").GetString()!))
            .ToList();
        
        // ============ BASE STATS ============
        // Stats base: HP, Attack, Defense, SpAttack, SpDefense, Speed
        int hp = 0, attack = 0, defense = 0, spAttack = 0, spDefense = 0, speed = 0;
        foreach (var stat in p.GetProperty("stats").EnumerateArray())
        {
            var sn = stat.GetProperty("stat").GetProperty("name").GetString()!;
            var sv = stat.GetProperty("base_stat").GetInt32();
            if (sn == "hp") hp = sv;
            else if (sn == "attack") attack = sv;
            else if (sn == "defense") defense = sv;
            else if (sn == "special-attack") spAttack = sv;
            else if (sn == "special-defense") spDefense = sv;
            else if (sn == "speed") speed = sv;
        }
        var stats = new BaseStats(hp, attack, defense, spAttack, spDefense, speed);
        
        // Calculamos el total de stats
        var totalStats = hp + attack + defense + spAttack + spDefense + speed;
        
        // ============ HABILIDADES ============
        // Obtenemos todas las habilidades del pokemon
        var abilities = p.GetProperty("abilities").EnumerateArray()
            .Select(a => new Ability(
                Capitalize(a.GetProperty("ability").GetProperty("name").GetString()!),
                a.GetProperty("is_hidden").GetBoolean(),
                null  // Descripción no disponible sin llamada adicional a la API
            )).ToList();
        
        // ============ DATOS DE LA ESPECIE ============
        var speciesName = Capitalize(s.GetProperty("name").GetString()!);
        
        // Genus - el tipo de Pokemon (ej: "Seed Pokemon")
        var genus = "";
        foreach (var g in s.GetProperty("genera").EnumerateArray())
        {
            if (g.TryGetProperty("language", out var lang) && lang.ValueKind == JsonValueKind.Object)
            {
                if (lang.GetProperty("name").GetString() == "en")
                {
                    genus = g.GetProperty("genus").GetString()!;
                    break;
                }
            }
        }
        
        // Description - texto descriptivo del pokemon
        var description = "";
        foreach (var f in s.GetProperty("flavor_text_entries").EnumerateArray())
        {
            if (f.GetProperty("language").GetProperty("name").GetString() == "en")
            {
                description = f.GetProperty("flavor_text").GetString()!.Replace("\n", " ").Replace("\f", " ");
                break;
            }
        }
        
        // Gender ratio - proporción de género
        var genderRate = s.GetProperty("gender_rate").GetInt32();
        var genderRatio = genderRate switch
        {
            -1 => "Genderless",
            0 => "0% female, 100% male",
            1 => "12.5% female, 87.5% male",
            2 => "25% female, 75% male",
            3 => "37.5% female, 62.5% male",
            4 => "50% female, 50% male",
            5 => "62.5% female, 37.5% male",
            6 => "75% female, 25% male",
            7 => "87.5% female, 12.5% male",
            8 => "100% female, 0% male",
            _ => "Unknown"
        };
        
        // Egg groups - grupos de huevo
        var eggGroups = s.GetProperty("egg_groups").EnumerateArray()
            .Select(e => Capitalize(e.GetProperty("name").GetString()!))
            .ToList();
        
        // Shape - forma del pokemon
        var shape = "";
        if (s.TryGetProperty("shape", out var shp) && shp.ValueKind == JsonValueKind.Object)
            shape = Capitalize(shp.GetProperty("name").GetString()!);
        
        // Capture rate y base happiness
        int? captureRate = null;
        if (s.TryGetProperty("capture_rate", out var cr) && cr.ValueKind == JsonValueKind.Number)
            captureRate = cr.GetInt32();
        
        int? baseHappiness = null;
        if (s.TryGetProperty("base_happiness", out var bh) && bh.ValueKind == JsonValueKind.Number)
            baseHappiness = bh.GetInt32();
        
        // Legendary y Mythical
        var isLegendary = s.GetProperty("is_legendary").GetBoolean();
        var isMythical = s.GetProperty("is_mythical").GetBoolean();
        
        // Varieties - otras formas de la especie
        var varieties = s.GetProperty("varieties").EnumerateArray()
            .Select(v => Capitalize(v.GetProperty("pokemon").GetProperty("name").GetString()!))
            .ToList();
        
        // ============ IMÁGENES ============
        var sprites = p.GetProperty("sprites");
        var sprite = sprites.TryGetProperty("front_default", out var sd) && sd.ValueKind == JsonValueKind.String ? sd.GetString()! : "";
        
        var thumbnail = "";
        var hires = "";
        if (sprites.TryGetProperty("other", out var other) && other.ValueKind == JsonValueKind.Object)
        {
            if (other.TryGetProperty("official-artwork", out var oa) && oa.ValueKind == JsonValueKind.Object)
                thumbnail = oa.TryGetProperty("front_default", out var t) && t.ValueKind == JsonValueKind.String ? t.GetString()! : "";
            if (other.TryGetProperty("home", out var hm) && hm.ValueKind == JsonValueKind.Object)
                hires = hm.TryGetProperty("front_default", out var h) && h.ValueKind == JsonValueKind.String ? h.GetString()! : "";
        }
        
        // ============ OTROS DATOS ============
        // Generation
        var generation = "";
        if (s.TryGetProperty("generation", out var gen) && gen.ValueKind == JsonValueKind.Object)
            generation = Capitalize(gen.GetProperty("name").GetString()!.Replace("generation-", "Gen "));
        
        // Habitat
        var habitat = "";
        if (s.TryGetProperty("habitat", out var hab) && hab.ValueKind == JsonValueKind.Object)
            habitat = Capitalize(hab.GetProperty("name").GetString()!);
        
        // Color
        var color = "";
        if (s.TryGetProperty("color", out var col) && col.ValueKind == JsonValueKind.Object)
            color = Capitalize(col.GetProperty("name").GetString()!);
        
        // ============ MOVIMIENTOS ============
        // Tomamos solo los primeros 10 movimientos para no saturar el JSON
        // NOTA: Obtener detalles completos de cada movimiento requeriria una llamada API adicional por movimiento
        var moves = p.GetProperty("moves").EnumerateArray()
            .Take(10)
            .Select(m => new MoveInfo(Capitalize(m.GetProperty("move").GetProperty("name").GetString()!), null, null, null, null, null, null))
            .ToList();
        
        // ============ CREAR EL POKEMON ============
        pokemons.Add(new Pokemon(
            Id: id,
            Name: name,
            DisplayName: name,
            Type: types,
            Base: stats,
            Species: speciesName,
            Genus: genus,
            Category: description,
            Description: description,
            NextEvolution: null,      // No extraído para evitar llamadas adicionales
            PrevEvolution: null,      // No extraído para evitar llamadas adicionales
            Height: height,
            Weight: weight,
            EggGroups: eggGroups,
            Abilities: abilities,
            GenderRatio: genderRatio,
            Sprite: sprite,
            Thumbnail: thumbnail,
            Hires: hires,
            Cry: "",                  // URL del grito no guardada
            Generation: generation,
            Habitat: habitat,
            Color: color,
            Shape: shape,
            CaptureRate: captureRate,
            BaseHappiness: baseHappiness,
            IsDefault: true,
            BaseExperience: baseExperience,
            Order: id,
            IsLegendary: isLegendary,
            IsMythical: isMythical,
            Varieties: varieties,
            Moves: moves,
            TotalStats: totalStats
        ));
        
        Console.WriteLine($"OK - {name}");
        
        // Pequeña pausa para no saturar la API
        await Task.Delay(30);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}

// Guardar en JSON con formato legible
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
var json = JsonSerializer.Serialize(pokemons, jsonOptions);
Directory.CreateDirectory("data");
await File.WriteAllTextAsync("data/pokedex.json", json);

Console.WriteLine($"\n¡Completado! Guardados {pokemons.Count} pokemons en data/pokedex.json");

// Función auxiliar para capitalizar texto
// Convierte "grass" -> "Grass", "fire-red" -> "Fire Red"
static string Capitalize(string text)
{
    if (string.IsNullOrEmpty(text)) return text;
    return char.ToUpper(text[0]) + text[1..].Replace("-", " ");
}
