using Pokedex.Dto;
using Pokedex.Models;

namespace Pokedex.Mappers;

public static class PokemonMapper
{
    public static Pokemon ToModel(this PokemonDto dto)
    {
        return new Pokemon(
            dto.Id,
            dto.Num,
            dto.Name,
            dto.Img,
            dto.Type,
            ParseHeight(dto.Height),
            ParseWeight(dto.Weight),
            dto.Candy,
            dto.CandyCount ?? 0,
            ParseEgg(dto.Egg),
            dto.SpawnChance,
            dto.AvgSpawns,
            dto.SpawnTime,
            dto.Multipliers,
            dto.Weaknesses,
            dto.NextEvolution?.Select(e => new Evolution(e.Name, e.Num)).ToList(),
            dto.PrevEvolution?.Select(e => new Evolution(e.Name, e.Num)).ToList()
        );
    }

    private static double ParseHeight(string height) =>
        double.Parse(height.Replace(" m", ""));

    private static double ParseWeight(string weight) =>
        double.Parse(weight.Replace(" kg", ""));

    private static int ParseEgg(string egg)
    {
        if (egg == "Not in Eggs" || egg == "Omanyte Candy") return 0;
        return int.Parse(egg.Replace(" km", ""));
    }
}
