using System.Text.Json.Serialization;

namespace Pokedex.Dto;

public sealed record EvolutionDto(
    [property: JsonPropertyName("num")] string Num,
    [property: JsonPropertyName("name")] string Name
);

public sealed record PokemonDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("num")] string Num,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("img")] string Img,
    [property: JsonPropertyName("type")] List<string> Type,
    [property: JsonPropertyName("height")] string Height,
    [property: JsonPropertyName("weight")] string Weight,
    [property: JsonPropertyName("candy")] string Candy,
    [property: JsonPropertyName("candy_count")] int? CandyCount,
    [property: JsonPropertyName("egg")] string Egg,
    [property: JsonPropertyName("spawn_chance")] double SpawnChance,
    [property: JsonPropertyName("avg_spawns")] double AvgSpawns,
    [property: JsonPropertyName("spawn_time")] string SpawnTime,
    [property: JsonPropertyName("multipliers")] List<double>? Multipliers,
    [property: JsonPropertyName("weaknesses")] List<string> Weaknesses,
    [property: JsonPropertyName("next_evolution")] List<EvolutionDto>? NextEvolution,
    [property: JsonPropertyName("prev_evolution")] List<EvolutionDto>? PrevEvolution
);

public sealed record PokedexDto(
    [property: JsonPropertyName("pokemon")] List<PokemonDto> Pokemons
);
