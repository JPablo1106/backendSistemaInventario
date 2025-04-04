using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Asignaciones
{
    public class ConsultarAsignacionPorId
    {
        public class Ejecutar : IRequest<AsignacionDTO?>
        {
            public int IdAsignacion { get; set; }

            public Ejecutar(int idAsignacion)
            {
                IdAsignacion = idAsignacion;
            }
        }

        public class Manejador : IRequestHandler<Ejecutar, AsignacionDTO?>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<AsignacionDTO?> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var asignacion = await _context.asignaciones
                    .Include(a => a.usuario)
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(da => da.equipo)
                            .ThenInclude(e => e.discoDuro)
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(da => da.componente)
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(da => da.dispositivoExt)
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(da => da.equipoSeguridad)
                    .FirstOrDefaultAsync(a => a.idAsignacion == request.IdAsignacion, cancellationToken);

                if (asignacion == null)
                {
                    return null;
                }

                return new AsignacionDTO
                {
                    idAsignacion = asignacion.idAsignacion,
                    fechaAsignacion = asignacion.fechaAsignacion,
                    idUsuario = asignacion.idUsuario,
                    usuario = asignacion.usuario == null ? null : new UsuariosDTO
                    {
                        idUsuario = asignacion.usuario.idUsuario,
                        nombreUsuario = asignacion.usuario.nombreUsuario,
                        area = asignacion.usuario.area,
                        departamento = asignacion.usuario.departamento
                    },
                    detalleAsignaciones = asignacion.detalleAsignaciones.Select(da => new DetalleAsignacionDTO
                    {
                        idDetalleAsignacion = da.idDetalleAsignacion,
                        idAsignacion = da.idAsignacion,
                        idEquipo = da.idEquipo,
                        equipo = da.equipo == null ? null : new EquipoDTO
                        {
                            idEquipo = da.equipo.idEquipo,
                            marca = da.equipo.marca,
                            modelo = da.equipo.modelo,
                            tipoEquipo = da.equipo.tipoEquipo,
                            velocidadProcesador = da.equipo.velocidadProcesador,
                            tipoProcesador = da.equipo.tipoProcesador,
                            memoriaRam = da.equipo.memoriaRam,
                            tipoMemoriaRam = da.equipo.tipoMemoriaRam,
                            idDiscoDuro = da.equipo.idDiscoDuro,
                            discoDuro = da.equipo.discoDuro == null ? null : new DiscoDuroDTO
                            {
                                idDiscoDuro = da.equipo.discoDuro.idDiscoDuro,
                                marca = da.equipo.discoDuro.marca,
                                modelo = da.equipo.discoDuro.modelo,
                                capacidad = da.equipo.discoDuro.capacidad,
                                c = da.equipo.discoDuro.c,
                                d = da.equipo.discoDuro.d,
                                e = da.equipo.discoDuro.e,
                            }
                        },
                        numSerieEquipo = da.numSerieEquipo,
                        ipAddress = da.ipAddress,
                        ipCpuRed = da.ipCpuRed,
                        idComponente = da.idComponente,
                        componente = MapComponente(da.componente),
                        numSerieComponente = da.numSerieComponente,
                        idDispExt = da.idDispExt,
                        dispositivoExt = da.dispositivoExt == null ? null : new DispositivoExtDTO
                        {
                            idDispExt = da.dispositivoExt.idDispExt,
                            marca = da.dispositivoExt.marca,
                            descripcion = da.dispositivoExt.descripcion,
                        },
                        numSerieDispExt = da.numSerieDispExt,
                        idEquipoSeguridad = da.idEquipoSeguridad,
                        equipoSeguridad = da.equipoSeguridad == null ? null : new EquipoSeguridadDTO
                        {
                            idEquipoSeguridad = da.equipoSeguridad.idEquipoSeguridad,
                            marca = da.equipoSeguridad.marca,
                            modelo = da.equipoSeguridad.modelo,
                            capacidad = da.equipoSeguridad.capacidad,
                            tipo = da.equipoSeguridad.tipo,
                        },
                        numSerieEquipoSeg = da.numSerieEquipoSeg
                    }).ToList()
                };
            }

            private static ComponenteDTO? MapComponente(Componente? componente)
            {
                if (componente == null)
                    return null;

                var dto = new ComponenteDTO
                {
                    idComponente = componente.idComponente,
                    tipoComponente = componente.tipoComponente,
                    marcaComponente = componente.marcaComponente,
                };

                if (componente is Modelo.Monitores monitor)
                {
                    dto.modeloMonitor = monitor.modeloMonitor;
                }
                else if (componente is Modelo.Telefono telefono)
                {
                    dto.modeloTelefono = telefono.modeloTelefono;
                }
                else if (componente is Modelo.Teclado teclado)
                {
                    dto.idiomaTeclado = teclado.idiomaTeclado;
                }
                return dto;
            }
        }
    }
}
