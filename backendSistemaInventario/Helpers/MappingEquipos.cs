using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;

namespace backendSistemaInventario.Helpers
{
    public class MappingEquipos : Profile
    {
        public MappingEquipos()
        {
            //Mapeo entre DiscoDuro y DiscoDuroDTO
            CreateMap<DiscoDuro, DiscoDuroDTO>();

            //Mapeo entre Equipo y EquipoDTO
            CreateMap<Equipo, EquipoDTO>().ForMember(dest => dest.discoDuro, opt => opt.MapFrom(src => src.discoDuro));
        }
    }
}
