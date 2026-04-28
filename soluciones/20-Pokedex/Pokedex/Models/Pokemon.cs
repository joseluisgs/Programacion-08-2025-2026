namespace Pokedex.Models;

public sealed record Evolution(string Name, string Num);

public sealed record Pokemon(
    int Id,
    string Num,
    string Name,
    string Img,
    List<string> Type,
    double Height,
    double Weight,
    string Candy,
    int CandyCount,
    int Egg,
    double SpawnChance,
    double AvgSpawns,
    string SpawnTime,
    List<double>? Multipliers,
    List<string> Weaknesses,
    List<Evolution>? NextEvolution,
    List<Evolution>? PrevEvolution
);
