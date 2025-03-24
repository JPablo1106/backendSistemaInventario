using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using System.Data;

namespace backendSistemaInventario.Aplicacion.EquiposSeguridad
{
    public class RegistroEquipoSeguridad
    {

        public class EjecutarRegistroEquipoSeguridad : IRequest
        {
            public string marca { get; set; }
            public string modelo { get; set; }
            public string capacidad { get; set; }
            public string tipo { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroEquipoSeguridad>
        {
            public EjecutaValidacion()
            {
                RuleFor(e => e.marca).NotEmpty();
                RuleFor(e => e.modelo).NotEmpty();
                RuleFor(e => e.capacidad).NotEmpty();
                RuleFor(e => e.tipo).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<EjecutarRegistroEquipoSeguridad>
        {
            public readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroEquipoSeguridad request, CancellationToken cancellationToken)
            {
                var equipoSeguridad = new EquipoSeguridad
                {
                    marca = request.marca,
                    modelo = request.modelo,
                    capacidad = request.capacidad,
                    tipo = request.tipo,
                };

                _context.equiposSeguridad.Add(equipoSeguridad);
                var respuesta = await _context.SaveChangesAsync();

                if(respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo registrar el equipo de seguridad");
            }
        }
    }
}
