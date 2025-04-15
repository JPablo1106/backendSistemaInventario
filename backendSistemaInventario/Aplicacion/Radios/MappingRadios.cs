using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;

namespace backendSistemaInventario.Aplicacion.Radios
{
    public class MappingRadios : Profile
    {
        public MappingRadios()
        {
            CreateMap<Radio, RadioDTO>();
        }
    }
}
