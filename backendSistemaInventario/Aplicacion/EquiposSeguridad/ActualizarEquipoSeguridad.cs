using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.EquiposSeguridad
{
    public class ActualizarEquipoSeguridad
    {
        // Se define la solicitud que retorna EquipoSeguridadDTO
        public class EjecutarActualizarEquipoSeguridad : IRequest<EquipoSeguridadDTO>
        {
            public int idEquipoSeguridad { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string capacidad { get; set; }
            public string tipo { get; set; }
        }

        // Validación de entrada
        public class EjecutaValidacion : AbstractValidator<EjecutarActualizarEquipoSeguridad>
        {
            public EjecutaValidacion()
            {
                RuleFor(e => e.idEquipoSeguridad).NotEmpty();
                RuleFor(e => e.marca).NotEmpty();
                RuleFor(e => e.modelo).NotEmpty();
                RuleFor(e => e.capacidad).NotEmpty();
                RuleFor(e => e.tipo).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<EjecutarActualizarEquipoSeguridad, EquipoSeguridadDTO>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<EquipoSeguridadDTO> Handle(EjecutarActualizarEquipoSeguridad request, CancellationToken cancellationToken)
            {
                // Buscar el registro a actualizar
                var equipoSeguridad = await _context.equiposSeguridad
                    .FirstOrDefaultAsync(e => e.idEquipoSeguridad == request.idEquipoSeguridad, cancellationToken);

                if (equipoSeguridad == null)
                {
                    throw new Exception("No se encontró el equipo de seguridad.");
                }

                // Actualizar los campos
                equipoSeguridad.marca = request.marca;
                equipoSeguridad.modelo = request.modelo;
                equipoSeguridad.capacidad = request.capacidad;
                equipoSeguridad.tipo = request.tipo;

                // Guardar los cambios
                var resultado = await _context.SaveChangesAsync(cancellationToken);
                if (resultado > 0)
                {
                    // Crear y retornar el DTO con la información actualizada
                    return new EquipoSeguridadDTO
                    {
                        idEquipoSeguridad = equipoSeguridad.idEquipoSeguridad,
                        marca = equipoSeguridad.marca,
                        modelo = equipoSeguridad.modelo,
                        capacidad = equipoSeguridad.capacidad,
                        tipo = equipoSeguridad.tipo
                    };
                }

                throw new Exception("No se pudo actualizar el equipo de seguridad.");
            }
        }
    }
}
