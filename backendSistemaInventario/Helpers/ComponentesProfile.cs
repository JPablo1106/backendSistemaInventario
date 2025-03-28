using AutoMapper;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.DTOS;

public class ComponentesProfile : Profile
{
    public ComponentesProfile()
    {
        CreateMap<Componente, ComponenteDTO>()
            .Include<Teclado, ComponenteDTO>()
            .Include<Monitores, ComponenteDTO>()
            .Include<Telefono, ComponenteDTO>()
            .Include<Mouse, ComponenteDTO>();

        CreateMap<Teclado, ComponenteDTO>();
        CreateMap<Monitores, ComponenteDTO>();
        CreateMap<Telefono, ComponenteDTO>();
        CreateMap<Mouse, ComponenteDTO>();
    }
}
