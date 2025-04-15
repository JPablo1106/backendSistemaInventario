using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Tabletas
{
    public class RegistroTableta
    {
        // DTO que agrupa la lista de tabletas a registrar para un usuario.
        public class EjecutarRegistroTabletas : IRequest
        {
            public int idUsuario { get; set; }
            // Lista de tabletas a registrar. Cada elemento puede tener las mismas características excepto el numSerie.
            public List<RegistroTabletaDto> Tabletas { get; set; } = new List<RegistroTabletaDto>();
        }

        // DTO individual para cada tableta.
        public class RegistroTabletaDto
        {
            public string marca { get; set; }
            public string modelo { get; set; }
            public string numSerie { get; set; }
            public string accesorios { get; set; }
        }

        // Validador para el DTO que contiene la lista completa.
        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroTabletas>
        {
            public EjecutaValidacion()
            {
                RuleFor(t => t.idUsuario).GreaterThan(0);
                // Para cada item en la lista se ejecuta la validación definida en TabletaValidator.
                RuleForEach(x => x.Tabletas).SetValidator(new TabletaValidator());
            }
        }

        // Validador para el registro individual de cada tableta.
        public class TabletaValidator : AbstractValidator<RegistroTabletaDto>
        {
            public TabletaValidator()
            {
                RuleFor(t => t.marca).NotEmpty();
                RuleFor(t => t.modelo).NotEmpty();
                RuleFor(t => t.numSerie).NotEmpty();
                RuleFor(t => t.accesorios).NotEmpty();
            }
        }

        // Manejador que procesa la solicitud para registrar múltiples tabletas.
        public class Manejador : IRequestHandler<EjecutarRegistroTabletas>
        {
            public readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroTabletas request, CancellationToken cancellationToken)
            {
                // Verificar que el usuario exista.
                var usuario = await _context.usuarios.FindAsync(request.idUsuario, cancellationToken);
                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                // Se recorre la lista de tabletas y se crea una entidad Tableta para cada item.
                foreach (var tabletaDto in request.Tabletas)
                {
                    var tableta = new Tableta
                    {
                        marca = tabletaDto.marca,
                        modelo = tabletaDto.modelo,
                        numSerie = tabletaDto.numSerie,
                        accesorios = tabletaDto.accesorios,
                        idUsuario = request.idUsuario,
                    };
                    _context.tabletas.Add(tableta);
                }

                // Se guarda el registro en la base de datos.
                var respuesta = await _context.SaveChangesAsync(cancellationToken);
                if (respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron registrar las tabletas");
            }
        }
    }
}
