using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Radios
{
    public class ConsultarRadios
    {
        public class ListaRadios : IRequest<List<RadioDTO>>
        {

        }

        public class Manejador : IRequestHandler<ListaRadios, List<RadioDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<RadioDTO>> Handle(ListaRadios request, CancellationToken cancellationToken)
            {
                var radios = await _context.radios.ToListAsync();
                var radiosDTO = _mapper.Map<List<Radio>, List<RadioDTO>>(radios);
                return radiosDTO;
            }
        }
    }
}
