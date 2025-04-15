using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Radios
{
    public class ConsultarRadioPorId
    {
        public class EjecutaConsultaRadioPorId : IRequest<RadioDTO>
        {
            public int idRadio { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutaConsultaRadioPorId, RadioDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RadioDTO> Handle(EjecutaConsultaRadioPorId request, CancellationToken cancellationToken)
            {
                var radio = await _context.radios.FirstOrDefaultAsync
                    (r => r.idRadio == request.idRadio, cancellationToken);
                if (radio == null)
                {
                    throw new Exception("No se encontró el radio");
                }

                var radioDTO = _mapper.Map<Radio, RadioDTO>(radio);
                return radioDTO;
            }
        }
    }
}
