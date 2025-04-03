using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace backendSistemaInventario.Aplicacion.Admin
{
    public class LogoutAdministrador
    {
        public class EjecutarLogout : IRequest<bool>
        {
            public string refreshToken { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutarLogout, bool>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<bool> Handle(EjecutarLogout request, CancellationToken cancellationToken)
            {
                var token = await _context.refreshToken
                    .FirstOrDefaultAsync(t => t.token == request.refreshToken);

                if (token == null)
                    throw new Exception("Token no encontrado");

                // Invalidar el token
                token.esValido = false;

                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
