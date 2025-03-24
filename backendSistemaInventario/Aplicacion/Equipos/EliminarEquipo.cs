using backendSistemaInventario.Persistencia;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Equipos
{
    public class EliminarEquipo
    {
        public class EjecutarEliminarEquipo : IRequest<string>
        {
            public int idEquipo { get; set; }
        }

    public class Manejador : IRequestHandler<EjecutarEliminarEquipo, string>
        {
            private readonly ContextoBD _context;
            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<string>Handle(EjecutarEliminarEquipo request, CancellationToken cancellationToken)
            {
                var equipo = await _context.equipos.FindAsync(request.idEquipo);

                if (equipo == null)
                {
                    throw new Exception("El equipo no existe");
                }

                _context.equipos.Remove(equipo);
                var resultado = await _context.SaveChangesAsync(cancellationToken);

                if (resultado > 0)
                {
                    return "Equipo eliminado correctamente";
                }

                throw new Exception("No se pudo eliminar el equipo");
            }
        }
    }
}
