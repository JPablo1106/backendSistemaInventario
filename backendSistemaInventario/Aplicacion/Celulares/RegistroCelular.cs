using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Celulares
{
    public class RegistroCelular
    {
        public class EjecutarRegistroCelular : IRequest
        {
            public string marca { get; set; }
            public string modelo { get; set; }
            public string compania { get; set; }
            public string numSerie { get; set; }
            public string imei { get; set; }
            public string numCelular { get; set; }
            public int idUsuario { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroCelular>
        {
            public EjecutaValidacion()
            {
                RuleFor(c => c.marca).NotEmpty();
                RuleFor(c => c.modelo).NotEmpty();
                RuleFor(c => c.compania).NotEmpty();
                RuleFor(c => c.numSerie).NotEmpty();
                RuleFor(c => c.imei).NotEmpty();
                RuleFor(c => c.numCelular).NotEmpty();
                RuleFor(c => c.idUsuario).GreaterThan(0);
            }
        }

        public class Manejador : IRequestHandler<EjecutarRegistroCelular>
        {
            public readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }   

            public async Task<Unit> Handle(EjecutarRegistroCelular request, CancellationToken cancellationToken)
            {
                var usuario = await _context.usuarios.FindAsync(request.idUsuario);
                if(usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }
                var celular = new Celular
                {
                    marca = request.marca,
                    modelo = request.modelo,
                    compania = request.compania,
                    numSerie = request.numSerie,
                    imei = request.imei,
                    numCelular = request.numCelular,
                    idUsuario = request.idUsuario,
                };

                _context.celulares.Add(celular);
                var respuesta = await _context.SaveChangesAsync();
                if(respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo registrar el celular");
            }
        }
    }
}
