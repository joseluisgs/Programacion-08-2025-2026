using DtoAvanzado.Dtos;
using DtoAvanzado.Models;

namespace DtoAvanzado.Mappers;

public static class AficionMapper {
    public static AficionDto ToDto(this Aficion aficion) {
        return new AficionDto(
            aficion.Nombre,
            aficion.Descripcion
        );
    }

    public static Aficion ToModel(this AficionDto aficionDto) {
        return new Aficion(
            aficionDto.Nombre,
            aficionDto.Descripcion
        );
    }
    
}