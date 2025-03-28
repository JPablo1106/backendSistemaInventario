using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Componentes
{
    public class ConsultarComponentes
    {
        public class ListaComponentes : IRequest<List<ComponenteDTO>> { }

        public class Manejador : IRequestHandler<ListaComponentes, List<ComponenteDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ComponenteDTO>> Handle(ListaComponentes request, CancellationToken cancellationToken)
            {
                var componentes = await _context.componentes.ToListAsync(cancellationToken);
                var componentesDTO = _mapper.Map<List<Componente>, List<ComponenteDTO>>(componentes);
                return componentesDTO;
            }
        }
    }
}
