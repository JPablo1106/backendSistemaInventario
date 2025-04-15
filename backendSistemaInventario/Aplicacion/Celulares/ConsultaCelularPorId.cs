using AutoMapper;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Celulares
{
    public class ConsultaCelularPorId
    {
        public class EjecutaConsultaCelularPorId : IRequest<CelularDTO>
        {
            public int idCelular { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutaConsultaCelularPorId, CelularDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CelularDTO>Handle(EjecutaConsultaCelularPorId request, CancellationToken cancellationToken)
            {
                var celular = await _context.celulares.FirstOrDefaultAsync
                    (c => c.idCelular == request.idCelular, cancellationToken);
                if (celular == null)
                {
                    throw new Exception("No se encontró el celular");
                }

                var celularDTO = _mapper.Map<Celular, CelularDTO>(celular);
                return celularDTO;
            }
        }
    }
}
