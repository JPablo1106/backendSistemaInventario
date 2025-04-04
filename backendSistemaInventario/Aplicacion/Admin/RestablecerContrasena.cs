using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Admin
{
    public class RestablecerContrasena
    {
        public class Ejecutar : IRequest<RestablecerContrasenaRespuestaDTO>
        {
            public string token { get; set; }
            public string nuevaContrasena { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecutar, RestablecerContrasenaRespuestaDTO>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<RestablecerContrasenaRespuestaDTO> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var resetToken = await _context.passwordResetTokens
                    .FirstOrDefaultAsync(t => t.token == request.token && t.esValido);

                if (resetToken == null)
                    throw new Exception("Token inválido o ya usado");

                if (resetToken.expira < DateTime.UtcNow)
                    throw new Exception("Token expirado");

                var admin = await _context.administrador
                    .FirstOrDefaultAsync(a => a.idAdministrador == resetToken.administradorId);

                if (admin == null)
                    throw new Exception("Administrador no encontrado");

                admin.contraseña = request.nuevaContrasena;
                resetToken.esValido = false;

                await _context.SaveChangesAsync();

                return new RestablecerContrasenaRespuestaDTO
                {
                    mensaje = "Contraseña restablecida correctamente"
                };
            }
        }
    }
}
