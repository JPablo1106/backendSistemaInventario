using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Equipos
{
    public class ActualizarEquipo
    {
        public class EjecutarActualizarEquipo : IRequest<EquipoDTO>
        {
            public int idEquipo { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string tipoEquipo { get; set; }
            public string velocidadProcesador { get; set; }
            public string tipoProcesador { get; set; }
            public string memoriaRam { get; set; }
            public string tipoMemoriaRam { get; set; }

            // Datos del disco duro asociado
            public string marcaDisco { get; set; }
            public string modeloDisco { get; set; }
            public int c { get; set; }
            public int d { get; set; }
            public int e { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<EjecutarActualizarEquipo>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.idEquipo).GreaterThan(0).WithMessage("El ID del equipo es requerido");
                RuleFor(p => p.marca).NotEmpty().WithMessage("La marca del equipo es requerida");
                RuleFor(p => p.modelo).NotEmpty().WithMessage("El modelo del equipo es requerido");
                RuleFor(p => p.tipoEquipo).NotEmpty().WithMessage("El tipo de equipo es necesario");
                RuleFor(p => p.velocidadProcesador).NotEmpty().WithMessage("La velocidad del procesador es requerida");
                RuleFor(p => p.tipoProcesador).NotEmpty().WithMessage("El tipo de procesador es requerido");
                RuleFor(p => p.memoriaRam).NotEmpty().WithMessage("La memoria RAM es requerida");
                RuleFor(p => p.tipoMemoriaRam).NotEmpty().WithMessage("El tipo de memoria RAM es requerido");

                // Validaciones del Disco Duro
                RuleFor(p => p.marcaDisco).NotEmpty().WithMessage("La marca del disco duro es requerida");
                RuleFor(p => p.modeloDisco).NotEmpty().WithMessage("El modelo del disco duro es requerido");
                RuleFor(p => p.c).GreaterThanOrEqualTo(0).WithMessage("La partición C no puede ser negativa");
            }
        }

        public class Manejador : IRequestHandler<EjecutarActualizarEquipo, EquipoDTO>
        {
            private readonly ContextoBD _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoBD context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<EquipoDTO> Handle(EjecutarActualizarEquipo request, CancellationToken cancellationToken)
            {
                var equipo = await _context.equipos.FindAsync(request.idEquipo);
                if (equipo == null)
                {
                    throw new Exception("El equipo no existe.");
                }

                // Verificar si el disco duro ya existe
                var discoExistente = await _context.discoDuro.FirstOrDefaultAsync(d =>
                    d.marca == request.marcaDisco &&
                    d.modelo == request.modeloDisco &&
                    d.c == request.c &&
                    d.d == request.d &&
                    d.e == request.e, cancellationToken);

                int idDiscoDuro;

                if (discoExistente == null)
                {
                    // Crear un nuevo disco duro
                    var nuevoDisco = new DiscoDuro
                    {
                        marca = request.marcaDisco,
                        modelo = request.modeloDisco,
                        c = request.c,
                        d = request.d,
                        e = request.e,
                        capacidad = request.c + request.d + request.e,
                    };

                    _context.discoDuro.Add(nuevoDisco);
                    await _context.SaveChangesAsync(cancellationToken);
                    idDiscoDuro = nuevoDisco.idDiscoDuro;
                }
                else
                {
                    // Si ya existe, se usa el id del disco existente
                    idDiscoDuro = discoExistente.idDiscoDuro;
                }

                // Actualizar el equipo
                equipo.marca = request.marca;
                equipo.modelo = request.modelo;
                equipo.tipoEquipo = request.tipoEquipo;
                equipo.velocidadProcesador = request.velocidadProcesador;
                equipo.tipoProcesador = request.tipoProcesador;
                equipo.memoriaRam = request.memoriaRam;
                equipo.tipoMemoriaRam = request.tipoMemoriaRam;
                equipo.idDiscoDuro = idDiscoDuro;

                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    var equipoDTO = _mapper.Map<EquipoDTO>(equipo);

                    // Consultar el disco duro asociado
                    var discoDuro = await _context.discoDuro.FindAsync(idDiscoDuro);
                    equipoDTO.discoDuro = _mapper.Map<DiscoDuroDTO>(discoDuro);

                    return equipoDTO;
                }

                throw new Exception("No se pudo actualizar el equipo.");
            }
        }
    }
}
