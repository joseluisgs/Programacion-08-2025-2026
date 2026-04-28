// =============================================================================
// SCRIPT DE DESCARGA COMPLETA - TODOS LOS DATOS DESDE pokeapi.co
// =============================================================================
// Este script descarga TODOS los Pokemon con la MÁXIMA información posible.
// Realiza llamadas API adicionales para obtener:
// - Descripciones de habilidades
// - Detalles de movimientos (power, accuracy, pp, type, damage class)
// - Datos de evoluciones
// - URLs de cries
//
// NOTA: Este script tarda ~1-2 horas en ejecutarse (1025 pokemons × varias llamadas cada uno)
//
// INSTRUCCIONES:
// 1. Ejecutar: dotnet run (cuando esté configurado en Pokedex.csproj)
// 2. El resultado se guarda en data/pokedex-full.json
// 3. Para usar este JSON, cambiar el path en PokedexJsonStorage.cs
// =============================================================================

using System.Net.Http;
using System.Text.Json;
using Pokedex.Models;

var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
var pokemons = new List<Pokemon>();

Console.WriteLine("==========================================");
Console.WriteLine("  DESCARGA COMPLETA - TODOS LOS DATOS");
Console.WriteLine("  ESTO TARDARÁ VARIAS HORAS");
Console.WriteLine("==========================================");

int total = 1025;
int descargados = 0;
int errores = 0;

