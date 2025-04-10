using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Usuarios
{
    public class ConsultaUsuarioPorId
    {
        // Definimos la consulta que recibe el ID del usuario.
        public class Ejecuta : IRequest<UsuariosDTO>
        {
            public int idUsuario { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuariosDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UsuariosDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Se obtiene el usuario cuyo id coincida con el idUsuario recibido
                var usuario = await _context.usuarios
                    .FirstOrDefaultAsync(u => u.idUsuario == request.idUsuario, cancellationToken);

                if (usuario == null)
                {
                    // Puedes personalizar el tipo de excepción o devolver un valor nulo dependiendo de tu lógica de negocio
                    throw new Exception("No se encontró el usuario");
                }

                // Mapeamos el objeto Usuario al DTO correspondiente y lo retornamos
                var usuarioDTO = _mapper.Map<Usuario, UsuariosDTO>(usuario);
                return usuarioDTO;
            }
        }
    }
}
