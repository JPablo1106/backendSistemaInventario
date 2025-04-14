using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Componentes
{
    public class ConsultarComponentePorId
    {
        public class EjecutaConsultaComponenteId : IRequest<ComponenteDTO>
        {
            public int idComponente { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutaConsultaComponenteId, ComponenteDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ComponenteDTO> Handle(EjecutaConsultaComponenteId request, CancellationToken cancellationToken)
            {
                var componente = await _context.componentes
                    .FirstOrDefaultAsync(c => c.idComponente == request.idComponente, cancellationToken);

                if (componente == null)
                {
                    throw new Exception("No se encontró el componente.");
                }

                var componenteDTO = _mapper.Map<Componente, ComponenteDTO>(componente);
                return componenteDTO;
            }
        }
    }
}
