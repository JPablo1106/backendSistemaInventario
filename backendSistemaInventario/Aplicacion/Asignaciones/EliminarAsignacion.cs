using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Asignaciones
{
    public class EliminarAsignacion
    {
        // Comando para eliminar la asignación. Solo se requiere el ID de la asignación.
        public class EjecutarEliminarAsignacion : IRequest
        {
            public int idAsignacion { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutarEliminarAsignacion>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EjecutarEliminarAsignacion request, CancellationToken cancellationToken)
            {
                // Se busca la asignación existente, incluyendo sus detalles.
                var asignacion = await _context.asignaciones
                    .Include(a => a.detalleAsignaciones)
                    .FirstOrDefaultAsync(a => a.idAsignacion == request.idAsignacion, cancellationToken);

                if (asignacion == null)
                {
                    throw new Exception("La asignación no existe");
                }

                // Se eliminan los detalles asociados a la asignación.
                _context.detalleAsignaciones.RemoveRange(asignacion.detalleAsignaciones);

                // Se elimina la asignación principal.
                _context.asignaciones.Remove(asignacion);

                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar la asignación");
            }
        }
    }
}
