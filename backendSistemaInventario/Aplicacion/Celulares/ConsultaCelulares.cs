using AutoMapper;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Celulares
{
    public class ConsultaCelulares
    {
        public class ListaCelulares : IRequest<List<CelularDTO>>
        {

        }

        public class Manejador : IRequestHandler<ListaCelulares, List<CelularDTO>>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CelularDTO>> Handle(ListaCelulares request, CancellationToken cancellationToken)
            {
                var celulares = await _context.celulares.ToListAsync();
                var celularesDTO = _mapper.Map<List<Celular>, List<CelularDTO>>(celulares);
                return celularesDTO;
            }
        }
    }
}
