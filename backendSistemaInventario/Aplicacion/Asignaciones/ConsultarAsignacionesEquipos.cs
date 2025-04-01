using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Asignaciones
{
    public class ConsultarAsignacionesEquipos
    {
        public class EjecutarConsulta : IRequest<List<AsignacionDTO>>
        {
        }

        public class Manejador : IRequestHandler<EjecutarConsulta, List<AsignacionDTO>>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<List<AsignacionDTO>> Handle(EjecutarConsulta request, CancellationToken cancellationToken)
            {
                var asignaciones = await _context.asignaciones
                    .Include(a => a.usuario) // Incluimos la relación con el usuario
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(d => d.equipo) // Incluimos la relación con equipo
                        .ThenInclude(e => e.discoDuro) // Incluimos la relación con discoDuro, si es necesario
                    .ToListAsync(cancellationToken);

                var asignacionesDto = asignaciones.Select(a => new AsignacionDTO
                {
                    idAsignacion = a.idAsignacion,
                    fechaAsignacion = a.fechaAsignacion,
                    idUsuario = a.idUsuario,
                    // Mapeo del usuario
                    usuario = a.usuario == null ? null : new UsuariosDTO
                    {
                        idUsuario = a.usuario.idUsuario,
                        nombreUsuario = a.usuario.nombreUsuario,
                        area = a.usuario.area,
                        departamento = a.usuario.departamento
                        // Agrega aquí las demás propiedades que tenga UsuariosDTO
                    },
                    // Mapeamos los detalles, filtrando aquellos en los que idEquipo sea nulo
                    detalleAsignaciones = a.detalleAsignaciones
                        .Where(d => d.idEquipo != null)
                        .Select(d => new DetalleAsignacionDTO
                        {
                            idDetalleAsignacion = d.idDetalleAsignacion,
                            idAsignacion = a.idAsignacion,
                            idEquipo = d.idEquipo,
                            numSerieEquipo = d.numSerieEquipo,
                            ipAddress = d.ipAddress,
                            ipCpuRed = d.ipCpuRed,
                            // Mapeamos el objeto equipo completo
                            equipo = d.equipo == null ? null : new EquipoDTO
                            {
                                idEquipo = d.equipo.idEquipo,
                                marca = d.equipo.marca,
                                modelo = d.equipo.modelo,
                                tipoEquipo = d.equipo.tipoEquipo,
                                velocidadProcesador = d.equipo.velocidadProcesador,
                                tipoProcesador = d.equipo.tipoProcesador,
                                memoriaRam = d.equipo.memoriaRam,
                                tipoMemoriaRam = d.equipo.tipoMemoriaRam,
                                idDiscoDuro = d.equipo.idDiscoDuro,
                                discoDuro = d.equipo.discoDuro == null ? null : new DiscoDuroDTO
                                {
                                    idDiscoDuro = d.equipo.discoDuro.idDiscoDuro,
                                    marca = d.equipo.discoDuro.marca,
                                    modelo = d.equipo.discoDuro.modelo,
                                    capacidad = d.equipo.discoDuro.capacidad,
                                    c = d.equipo.discoDuro.c,
                                    d = d.equipo.discoDuro.d,
                                    e = d.equipo.discoDuro.e
                                }
                            }
                        }).ToList()
                }).ToList();

                return asignacionesDto;
            }
        }
    }
}
