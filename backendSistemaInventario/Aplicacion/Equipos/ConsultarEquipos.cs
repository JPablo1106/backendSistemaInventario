using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Equipos
{
    public class ConsultarEquipos
    {
        public class ListaEquipos : IRequest<List<EquipoDTO>>
        {

        }

        public class Manejador : IRequestHandler<ListaEquipos, List<EquipoDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<EquipoDTO>> Handle(ListaEquipos request, CancellationToken cancellationToken)
            {
                var equipos = await _context.equipos
                    .Include(e => e.discoDuro) //cargar datos del disco duro asociado
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<EquipoDTO>>(equipos);
            }
        }
    }
}
