using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Usuarios
{
    public class ActualizarUsuario
    {
        public class EjecutarActualizarUsuario : IRequest<UsuariosDTO>
        {
            public int idUsuario { get; set; } // Se busca por ID 
            public string nombreUsuario { get; set; }
            public string area { get; set; }
            public string departamento { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarActualizarUsuario>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.idUsuario).NotEmpty().WithMessage("El ID del usuario es requerido");
                RuleFor(p => p.nombreUsuario).NotEmpty().WithMessage("El nombre del usuario es requerido");
                RuleFor(p => p.area).NotEmpty().WithMessage("El área es requerida");
                RuleFor(p => p.departamento).NotEmpty().WithMessage("El departamento es requerido");
            }
        }

        public class Manejador : IRequestHandler<EjecutarActualizarUsuario, UsuariosDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UsuariosDTO> Handle(EjecutarActualizarUsuario request, CancellationToken cancellationToken)
            {
                // Buscamos el usuario por ID
                var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.idUsuario == request.idUsuario, cancellationToken);

                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                // Actualizamos los campos
                usuario.nombreUsuario = request.nombreUsuario;
                usuario.area = request.area;
                usuario.departamento = request.departamento;

                // Guardamos los cambios
                var resultado = await _context.SaveChangesAsync(cancellationToken);
                if (resultado > 0)
                {
                    return _mapper.Map<UsuariosDTO>(usuario);
                }
                throw new Exception("No se pudo actualizar el usuario");
            }
        }
    }

}
