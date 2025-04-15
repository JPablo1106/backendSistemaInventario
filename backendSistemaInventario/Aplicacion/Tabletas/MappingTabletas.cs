using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;

namespace backendSistemaInventario.Aplicacion.Tabletas
{
    public class MappingTabletas :  Profile
    {
        public MappingTabletas()
        {
            CreateMap<Tableta, TabletaDTO>();
        }
    }
}
