using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Componentes
{
    public class EliminarComponente
    {
        // Solicitud
        public class EjecutarEliminarComponente : IRequest
        {
            public int idComponente { get; set; }
        }

        // Validación
        public class EjecutaValidacion : AbstractValidator<EjecutarEliminarComponente>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.idComponente).NotEmpty().WithMessage("El ID del componente es obligatorio.");
            }
        }

        // Manejador
        public class Manejador : IRequestHandler<EjecutarEliminarComponente>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEliminarComponente request, CancellationToken cancellationToken)
            {
                var componente = await _context.componentes
                    .FirstOrDefaultAsync(c => c.idComponente == request.idComponente, cancellationToken);

                if (componente == null)
                {
                    throw new Exception("No se encontró el componente con ese ID.");
                }

                _context.componentes.Remove(componente);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el componente.");
            }
        }
    }
}
