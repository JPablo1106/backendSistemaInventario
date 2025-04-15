using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Radios
{
    public class EliminarRadio
    {
        public class EjecutarEliminarRadio : IRequest
        {
            public int idRadio { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarEliminarRadio>
        {
            public EjecutaValidacion()
            {
                RuleFor(r => r.idRadio).NotEmpty().WithMessage("El ID del radio es requerido.").GreaterThan(0);
            }
        }

        public class Manejador : IRequestHandler<EjecutarEliminarRadio>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEliminarRadio request, CancellationToken cancellationToken)
            {
                var radio = await _context.radios.FirstOrDefaultAsync
                    (r => r.idRadio == request.idRadio);
                if (radio == null)
                {
                    throw new Exception("El radio no existe");
                }

                _context.radios.Remove(radio);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el radio");
           }
        }
    }
}
