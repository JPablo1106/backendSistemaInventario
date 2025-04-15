using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Celulares
{
    public class EliminarCelular
    {
        public class EjecutarEliminarCelular : IRequest
        {
            public int idCelular { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarEliminarCelular>
        {
            public EjecutaValidacion()
            {
                RuleFor(c => c.idCelular).NotEmpty().WithMessage("El ID del celular es requerido").GreaterThan(0);
            }
        }

        public class Manejador : IRequestHandler<EjecutarEliminarCelular>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit>Handle(EjecutarEliminarCelular request, CancellationToken cancellationToken)
            {
                var celular = await _context.celulares.FirstOrDefaultAsync(c => c.idCelular == request.idCelular);

                if (celular == null)
                {
                    throw new Exception("El celular no existe");
                }

                _context.celulares.Remove(celular);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el celular");
            }
        }
    }
}
