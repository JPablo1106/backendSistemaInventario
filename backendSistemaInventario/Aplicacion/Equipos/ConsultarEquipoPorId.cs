using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Equipos
{
    public class ConsultarEquipoPorId
    {
        public class EjecutarConsultaEquipoId : IRequest<EquipoDTO>
        {
            public int idEquipo { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutarConsultaEquipoId, EquipoDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<EquipoDTO> Handle(EjecutarConsultaEquipoId request, CancellationToken cancellationToken)
            {
                var equipo = await _context.equipos
                    .Include(e => e.discoDuro) // se incluye la relación con el disco duro
                    .FirstOrDefaultAsync(e => e.idEquipo == request.idEquipo, cancellationToken);

                if (equipo == null)
                {
                    // Se puede manejar de la forma que prefieras, aquí se lanza una excepción:
                    throw new Exception("No se encontró el equipo.");
                }

                var equipoDTO = _mapper.Map<EquipoDTO>(equipo);
                return equipoDTO;
            }
        }
    }
}
