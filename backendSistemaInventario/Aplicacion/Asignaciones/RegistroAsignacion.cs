using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Asignaciones
{
    public class RegistroAsignacion
    {

        public class ComponenteAsignado
        {
            public int idComponente { get; set; }
            public string numSerieComponente { get; set; }
        }

        public class DispositivoExtAsignado
        {
            public int? idDispExt { get; set; }
            public string marca { get; set; }
            public string descripcion { get; set; }
            public string numSerieDispExt { get; set; }
        }
        // Comando actualizado que permite enviar seriales e identificar ambos elementos.
        public class EjecutarRegistroAsignacion : IRequest
        {
            public int idUsuario { get; set; }
            public int? idEquipo { get; set; }
            public string? numSerieEquipo { get; set; }
            public DateTime fechaAsignacion { get; set; }
            public string? ipAddress { get; set; }
            public string? ipCpuRed { get; set; }
            public List<ComponenteAsignado>? componentes { get; set; }
            public List<DispositivoExtAsignado>? dispositivosExt { get; set; }
            public int? idEquipoSeguridad {  get; set; }
            public string? numSerieEquipoSeg {  get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroAsignacion>
        {
            public EjecutaValidacion()
            {
                RuleFor(a => a.fechaAsignacion)
                    .NotEmpty().WithMessage("La fecha de asignación es obligatoria");

                RuleFor(a => a.idUsuario)
                    .GreaterThan(0).WithMessage("El ID del usuario debe ser válido");

                // Se requiere que se asigne al menos un elemento: equipo o componente.
                RuleFor(a => a)
                    .Must(a =>
                    {
                        bool tieneEquipo = a.idEquipo.HasValue && a.idEquipo.Value > 0;
                        bool tieneComponentes = a.componentes != null && a.componentes.Any();
                        bool tieneDispositivos = a.dispositivosExt != null && a.dispositivosExt.Any();
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

                // Validaciones para componentes
                RuleForEach(a => a.componentes).ChildRules(componente =>
                {
                    componente.RuleFor(c => c.idComponente).GreaterThan(0).WithMessage("El ID del componente debe ser válido");

                    componente.RuleFor(c => c.numSerieComponente).NotEmpty().WithMessage("El número de serie del componente es obligatorio");
                });

                RuleForEach(a => a.dispositivosExt).ChildRules(dispositivo =>
                {
                    dispositivo.RuleFor(d => d.marca).NotEmpty().WithMessage("La marca del dispositivo es obligatoria");
                    dispositivo.RuleFor(d => d.descripcion).NotEmpty().WithMessage("La descripción del dispositivo es obligatoria");
                    dispositivo.RuleFor(d => d.numSerieDispExt).NotEmpty().WithMessage("El número de serie del dispositivo es obligatorio");
                });
            }
        }

        public class Manejador : IRequestHandler<EjecutarRegistroAsignacion>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroAsignacion request, CancellationToken cancellationToken)
            {
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

                var nuevaAsignacion = new Asignacion
                {
                    fechaAsignacion = request.fechaAsignacion,
                    idUsuario = request.idUsuario,
                };

                _context.asignaciones.Add(nuevaAsignacion);
                var resultadoAsignacion = await _context.SaveChangesAsync(cancellationToken);

                if (resultadoAsignacion <= 0)
                {
                    throw new Exception("No se pudo registrar la asignación");
                }

                var detalles = new List<DetalleAsignacion>();

                if (asignaEquipo)
                {
                    detalles.Add(new DetalleAsignacion
                    {
                        idAsignacion = nuevaAsignacion.idAsignacion,
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
                        detalles.Add(new DetalleAsignacion
                        {
                            idAsignacion = nuevaAsignacion.idAsignacion,
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

                        detalles.Add(new DetalleAsignacion
                        {
                            idAsignacion = nuevaAsignacion.idAsignacion,
                            idDispExt = dispositivoExistente.idDispExt,
                            numSerieDispExt = dispositivo.numSerieDispExt,
                        });
                    }
                }


                _context.detalleAsignaciones.AddRange(detalles);
                var resultadoDetalle = await _context.SaveChangesAsync(cancellationToken);

                if (resultadoDetalle > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo registrar el detalle de la asignación");
            }
        }
    }
}