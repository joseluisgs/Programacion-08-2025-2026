using Pokedex.Dto;
using Pokedex.Models;

namespace Pokedex.Mappers;

public static class PokemonMapper 
{
    public static Pokemon ToModel(this PokemonDto dto) 
    {
        return new Pokemon(
            dto.Id,
            dto.Name,
            dto.DisplayName,
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
            dto.Genus,
            dto.Category,
            dto.Description,
            dto.NextEvolution?.Select(e => new Evolution(e.Id, e.Name, e.Condition)).ToList(),
            dto.PrevEvolution?.Select(e => new Evolution(e.Id, e.Name, e.Condition)).ToList(),
            dto.Height,
            dto.Weight,
            dto.EggGroups,
            dto.Abilities.Select(a => new Ability(a.Name, a.IsHidden, a.Description)).ToList(),
            dto.GenderRatio,
            dto.Sprite,
            dto.Thumbnail,
            dto.Hires,
            dto.Cry,
            dto.Generation,
            dto.Habitat,
            dto.Color,
            dto.Shape,
            dto.CaptureRate,
            dto.BaseHappiness,
            dto.IsDefault,
            dto.BaseExperience,
            dto.Order,
            dto.IsLegendary,
            dto.IsMythical,
            dto.Varieties,
            dto.Moves.Select(m => new MoveInfo(m.Name, m.Description, m.Power, m.Accuracy, m.PP, m.Type, m.DamageClass)).ToList(),
            dto.TotalStats
        );
    }
}
