using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.EquiposSeguridad
{
    public class EliminarEquipoSeguridad
    {
        public class EjecutarEliminarEquipoSeguridad : IRequest
        {
            public int id { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutarEliminarEquipoSeguridad>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEliminarEquipoSeguridad request, CancellationToken cancellationToken)
            {
                // Buscar el equipo a eliminar
                var equipoSeguridad = await _context.equiposSeguridad.FirstOrDefaultAsync(e => e.idEquipoSeguridad == request.id, cancellationToken);

                if (equipoSeguridad == null)
                {
                    throw new Exception("No se encontró el equipo de seguridad.");
                }

                _context.equiposSeguridad.Remove(equipoSeguridad);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el equipo de seguridad.");
            }
        }
    }
}
