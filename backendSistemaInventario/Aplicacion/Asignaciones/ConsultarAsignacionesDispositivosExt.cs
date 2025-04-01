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
    public class ConsultarAsignacionesDispositivosExt
    {
        public class EjecutarConsultaDispositivosExt : IRequest<List<AsignacionDTO>>
        {
        }

        public class Manejador : IRequestHandler<EjecutarConsultaDispositivosExt, List<AsignacionDTO>>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<List<AsignacionDTO>> Handle(EjecutarConsultaDispositivosExt request, CancellationToken cancellationToken)
            {
                var asignaciones = await _context.asignaciones
                    .Include(a => a.usuario) // Incluimos la relación con el usuario
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(d => d.dispositivoExt) // Incluimos la relación con equipo
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
                        .Where(d => d.idDispExt != null)
                        .Select(d => new DetalleAsignacionDTO
                        {
                            idDetalleAsignacion = d.idDetalleAsignacion,
                            idAsignacion = a.idAsignacion,
                            idDispExt = d.idDispExt,
                            numSerieDispExt = d.numSerieDispExt,
                            // Mapeamos el objeto equipo completo
                            dispositivoExt = d.dispositivoExt == null ? null : new DispositivoExtDTO
                            {
                                idDispExt = d.dispositivoExt.idDispExt,
                                marca = d.dispositivoExt.marca,
                                descripcion = d.dispositivoExt.descripcion
                            }
                        }).ToList()
                }).ToList();

                return asignacionesDto;
            }
        }
    }
}
