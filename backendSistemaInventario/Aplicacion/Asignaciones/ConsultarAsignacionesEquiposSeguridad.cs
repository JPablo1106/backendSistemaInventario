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
    public class ConsultarAsignacionesEquiposSeguridad
    {
        public class EjecutarConsultaEquiposSeguridad : IRequest<List<AsignacionDTO>>
        {
        }

        public class Manejador : IRequestHandler<EjecutarConsultaEquiposSeguridad, List<AsignacionDTO>>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<List<AsignacionDTO>> Handle(EjecutarConsultaEquiposSeguridad request, CancellationToken cancellationToken)
            {
                var asignaciones = await _context.asignaciones
                    .Include(a => a.usuario) // Incluimos la relación con el usuario
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(d => d.equipoSeguridad) // Incluimos la relación con equipo
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
                    },
                    // Mapeamos los detalles, filtrando aquellos en los que idEquipo sea nulo
                    detalleAsignaciones = a.detalleAsignaciones
                        .Where(d => d.idEquipoSeguridad != null)
                        .Select(d => new DetalleAsignacionDTO
                        {
                            idDetalleAsignacion = d.idDetalleAsignacion,
                            idAsignacion = a.idAsignacion,
                            idEquipoSeguridad = d.idEquipoSeguridad,
                            numSerieEquipoSeg = d.numSerieEquipoSeg,
                            // Mapeamos el objeto equipo completo
                            equipoSeguridad = d.equipoSeguridad == null ? null : new EquipoSeguridadDTO
                            {
                                idEquipoSeguridad = d.equipoSeguridad.idEquipoSeguridad,
                                marca = d.equipoSeguridad.marca,
                                modelo = d.equipoSeguridad.modelo,
                                tipo = d.equipoSeguridad.tipo,
                                capacidad = d.equipoSeguridad.capacidad
                            }
                        }).ToList()
                }).ToList();

                return asignacionesDto;
            }
        }
    }
}
