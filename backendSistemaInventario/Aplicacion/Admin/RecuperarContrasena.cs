using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Admin
{
    public class RecuperarContrasena
    {
        public class Ejecutar : IRequest<RecuperarContrasenaRespuestaDTO>
        {
            public string usuario { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecutar, RecuperarContrasenaRespuestaDTO>
        {
            private readonly ContextoBD _context;

            public Manejador(ContextoBD context)
            {
                _context = context;
            }

            public async Task<RecuperarContrasenaRespuestaDTO> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var admin = await _context.administrador
                    .FirstOrDefaultAsync(a => a.usuario == request.usuario);

                if (admin == null)
                    throw new Exception("Usuario no encontrado");

                string token = Guid.NewGuid().ToString();
                DateTime expiracion = DateTime.UtcNow.AddHours(1);

                var resetToken = new PasswordResetToken
                {
                    token = token,
                    expira = expiracion,
                    esValido = true,
                    administradorId = admin.idAdministrador
                };

                _context.passwordResetTokens.Add(resetToken);
                await _context.SaveChangesAsync();

                return new RecuperarContrasenaRespuestaDTO
                {
                    mensaje = "Token generado correctamente",
                    token = token,
                    expiracion = expiracion
                };
            }
        }
    }
}
