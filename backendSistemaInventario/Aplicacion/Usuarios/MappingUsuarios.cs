using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;

namespace backendSistemaInventario.Aplicacion.Usuarios
{
    public class MappingUsuarios : Profile
    {
        public MappingUsuarios()
        {
            CreateMap<Usuario, UsuariosDTO>();
        }

    }
}
