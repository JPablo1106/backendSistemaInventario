﻿using backendSistemaInventario.DTOS;
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
    public class ConsultarAsignacionesMonitores
    {
        public class EjecutarConsultaMonitores : IRequest<List<AsignacionDTO>>
        {
        }

        public class Manejador : IRequestHandler<EjecutarConsultaMonitores, List<AsignacionDTO>>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<List<AsignacionDTO>> Handle(EjecutarConsultaMonitores request, CancellationToken cancellationToken)
            {
                // Se incluyen las relaciones con usuario y componente (incluyendo la herencia) en los detalles.
                var asignaciones = await _context.asignaciones
                    .Include(a => a.usuario)
                    .Include(a => a.detalleAsignaciones)
                        .ThenInclude(d => d.componente)
                    // Se filtran las asignaciones que tengan al menos un detalle con componente de tipo Monitores.
                    .Where(a => a.detalleAsignaciones.Any(d => d.componente is Monitores))
                    .ToListAsync(cancellationToken);

                // Mapeo a DTO, filtrando solo aquellos detalles en los que el componente sea de tipo Monitores.
                var asignacionesDto = asignaciones.Select(a => new AsignacionDTO
                {
                    idAsignacion = a.idAsignacion,
                    fechaAsignacion = a.fechaAsignacion,
                    idUsuario = a.idUsuario,
                    usuario = a.usuario == null ? null : new UsuariosDTO
                    {
                        idUsuario = a.usuario.idUsuario,
                        nombreUsuario = a.usuario.nombreUsuario,
                        area = a.usuario.area,
                        departamento = a.usuario.departamento
                    },
                    detalleAsignaciones = a.detalleAsignaciones
                        .Where(d => d.componente is Monitores)
                        .Select(d => new DetalleAsignacionDTO
                        {
                            idDetalleAsignacion = d.idDetalleAsignacion,
                            idAsignacion = a.idAsignacion,
                            idComponente = d.idComponente,
                            numSerieComponente = d.numSerieComponente,
                            // Se realiza el cast de componente a Monitores para mapear sus propiedades específicas.
                            componente = d.componente is Monitores monitor ? new ComponenteDTO
                            {
                                idComponente = monitor.idComponente,
                                tipoComponente = monitor.tipoComponente,
                                marcaComponente = monitor.marcaComponente,
                                modeloMonitor = monitor.modeloMonitor
                            } : null
                        }).ToList()
                }).ToList();

                return asignacionesDto;
            }
        }
    }
}
