using AutoMapper;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.DTOS;

public class EquiposSeguridadProfile : Profile
{
    public EquiposSeguridadProfile()
    {
        CreateMap<EquipoSeguridad, EquipoSeguridadDTO>();
    }
}
