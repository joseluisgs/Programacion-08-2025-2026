using Pokedex.Dto;
using Pokedex.Models;

namespace Pokedex.Mappers;

public static class PokemonMapper {
    public static Pokemon ToModel(this PokemonDto dto) {
        return new Pokemon(
            dto.Id,
            dto.Name.English,
            dto.Type,
            new BaseStats(
                dto.Base.HP,
                dto.Base.Attack,
                dto.Base.Defense,
                dto.Base.SpAttack,
                dto.Base.SpDefense,
                dto.Base.Speed
            ),
            dto.Species,
            dto.Description,
            ParseNextEvolution(dto.Evolution?.Next),
            ParsePrevEvolution(dto.Evolution?.Prev),
            ParseHeight(dto.Profile.Height),
            ParseWeight(dto.Profile.Weight),
            dto.Profile.Egg,
            ParseAbilities(dto.Profile.Ability),
            dto.Profile.Gender,
            dto.Image.Sprite,
            dto.Image.Thumbnail,
            dto.Image.Hires
        );
    }

    private static List<Evolution>? ParseNextEvolution(List<List<string>>? next) {
        return next?.Select(n => new Evolution(
            int.TryParse(n[0], out var id) ? id : null,
            null,
            n.Count > 1 ? n[1] : null
        )).ToList();
    }

    private static List<Evolution>? ParsePrevEvolution(List<string>? prev) {
        if (prev == null || prev.Count == 0) return null;
        return [
            new Evolution(
                int.TryParse(prev[0], out var id) ? id : null,
                null,
                prev.Count > 1 ? prev[1] : null
            )
        ];
    }

    private static double ParseHeight(string height) {
        return double.Parse(height.Replace(" m", ""));
    }

    private static double ParseWeight(string weight) {
        return double.Parse(weight.Replace(" kg", ""));
    }

    private static List<Ability> ParseAbilities(List<List<string>> abilities) {
        return abilities.Select(a => new Ability(
            a[0],
            bool.TryParse(a[1], out var isHidden) && isHidden
        )).ToList();
    }
}