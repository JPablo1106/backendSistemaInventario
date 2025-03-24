using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Componentes
{
    public class ActualizarComponente
    {

        public class EjecutarActualizarComponente : IRequest
        {
            public int idComponente { get; set; }
            public string tipoComponente { get; set; }
            public string marcaComponente { get; set; }
            public string? modeloMonitor { get; set; }
            public string? idiomaTeclado { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarActualizarComponente>
        {
            public  EjecutaValidacion()
            {
                RuleFor(c => c.idComponente).NotEmpty().WithMessage("El id del componente es obligatorio");
                RuleFor(c => c.tipoComponente).NotEmpty();
                RuleFor(c => c.marcaComponente).NotEmpty();
                RuleFor(c => c.modeloMonitor).NotEmpty().When(c => c.tipoComponente == "Monitor")
                    .WithMessage("El modelo del monitor es obligatorio para el tipo Monitor");

                RuleFor(c => c.idiomaTeclado).NotEmpty().When(c => c.tipoComponente == "Teclado")
                    .WithMessage("El idioma es obligatorio para el tipo Teclado");

            }
        }

        public class Manejador : IRequestHandler<EjecutarActualizarComponente>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarActualizarComponente request, CancellationToken cancellationToken)
            {
                //Se busca el componente por el id
                var componente = await _context.componentes
                    .FirstOrDefaultAsync(c => c.idComponente == request.idComponente);

                if(componente == null)
                {
                    throw new Exception("No se encontró el componente");
                }

                //Actualizar propiedades comunes
                componente.tipoComponente = request.tipoComponente;
                componente.marcaComponente = request.marcaComponente;

                if(request.tipoComponente == "Monitor" && componente is Monitores monitor)
                {
                    monitor.modeloMonitor = request.modeloMonitor;
                }

                if (request.tipoComponente == "Teclado" && componente is Teclado teclado)
                {
                    teclado.idiomaTeclado = request.idiomaTeclado;
                }

                var respuesta = await _context.SaveChangesAsync();

                if(respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el componente");
            }
        }
    }
}
