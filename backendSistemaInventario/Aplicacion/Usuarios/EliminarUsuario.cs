using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Usuarios
{
    public class EliminarUsuario
    {
        public class EjecutarEliminarUsuario : IRequest
        {
            public int idUsuario { get; set; }
        }

    public  class EjecutaValidacion : AbstractValidator<EjecutarEliminarUsuario>
        {
            public EjecutaValidacion()
            {
                RuleFor(u => u.idUsuario).NotEmpty().WithMessage("El ID del usuario es requerido").GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a 0");
            }
        }

        public class Manejador : IRequestHandler<EjecutarEliminarUsuario>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEliminarUsuario request, CancellationToken cancellationToken)
            {
                var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.idUsuario == request.idUsuario, cancellationToken);

                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                _context.usuarios.Remove(usuario);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el usaurio");
            }
        }
    }
}
