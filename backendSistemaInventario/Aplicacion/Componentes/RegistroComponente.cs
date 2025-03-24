using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Componentes
{
    public class RegistroComponente
    {
        public class EjecutarRegistroComponente : IRequest
        {
            public string tipoComponente { get; set; }
            public string marcaComponente { get; set; }
            public string? modeloMonitor { get; set; }
            public string? modeloTelefono { get; set; }
            public string? idiomaTeclado { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroComponente>
        {
            public EjecutaValidacion()
            {
                RuleFor(c => c.tipoComponente).NotEmpty();
                RuleFor(c => c.marcaComponente).NotEmpty();
                RuleFor(c => c.modeloMonitor).NotEmpty().When(c => c.tipoComponente == "Monitor")
                    .WithMessage("El modelo del monitor es obligatorio para el tipo Monitor");
                RuleFor(c => c.idiomaTeclado).NotEmpty().When(c => c.tipoComponente == "Teclado")
                    .WithMessage("El idioma es obligatorio solo para el tipo Teclado");
                RuleFor(c => c.modeloTelefono).NotEmpty().When(c => c.tipoComponente == "Teléfono IP")
                    .WithMessage("El idioma es obligatorio solo para el tipo Teclado");
            }
        }

        public class Manejador : IRequestHandler<EjecutarRegistroComponente>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroComponente request, CancellationToken cancellationToken)
            {
                if (request.tipoComponente == "Teclado" && (string.IsNullOrEmpty(request.idiomaTeclado) || request.idiomaTeclado == "string"))
                {
                    throw new Exception("El idioma del teclado es obligatorio y debe ser válido");
                }

                if ((request.tipoComponente == "Teléfono IP" || request.tipoComponente == "Telefono IP")
                    && (string.IsNullOrEmpty(request.modeloTelefono) || request.modeloTelefono == "string"))
                {
                    throw new Exception("El modelo del teléfono es obligatorio y debe ser válido");
                }


                Componente componente = request.tipoComponente switch
                {
                    "Monitor" => new Monitores { modeloMonitor = request.modeloMonitor },
                    "Teclado" => new Teclado { idiomaTeclado = request.idiomaTeclado },
                    "Telefono IP" or "Teléfeono IP" => new Telefono { modeloTelefono = request.modeloTelefono },
                    "Mouse" => new Mouse(),
                    _ => throw new Exception("Tipo de componente no válido")
                };

                componente.tipoComponente = request.tipoComponente;
                componente.marcaComponente = request.marcaComponente;

                _context.componentes.Add(componente);
                var respuesta = await _context.SaveChangesAsync();

                if (respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo registrar el componente");
            }

        }
    }
}
