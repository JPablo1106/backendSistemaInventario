using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Persistencia;
using DocumentFormat.OpenXml.Drawing;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Tabletas
{
    public class ActualizarTableta
    {
        public class EjecutarActualizarTableta : IRequest<TabletaDTO>
        {
            public int idTableta { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string numSerie { get; set; }
            public string accesorios { get; set; }
            public int idUsuario { get; set; }
        }

        public class EjecutarValidacion : AbstractValidator<EjecutarActualizarTableta>
        {
            public EjecutarValidacion()
            {
                RuleFor(t => t.idTableta).NotEmpty();
                RuleFor(t => t.marca).NotEmpty();
                RuleFor(t => t.modelo).NotEmpty();
                RuleFor(t => t.numSerie).NotEmpty();
                RuleFor(t => t.accesorios).NotEmpty();
                RuleFor(t => t.idUsuario).GreaterThan(0);
            }
        }

        public class Manejador : IRequestHandler<EjecutarActualizarTableta, TabletaDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TabletaDTO> Handle (EjecutarActualizarTableta request, CancellationToken cancellationToken)
            {
                var tableta = await _context.tabletas.FirstOrDefaultAsync(t => t.idTableta == request.idTableta, cancellationToken);

                if (tableta == null)
                {
                    throw new Exception("La tableta no existe");
                }

                var usuario = await _context.tabletas.FindAsync(request.idUsuario);
                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                tableta.marca = request.marca;
                tableta.modelo = request.modelo;
                tableta.numSerie = request.numSerie;
                tableta.accesorios = request.accesorios;
                tableta.idUsuario = request.idUsuario;

                var resultado = await _context.SaveChangesAsync(cancellationToken);
                if(resultado > 0)
                {
                    return _mapper.Map<TabletaDTO>(tableta);
                }
                throw new Exception("No se pudo actualizar la tableta");
            }
        }
    }
}
