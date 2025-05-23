﻿using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backendSistemaInventario.Aplicacion.Asignaciones
{
    public class ConsultaAsignacionBusqueda
    {
        // Clase de request que incluye los parámetros de búsqueda
        public class Ejecutar : IRequest<List<AsignacionDTO>>
        {
            // Parámetro para filtrar por número de serie (ej. equipo, componente, etc.)
            public string? NumeroSerie { get; set; }
            // Parámetro para filtrar por marca del equipo
            public string? Marca { get; set; }
            // Parámetro para filtrar por modelo del equipo
            public string? Modelo { get; set; }
        }

        // Manejador que procesa la petición y retorna la lista de AsignacionDTO filtrada
        public class Manejador : IRequestHandler<Ejecutar, List<AsignacionDTO>>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<List<AsignacionDTO>> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                // Se incluye las relaciones necesarias para obtener la información completa
                var query = _context.asignaciones
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
                    .AsQueryable();

                // Filtro por número de serie en equipo (puedes ampliar la búsqueda a otros campos según se requiera)
                if (!string.IsNullOrEmpty(request.NumeroSerie))
                {
                    query = query.Where(a => a.detalleAsignaciones.Any(da => da.numSerieEquipo.Contains(request.NumeroSerie)));
                }

                // Filtro por marca del equipo
                if (!string.IsNullOrEmpty(request.Marca))
                {
                    query = query.Where(a => a.detalleAsignaciones.Any(da => da.equipo != null && da.equipo.marca.Contains(request.Marca)));
                }

                // Filtro por modelo del equipo
                if (!string.IsNullOrEmpty(request.Modelo))
                {
                    query = query.Where(a => a.detalleAsignaciones.Any(da => da.equipo != null && da.equipo.modelo.Contains(request.Modelo)));
                }

                // Se ejecuta la consulta con los filtros aplicados
                var asignaciones = await query.ToListAsync(cancellationToken);

                // Mapeo de las entidades a DTOs
                var asignacionesDTO = asignaciones.Select(a => new AsignacionDTO
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
                    detalleAsignaciones = a.detalleAsignaciones.Select(da => new DetalleAsignacionDTO
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
                }).ToList();

                return asignacionesDTO;
            }

            // Método auxiliar para mapear la entidad Componente a su DTO considerando la herencia.
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

                // Se asignan las propiedades específicas según el tipo de componente.
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
                // El tipo Mouse no posee propiedades adicionales.
                return dto;
            }
        }
    }
}