for (int i = 1; i <= total; i++)
{
    try
    {
        Console.Write($"\rDescargando {i}/{total}... (OK:{descargados} ERR:{errores}) ");
        
        // Datos básicos del pokemon
        var pokemonJson = await httpClient.GetStringAsync($"pokemon/{i}");
        var speciesJson = await httpClient.GetStringAsync($"pokemon-species/{i}");
        
        using var pokemonDoc = JsonDocument.Parse(pokemonJson);
        using var speciesDoc = JsonDocument.Parse(speciesJson);
        
        var p = pokemonDoc.RootElement;
        var s = speciesDoc.RootElement;
        
        // ============ DATOS BÁSICOS ============
        var id = p.GetProperty("id").GetInt32();
        var name = Capitalize(p.GetProperty("name").GetString()!);
        var height = p.GetProperty("height").GetInt32() / 10.0;
        var weight = p.GetProperty("weight").GetInt32() / 10.0;
        
        int baseExperience = 0;
        if (p.TryGetProperty("base_experience", out var be) && be.ValueKind == JsonValueKind.Number)
            baseExperience = be.GetInt32();
        
        // ============ CRY URL ============
        var cryUrl = "";
        try {
            if (p.TryGetProperty("cries", out var cries) && cries.ValueKind == JsonValueKind.Object)
            {
                if (cries.TryGetProperty("latest", out var latest) && latest.ValueKind == JsonValueKind.String)
                    cryUrl = latest.GetString() ?? "";
            }
        } catch { }
        
        // ============ TIPOS ============
        var types = p.GetProperty("types").EnumerateArray()
            .OrderBy(t => t.GetProperty("slot").GetInt32())
            .Select(t => Capitalize(t.GetProperty("type").GetProperty("name").GetString()!))
            .ToList();
        
        // ============ BASE STATS ============
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
        var totalStats = hp + attack + defense + spAttack + spDefense + speed;
        
        // ============ HABILIDADES (CON DESCRIPCIONES) ============
        var abilities = new List<Ability>();
        foreach (var a in p.GetProperty("abilities").EnumerateArray())
        {
            var abilityName = Capitalize(a.GetProperty("ability").GetProperty("name").GetString()!);
            var isHidden = a.GetProperty("is_hidden").GetBoolean();
            
            // Obtener descripción de la habilidad
            string? abilityDesc = null;
            try {
                var abilityUrl = a.GetProperty("ability").GetProperty("url").GetString();
                if (!string.IsNullOrEmpty(abilityUrl))
                {
                    var abilityJson = await httpClient.GetStringAsync(abilityUrl.Replace("https://pokeapi.co/api/v2/", ""));
                    using var abilityDoc = JsonDocument.Parse(abilityJson);
                    var abilityRoot = abilityDoc.RootElement;
                    
                    foreach (var eff in abilityRoot.GetProperty("effect_entries").EnumerateArray())
                    {
                        if (eff.TryGetProperty("language", out var lang) && lang.ValueKind == JsonValueKind.Object)
                        {
                            if (lang.GetProperty("name").GetString() == "en")
                            {
                                abilityDesc = eff.GetProperty("short_effect").GetString();
                                break;
                            }
                        }
                    }
                }
            } catch { }
            
            abilities.Add(new Ability(abilityName, isHidden, abilityDesc));
        }
        
        // ============ DATOS DE LA ESPECIE ============
        var speciesName = Capitalize(s.GetProperty("name").GetString()!);
        
        // Genus
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
        
        // Description
        var description = "";
        foreach (var f in s.GetProperty("flavor_text_entries").EnumerateArray())
        {
            if (f.GetProperty("language").GetProperty("name").GetString() == "en")
            {
                description = f.GetProperty("flavor_text").GetString()!.Replace("\n", " ").Replace("\f", " ");
                break;
            }
        }
        
        // Gender ratio
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
        
        // Egg groups
        var eggGroups = s.GetProperty("egg_groups").EnumerateArray()
            .Select(e => Capitalize(e.GetProperty("name").GetString()!))
            .ToList();
        
        // Shape
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
        
        var isLegendary = s.GetProperty("is_legendary").GetBoolean();
        var isMythical = s.GetProperty("is_mythical").GetBoolean();
        
        // Varieties
        var varieties = s.GetProperty("varieties").EnumerateArray()
            .Select(v => Capitalize(v.GetProperty("pokemon").GetProperty("name").GetString()!))
            .ToList();
        
        // ============ EVOLUCIONES ============
        List<Evolution>? nextEvolution = null;
        List<Evolution>? prevEvolution = null;
        
        try {
            // Obtener cadena de evolución
            if (s.TryGetProperty("evolution_chain", out var evoChain) && evoChain.ValueKind == JsonValueKind.Object)
            {
                var evoChainUrl = evoChain.GetProperty("url").GetString();
                if (!string.IsNullOrEmpty(evoChainUrl))
                {
                    var evoJson = await httpClient.GetStringAsync(evoChainUrl.Replace("https://pokeapi.co/api/v2/", ""));
                    using var evoDoc = JsonDocument.Parse(evoJson);
                    var evoRoot = evoDoc.RootElement;
                    
                    // Recorrer la cadena de evoluciones
                    var chain = evoRoot.GetProperty("chain");
                    var evolutions = new List<(int? id, string? name, string? condition)>();
                    
                    void ParseEvoChain(JsonElement element, string? fromName, string? condition)
                    {
                        var species = element.GetProperty("species");
                        var speciesNameEvo = species.GetProperty("name").GetString();
                        var speciesUrl = species.GetProperty("url").GetString();
                        var evoId = 0;
                        if (!string.IsNullOrEmpty(speciesUrl))
                        {
                            var parts = speciesUrl.TrimEnd('/').Split('/');
                            if (int.TryParse(parts.Last(), out var parsedId))
                                evoId = parsedId;
                        }
                        
                        evolutions.Add((evoId, Capitalize(speciesNameEvo!), condition));
                        
                        foreach (var evoDetail in element.GetProperty("evolves_to").EnumerateArray())
                        {
                            var evoCondition = "";
                            if (evoDetail.TryGetProperty("evolution_details", out var details) && details.EnumerateArray().Any())
                            {
                                var detail = details.EnumerateArray().First();
                                if (detail.TryGetProperty("trigger", out var trigger))
                                    evoCondition = trigger.GetProperty("name").GetString() ?? "";
                                // Añadir más condiciones si están disponibles
                            }
                            ParseEvoChain(evoDetail, speciesNameEvo, evoCondition);
                        }
                    }
                    
                    ParseEvoChain(chain, null, null);
                    
                    // Determinar prev y next evolution para este pokemon
                    var currentIndex = evolutions.FindIndex(e => e.name == name);
                    if (currentIndex >= 0)
                    {
                        if (currentIndex > 0)
                        {
                            prevEvolution = new List<Evolution> {
                                new Evolution(evolutions[currentIndex - 1].id, evolutions[currentIndex - 1].name, evolutions[currentIndex - 1].condition)
                            };
                        }
                        if (currentIndex < evolutions.Count - 1)
                        {
                            nextEvolution = new List<Evolution> {
                                new Evolution(evolutions[currentIndex + 1].id, evolutions[currentIndex + 1].name, evolutions[currentIndex + 1].condition)
                            };
                        }
                    }
                }
            }
        } catch { }
        
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
        var generation = "";
        if (s.TryGetProperty("generation", out var gen) && gen.ValueKind == JsonValueKind.Object)
            generation = Capitalize(gen.GetProperty("name").GetString()!.Replace("generation-", "Gen "));
        
        var habitat = "";
        if (s.TryGetProperty("habitat", out var hab) && hab.ValueKind == JsonValueKind.Object)
            habitat = Capitalize(hab.GetProperty("name").GetString()!);
        
        var color = "";
        if (s.TryGetProperty("color", out var col) && col.ValueKind == JsonValueKind.Object)
            color = Capitalize(col.GetProperty("name").GetString()!);
        
        // ============ MOVIMIENTOS (CON DETALLES) ============
        var moves = new List<MoveInfo>();
        var moveCount = 0;
        foreach (var moveElement in p.GetProperty("moves").EnumerateArray())
        {
            if (moveCount >= 15) break; // Limitar a 15 movimientos por pokemon
            
            var moveName = Capitalize(moveElement.GetProperty("move").GetProperty("name").GetString()!);
            
            // Obtener detalles del movimiento
            string? desc = null;
            int? power = null;
            int? accuracy = null;
            int? pp = null;
            string? moveType = null;
            string? damageClass = null;
            
            try {
                var moveUrl = moveElement.GetProperty("move").GetProperty("url").GetString();
                if (!string.IsNullOrEmpty(moveUrl))
                {
                    var moveJson = await httpClient.GetStringAsync(moveUrl.Replace("https://pokeapi.co/api/v2/", ""));
                    using var moveDoc = JsonDocument.Parse(moveJson);
                    var moveRoot = moveDoc.RootElement;
                    
                    // Power
                    if (moveRoot.TryGetProperty("power", out var pwr) && pwr.ValueKind == JsonValueKind.Number)
                        power = pwr.GetInt32();
                    
                    // Accuracy
                    if (moveRoot.TryGetProperty("accuracy", out var acc) && acc.ValueKind == JsonValueKind.Number)
                        accuracy = acc.GetInt32();
                    
                    // PP
                    if (moveRoot.TryGetProperty("pp", out var ppVal) && ppVal.ValueKind == JsonValueKind.Number)
                        pp = ppVal.GetInt32();
                    
                    // Type
                    if (moveRoot.TryGetProperty("type", out var mt) && mt.ValueKind == JsonValueKind.Object)
                        moveType = Capitalize(mt.GetProperty("name").GetString()!);
                    
                    // Damage class
                    if (moveRoot.TryGetProperty("damage_class", out var dc) && dc.ValueKind == JsonValueKind.Object)
                        damageClass = Capitalize(dc.GetProperty("name").GetString()!);
                    
                    // Description
                    foreach (var ft in moveRoot.GetProperty("flavor_text_entries").EnumerateArray())
                    {
                        if (ft.TryGetProperty("language", out var lang) && lang.ValueKind == JsonValueKind.Object)
                        {
                            if (lang.GetProperty("name").GetString() == "en")
                            {
                                desc = ft.GetProperty("flavor_text").GetString();
                                break;
                            }
                        }
                    }
                }
            } catch { }
            
            moves.Add(new MoveInfo(moveName, desc, power, accuracy, pp, moveType, damageClass));
            moveCount++;
            
            // Pequeña pausa para no saturar la API
            await Task.Delay(100);
        }
        
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
            NextEvolution: nextEvolution,
            PrevEvolution: prevEvolution,
            Height: height,
            Weight: weight,
            EggGroups: eggGroups,
            Abilities: abilities,
            GenderRatio: genderRatio,
            Sprite: sprite,
            Thumbnail: thumbnail,
            Hires: hires,
            Cry: cryUrl,
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
        
        descargados++;
        
        // Pausa entre pokemon
        await Task.Delay(150);
    }
    catch (Exception ex)
    {
        errores++;
        Console.WriteLine($"\nERROR en pokemon {i}: {ex.Message}");
    }
}

// Guardar en JSON
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
var json = JsonSerializer.Serialize(pokemons, jsonOptions);
Directory.CreateDirectory("data");
await File.WriteAllTextAsync("data/pokedex-full.json", json);

Console.WriteLine($"\n\n==========================================");
Console.WriteLine($"¡COMPLETADO!");
Console.WriteLine($"  Pokemons descargados: {descargados}");
Console.WriteLine($"  Errores: {errores}");
Console.WriteLine($"  Guardado en: data/pokedex-full.json");
Console.WriteLine($"==========================================");

static string Capitalize(string text)
{
    if (string.IsNullOrEmpty(text)) return text;
    return char.ToUpper(text[0]) + text[1..].Replace("-", " ");
}
