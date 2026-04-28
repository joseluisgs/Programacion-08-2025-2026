namespace Pokedex.Models;

public sealed record Evolution(
    int? Id,
    string? Name,
    string? Condition
);

public sealed record BaseStats(
    int HP,
    int Attack,
    int Defense,
    int SpAttack,
    int SpDefense,
    int Speed
);

public sealed record Ability(
    string Name,
    bool IsHidden,
    string? Description
);

public sealed record TypeInfo(
    string Name,
    int Slot
);

public sealed record MoveInfo(
    string Name,
    string? Description,
    int? Power,
    int? Accuracy,
    int? PP,
    string? Type,
    string? DamageClass
);

public sealed record Pokemon(
    int Id,
    string Name,
    string DisplayName,
    List<string> Type,
    BaseStats Base,
    string Species,
    string Genus,
    string Category,
    string Description,
    List<Evolution>? NextEvolution,
    List<Evolution>? PrevEvolution,
    double Height,
    double Weight,
    List<string> EggGroups,
    List<Ability> Abilities,
    string GenderRatio,
    string Sprite,
    string Thumbnail,
    string Hires,
    string? Cry,
    string Generation,
    string? Habitat,
    string? Color,
    string? Shape,
    int? CaptureRate,
    int? BaseHappiness,
    bool IsDefault,
    int BaseExperience,
    int Order,
    bool IsLegendary,
    bool IsMythical,
    List<string> Varieties,
    List<MoveInfo> Moves,
    double? TotalStats
);
