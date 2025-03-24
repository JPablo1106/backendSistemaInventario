using backendSistemaInventario.DTOS;
using backendSistemaInventario.Helpers;
using backendSistemaInventario.Modelo;
using backendSistemaInventario.Persistencia;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace backendSistemaInventario.Aplicacion.Admin
{
    public class LoginAdministrador
    {
        public class EjecutarLogin : IRequest<AuthResponseDTO>
        {
            public string usuario { get; set; }
            public string contraseña { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutarLogin, AuthResponseDTO>
        {
            private readonly ContextoBD _context;
            private readonly JwtGenerator _jwtGenerator;

            public Manejador(ContextoBD context, JwtGenerator jwtGenerator)
            {
                _context = context;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<AuthResponseDTO> Handle(EjecutarLogin request, CancellationToken cancellationToken)
            {
                // Buscar al administrador por usuario
                var admin = await _context.administrador
                    .FirstOrDefaultAsync(a => a.usuario == request.usuario);

                if (admin == null)
                    throw new Exception("Usuario incorrecto");

                // Validar contraseña
                if (admin.contraseña != request.contraseña)
                    throw new Exception("Contraseña incorrecta");

                // Generar el JWT
                var token = _jwtGenerator.GenerateJwtToken(admin);

                // Generar la cadena del Refresh Token
                var refreshTokenString = _jwtGenerator.GenerateRefreshToken();

                // Crear el objeto RefreshToken usando el modelo
                var newRefreshToken = new RefreshToken
                {
                    token = refreshTokenString,
                    expira = DateTime.UtcNow.AddDays(7), // Establece la expiración deseada
                    esValido = true,
                    administradorId = admin.idAdministrador
                };

                // Guardar el Refresh Token en la base de datos
                _context.refreshToken.Add(newRefreshToken);
                await _context.SaveChangesAsync();

                // Devolver respuesta
                return new AuthResponseDTO
                {
                    token = token,
                    refreshToken = newRefreshToken.token,
                    expira = newRefreshToken.expira,
                    idAdministrador = admin.idAdministrador,
                    nombreAdmin = admin.nombreAdmin,
                    usuario = admin.usuario
                };
            }
        }
    }
}
