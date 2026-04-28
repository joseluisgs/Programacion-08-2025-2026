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
    bool IsHidden
);

public sealed record Pokemon(
    int Id,
    string Name,
    List<string> Type,
    BaseStats Base,
    string Species,
    string Description,
    List<Evolution>? NextEvolution,
    List<Evolution>? PrevEvolution,
    double Height,
    double Weight,
    List<string> Egg,
    List<Ability> Abilities,
    string GenderRatio,
    string Sprite,
    string Thumbnail,
    string Hires
);