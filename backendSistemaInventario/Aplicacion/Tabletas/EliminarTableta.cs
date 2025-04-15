using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Tabletas
{
    public class EliminarTableta
    {
        public class EjecutarEliminarTableta : IRequest
        {
            public int idTableta { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarEliminarTableta>
        {
            public EjecutaValidacion()
            {
                RuleFor(t => t.idTableta).NotEmpty().WithMessage("El ID de la tableta es requerido.").GreaterThan(0);

            }
        }

        public class Manejador : IRequestHandler<EjecutarEliminarTableta>
        {
            private readonly ContextoBD _context;
            
            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEliminarTableta request, CancellationToken cancellationToken)
            {
                var tableta = await _context.tabletas.FirstOrDefaultAsync
                    (t => t.idTableta == request.idTableta);
                if (tableta == null)
                {
                    throw new Exception("La tableta no existe");
                }

                _context.tabletas.Remove(tableta);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar la tableta");
            }
        }
    }
}
