using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Asignaciones
{
    public class ActualizarAsignacion
    {
        // Renombramos las clases internas para evitar conflicto con RegistroAsignacion
        public class ComponenteAsignadoEditar
        {
            public int idComponente { get; set; }
            public string numSerieComponente { get; set; }
        }

        public class DispositivoExtAsignadoEditar
        {
            public int? idDispExt { get; set; }
            public string marca { get; set; }
            public string descripcion { get; set; }
            public string numSerieDispExt { get; set; }
        }

        // Comando para editar la asignación. Se incluye el idAsignacion para identificar el registro a modificar.
        public class EjecutarEditarAsignacion : IRequest
        {
            public int idAsignacion { get; set; }
            public int idUsuario { get; set; }
            public int? idEquipo { get; set; }
            public string? numSerieEquipo { get; set; }
            public DateTime fechaAsignacion { get; set; }
            public string? ipAddress { get; set; }
            public string? ipCpuRed { get; set; }
            public List<ComponenteAsignadoEditar>? componentes { get; set; }
            public List<DispositivoExtAsignadoEditar>? dispositivosExt { get; set; }
            public int? idEquipoSeguridad { get; set; }
            public string? numSerieEquipoSeg { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarEditarAsignacion>
        {
            public EjecutaValidacion()
            {
                RuleFor(a => a.idAsignacion)
                    .GreaterThan(0).WithMessage("El ID de la asignación debe ser válido");

                RuleFor(a => a.fechaAsignacion)
                    .NotEmpty().WithMessage("La fecha de asignación es obligatoria");

                RuleFor(a => a.idUsuario)
                    .GreaterThan(0).WithMessage("El ID del usuario debe ser válido");

                // Se requiere que se asigne al menos un elemento: equipo, componente o equipo de seguridad.
                RuleFor(a => a)
                    .Must(a =>
                    {
                        bool tieneEquipo = a.idEquipo.HasValue && a.idEquipo.Value > 0;
                        bool tieneComponentes = a.componentes != null && a.componentes.Any();
                        bool tieneEquipoSeg = a.idEquipoSeguridad.HasValue && a.idEquipoSeguridad.Value > 0;
                        return tieneEquipo || tieneComponentes || tieneEquipoSeg;
                    })
                    .WithMessage("Debe asignarse al menos a un elemento.");

                // Validaciones para asignación de equipo.
                When(a => a.idEquipo.HasValue && a.idEquipo.Value > 0, () =>
                {
                    RuleFor(a => a.numSerieEquipo)
                        .NotEmpty().WithMessage("El número de serie del equipo es obligatorio");

                    RuleFor(a => a.ipAddress)
                        .NotEmpty().WithMessage("La dirección IP es obligatoria para equipos");

                    RuleFor(a => a.ipCpuRed)
                        .NotEmpty().WithMessage("La IP de la CPU en red es obligatoria para equipos");
                });

                When(a => a.idEquipoSeguridad.HasValue && a.idEquipoSeguridad.Value > 0, () =>
                {
                    RuleFor(a => a.numSerieEquipoSeg)
                        .NotEmpty().WithMessage("El número de serie del equipo de seguridad es obligatorio");
                });

                // Validaciones para componentes.
                RuleForEach(a => a.componentes).ChildRules(componente =>
                {
                    componente.RuleFor(c => c.idComponente).GreaterThan(0).WithMessage("El ID del componente debe ser válido");
                    componente.RuleFor(c => c.numSerieComponente).NotEmpty().WithMessage("El número de serie del componente es obligatorio");
                });

                // Validaciones para dispositivos externos.
                RuleForEach(a => a.dispositivosExt).ChildRules(dispositivo =>
                {
                    dispositivo.RuleFor(d => d.marca).NotEmpty().WithMessage("La marca del dispositivo es obligatoria");
                    dispositivo.RuleFor(d => d.descripcion).NotEmpty().WithMessage("La descripción del dispositivo es obligatoria");
                    dispositivo.RuleFor(d => d.numSerieDispExt).NotEmpty().WithMessage("El número de serie del dispositivo es obligatorio");
                });
            }
        }

        public class Manejador : IRequestHandler<EjecutarEditarAsignacion>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEditarAsignacion request, CancellationToken cancellationToken)
            {
                // Se busca la asignación existente, incluyendo sus detalles.
                var asignacion = await _context.asignaciones
                    .Include(a => a.detalleAsignaciones)
                    .FirstOrDefaultAsync(a => a.idAsignacion == request.idAsignacion, cancellationToken);

                if (asignacion == null)
                {
                    throw new Exception("La asignación no existe");
                }

                // Se valida que el usuario exista.
                var usuario = await _context.usuarios.FindAsync(request.idUsuario);
                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                bool asignaEquipo = request.idEquipo.HasValue && request.idEquipo.Value > 0;
                bool asignaComponentes = request.componentes != null && request.componentes.Any();
                bool asignaDispositivos = request.dispositivosExt != null && request.dispositivosExt.Any();
                bool asignaEquipoSeguridad = request.idEquipoSeguridad.HasValue && request.idEquipoSeguridad.Value > 0;

                if (!asignaEquipo && !asignaComponentes && !asignaDispositivos && !asignaEquipoSeguridad)
                {
                    throw new Exception("Debe proporcionar al menos un idEquipo, un idComponente, un idEquipoSeguridad o un Dispositivo Externo.");
                }

                // Validaciones de existencia de relaciones.
                if (asignaEquipo)
                {
                    var equipo = await _context.equipos.FindAsync(request.idEquipo);
                    if (equipo == null)
                    {
                        throw new Exception("El equipo no existe");
                    }
                }

                if (asignaComponentes)
                {
                    foreach (var componente in request.componentes!)
                    {
                        var existeComponente = await _context.componentes.FindAsync(componente.idComponente);
                        if (existeComponente == null)
                        {
                            throw new Exception($"El componente con ID {componente.idComponente} no existe");
                        }
                    }
                }

                if (asignaEquipoSeguridad)
                {
                    var equipoSeg = await _context.equiposSeguridad.FindAsync(request.idEquipoSeguridad);
                    if (equipoSeg == null)
                    {
                        throw new Exception("El equipo de seguridad no existe");
                    }
                }

                // Actualizar campos de la asignación principal.
                asignacion.fechaAsignacion = request.fechaAsignacion;
                asignacion.idUsuario = request.idUsuario;
                // Actualizar otros campos si es necesario.

                // Se eliminan los detalles anteriores.
                _context.detalleAsignaciones.RemoveRange(asignacion.detalleAsignaciones);

                var nuevosDetalles = new List<DetalleAsignacion>();

                if (asignaEquipo)
                {
                    nuevosDetalles.Add(new DetalleAsignacion
                    {
                        idAsignacion = asignacion.idAsignacion,
                        idEquipo = request.idEquipo,
                        numSerieEquipo = request.numSerieEquipo,
                        ipAddress = request.ipAddress,
                        ipCpuRed = request.ipCpuRed,
                        idEquipoSeguridad = request.idEquipoSeguridad,
                        numSerieEquipoSeg = request.numSerieEquipoSeg,
                    });
                }

                if (asignaComponentes)
                {
                    foreach (var componente in request.componentes!)
                    {
                        nuevosDetalles.Add(new DetalleAsignacion
                        {
                            idAsignacion = asignacion.idAsignacion,
                            idComponente = componente.idComponente,
                            numSerieComponente = componente.numSerieComponente,
                        });
                    }
                }

                if (asignaDispositivos)
                {
                    var dispositivosValidos = request.dispositivosExt!
                        .Where(d => !string.IsNullOrWhiteSpace(d.marca) && d.marca != "string"
                                    && !string.IsNullOrWhiteSpace(d.descripcion) && d.descripcion != "string"
                                    && !string.IsNullOrWhiteSpace(d.numSerieDispExt) && d.numSerieDispExt != "string")
                        .ToList();

                    foreach (var dispositivo in dispositivosValidos)
                    {
                        var dispositivoExistente = await _context.dispositivosExt
                            .FirstOrDefaultAsync(d => d.idDispExt == dispositivo.idDispExt ||
                                                      (d.marca == dispositivo.marca && d.descripcion == dispositivo.descripcion));

                        if (dispositivoExistente == null)
                        {
                            dispositivoExistente = new DispositivoExt
                            {
                                marca = dispositivo.marca,
                                descripcion = dispositivo.descripcion
                            };
                            _context.dispositivosExt.Add(dispositivoExistente);
                            await _context.SaveChangesAsync(cancellationToken);
                        }

                        nuevosDetalles.Add(new DetalleAsignacion
                        {
                            idAsignacion = asignacion.idAsignacion,
                            idDispExt = dispositivoExistente.idDispExt,
                            numSerieDispExt = dispositivo.numSerieDispExt,
                        });
                    }
                }

                // Se agregan los nuevos detalles.
                _context.detalleAsignaciones.AddRange(nuevosDetalles);

                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar la asignación");
            }
        }
    }
}
