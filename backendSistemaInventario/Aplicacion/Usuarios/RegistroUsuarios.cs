using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Usuarios
{
    public class RegistroUsuarios
    {
        public class EjecutarRegistroUsuario : IRequest
        {
            public string nombreUsuario { get; set; }
            public string area { get; set; }
            public string departamento { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroUsuario>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.nombreUsuario).NotEmpty();
                RuleFor(p => p.area).NotEmpty();
                RuleFor(p => p.departamento).NotEmpty();

            }
        }

        public class Manejador : IRequestHandler<EjecutarRegistroUsuario>
        {
            public readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroUsuario request, CancellationToken cancellationToken)
            {
                var usuario = new Usuario
                {
                    nombreUsuario = request.nombreUsuario,
                    area = request.area,
                    departamento = request.departamento,
                };

                _context.usuarios.Add(usuario);
                var respuesta = await _context.SaveChangesAsync();
                if(respuesta > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo registrar el usuario");
            }
        }
    }
}
