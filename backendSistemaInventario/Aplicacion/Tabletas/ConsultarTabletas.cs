using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Tabletas
{
    public class ConsultarTabletas
    {
        public class ListaTabletas : IRequest<List<TabletaDTO>>
        {

        }

        public class Manejador : IRequestHandler<ListaTabletas, List<TabletaDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TabletaDTO>> Handle(ListaTabletas request, CancellationToken cancellationToken)
            {
                var tabletas = await _context.tabletas.ToListAsync();
                var tabletasDTO = _mapper.Map<List<Tableta>, List<TabletaDTO>>(tabletas);
                return tabletasDTO;
            }
        }
    }
}
