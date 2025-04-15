using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Radios
{
    public class RegistroRadios
    {
        public class EjecutarRegistroRadios : IRequest
        {
            public int idUsuario { get; set; }

            public List<RegistroRadioDTO> Radios { get; set; } = new List<RegistroRadioDTO>();
        }

        public class RegistroRadioDTO
        {
            public string marca { get; set; }
            public string modelo { get; set; }
            public string issi { get; set; }
            public string numSerie { get; set; }
            public bool tieneAntena { get; set; }
            public bool tieneClip { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarRegistroRadios>
        {
            public EjecutaValidacion()
            {
                RuleFor(r => r.idUsuario).GreaterThan(0);
                RuleForEach(t => t.Radios).SetValidator(new RadioValidator());
            }
        }

        public class RadioValidator : AbstractValidator<RegistroRadioDTO>
        {
            public RadioValidator()
            {
                RuleFor(r => r.marca).NotEmpty();
                RuleFor(r => r.modelo).NotEmpty();
                RuleFor(r => r.issi).NotEmpty();
                RuleFor(r => r.numSerie).NotEmpty();
                RuleFor(r => r.tieneAntena).NotEmpty();
                RuleFor(r => r.tieneClip).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<EjecutarRegistroRadios>
        {
            public readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarRegistroRadios request, CancellationToken cancellationToken)
            {
                var usuario = await _context.usuarios.FindAsync(request.idUsuario, cancellationToken);
                if(usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                foreach( var radioDTO in request.Radios)
                {
                    var radio = new Radio
                    {
                        marca = radioDTO.marca,
                        modelo = radioDTO.modelo,
                        issi = radioDTO.issi,
                        numSerie = radioDTO.numSerie,
                        tieneAntena = radioDTO.tieneAntena,
                        tieneClip = radioDTO.tieneClip,
                        idUsuario = request.idUsuario
                    };
                    _context.radios.Add(radio);
                }

                var respuesta = await _context.SaveChangesAsync(cancellationToken);
                if (respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron registrar los radios");
            }
        }
    }
}
