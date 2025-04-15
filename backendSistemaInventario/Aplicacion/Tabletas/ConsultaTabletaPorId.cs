using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Tabletas
{
    public class ConsultaTabletaPorId
    {
        public class EjecutaConsultaTabletaPorId : IRequest<TabletaDTO>
        {
            public int idTableta { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutaConsultaTabletaPorId, TabletaDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TabletaDTO> Handle(EjecutaConsultaTabletaPorId request, CancellationToken cancellationToken)
            {
                var tableta = await _context.tabletas.FirstOrDefaultAsync
                    (t => t.idTableta == request.idTableta, cancellationToken);
                if(tableta == null)
                {
                    throw new Exception("No se encontró la tableta");
                }

                var tabletaDTO = _mapper.Map<Tableta, TabletaDTO>(tableta);
                return tabletaDTO;
            }
        }
    }
}
