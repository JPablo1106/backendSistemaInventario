using AutoMapper;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Celulares
{
    public class ActualizarCelular
    {
        public class EjecutarActualizarCelular : IRequest<CelularDTO>
        {
            public int idCelular { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string compania { get; set; }
            public string numSerie { get; set; }
            public string imei { get; set; }
            public string numCelular { get; set; }
            public int idUsuario { get; set; }
        }

        public class EjecutarValidacion : AbstractValidator<EjecutarActualizarCelular>
        {
            public EjecutarValidacion()
            {
                RuleFor(c => c.idCelular).NotEmpty();
                RuleFor(c => c.marca).NotEmpty();
                RuleFor(c => c.modelo).NotEmpty();
                RuleFor(c => c.compania).NotEmpty();
                RuleFor(c => c.numSerie).NotEmpty();
                RuleFor(c => c.imei).NotEmpty();
                RuleFor(c => c.numCelular).NotEmpty();
                RuleFor(c => c.idUsuario).GreaterThan(0);
            }
        }

        public class Manejador : IRequestHandler<EjecutarActualizarCelular, CelularDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CelularDTO> Handle(EjecutarActualizarCelular request, CancellationToken cancellationToken)
            {
                var celular = await _context.celulares.FirstOrDefaultAsync(c => c.idCelular == request.idCelular, cancellationToken);

                if (celular == null)
                {
                    throw new Exception("El celular no existe");
                }

                var usuario = await _context.usuarios.FindAsync(request.idUsuario);
                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                celular.marca = request.marca;
                celular.modelo = request.modelo;
                celular.compania = request.compania;
                celular.numSerie = request.numSerie;
                celular.imei = request.imei;
                celular.numCelular = request.numCelular;
                celular.idUsuario = request.idUsuario;

                var resultado = await _context.SaveChangesAsync(cancellationToken);
                if (resultado > 0)
                {
                    return _mapper.Map<CelularDTO>(celular);
                }
                throw new Exception("No se pudo actualizar el celular");
            }
        }
    }
}
