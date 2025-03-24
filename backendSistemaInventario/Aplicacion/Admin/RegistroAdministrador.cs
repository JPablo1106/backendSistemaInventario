using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Admin
{
    public class RegistroAdministrador
    {
        public class EjecutarRegistroAdministrador : IRequest
        {
            public string nombreAdmin { get; set; }
            public string usuario { get; set; }
            public string contraseña { get; set; }

        }
        //clase para validar la clase ejecuta a traves de apifluent validator
        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroAdministrador>
        {
            public EjecutaValidacion()
            {
                //Estas propiedades no se aceptan valores nulos
                RuleFor(p => p.nombreAdmin).NotEmpty();
                RuleFor(p => p.usuario).NotEmpty();
                RuleFor(p => p.contraseña).NotEmpty();
            }

        }

        public class Manejador : IRequestHandler<EjecutarRegistroAdministrador>
        {
            public readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroAdministrador request, CancellationToken cancellationToken)
            {
                //Se crea la instancia del administrador ligada al contexto
                var administrador = new Administrador
                {
                    nombreAdmin = request.nombreAdmin,
                    usuario = request.usuario,
                    contraseña = request.contraseña
                };

                //Se agrega el objeto del tipo administrador
                _context.administrador.Add(administrador);
                //Insertamos el valor de insercion
                var respuesta = await _context.SaveChangesAsync();
                if(respuesta > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo registrar el administrador");
            }
        }
    }
}
