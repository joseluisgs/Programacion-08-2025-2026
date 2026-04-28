using System.Text.Json.Serialization;

namespace Pokedex.Dto;

public sealed record PokemonNameDto(
    [property: JsonPropertyName("english")]
    string English
);

public sealed record BaseStatsDto(
    [property: JsonPropertyName("HP")] int HP,
    [property: JsonPropertyName("Attack")] int Attack,
    [property: JsonPropertyName("Defense")]
    int Defense,
    [property: JsonPropertyName("Sp. Attack")]
    int SpAttack,
    [property: JsonPropertyName("Sp. Defense")]
    int SpDefense,
    [property: JsonPropertyName("Speed")] int Speed
);

public sealed record EvolutionInfoDto(
    [property: JsonPropertyName("0")] string? Id,
    [property: JsonPropertyName("1")] string? Condition
);

public sealed record EvolutionDto(
    [property: JsonPropertyName("prev")] List<string>? Prev,
    [property: JsonPropertyName("next")] List<List<string>>? Next
);

public sealed record ProfileDto(
    [property: JsonPropertyName("height")] string Height,
    [property: JsonPropertyName("weight")] string Weight,
    [property: JsonPropertyName("egg")] List<string> Egg,
    [property: JsonPropertyName("ability")]
    List<List<string>> Ability,
    [property: JsonPropertyName("gender")] string Gender
);

public sealed record ImageDto(
    [property: JsonPropertyName("sprite")] string Sprite,
    [property: JsonPropertyName("thumbnail")]
    string Thumbnail,
    [property: JsonPropertyName("hires")] string Hires
);

public sealed record PokemonDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] PokemonNameDto Name,
    [property: JsonPropertyName("type")] List<string> Type,
    [property: JsonPropertyName("base")] BaseStatsDto Base,
    [property: JsonPropertyName("species")]
    string Species,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("evolution")]
    EvolutionDto? Evolution,
    [property: JsonPropertyName("profile")]
    ProfileDto Profile,
    [property: JsonPropertyName("image")] ImageDto Image
);