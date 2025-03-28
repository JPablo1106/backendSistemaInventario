using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.EquiposSeguridad
{
    public class ConsultarEquiposSeguridad
    {
        public class ListaEquiposSeguridad : IRequest<List<EquipoSeguridadDTO>> { }

        public class Manejador : IRequestHandler<ListaEquiposSeguridad, List<EquipoSeguridadDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<EquipoSeguridadDTO>> Handle(ListaEquiposSeguridad request, CancellationToken cancellationToken)
            {
                var equipos = await _context.equiposSeguridad.ToListAsync();
                var equiposDTO = _mapper.Map<List<EquipoSeguridad>, List<EquipoSeguridadDTO>>(equipos);
                return equiposDTO;
            }
        }
    }
}
