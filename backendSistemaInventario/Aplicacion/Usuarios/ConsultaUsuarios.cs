using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Usuarios
{
    public class ConsultaUsuarios
    {
        public class ListaUsuarios : IRequest<List<UsuariosDTO>>
        {

        }

        public class Manejador : IRequestHandler<ListaUsuarios, List<UsuariosDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UsuariosDTO>> Handle(ListaUsuarios request, CancellationToken cancellationToken)
            {
                var usuarios = await _context.usuarios.ToListAsync();
                var usuariosDTO = _mapper.Map<List<Usuario>, List<UsuariosDTO>>(usuarios);
                return usuariosDTO;
            }
        }
    }
}
