using backendSistemaInventario.DTOS;
using backendSistemaInventario.Helpers;
using backendSistemaInventario.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Aplicacion.Admin
{
    public class RefreshTokenAdministrador
    {
        public class Ejecuta : IRequest<AuthResponseDTO>
        {
            public string token { get; set; }
            public string refreshToken { get; set; }

        }
        public class Manejador : IRequestHandler<Ejecuta, AuthResponseDTO>
        {
            private readonly ContextoBD _context;
            private readonly JwtGenerator _jwtGenerator;

            public Manejador(ContextoBD context, JwtGenerator jwtGenerator)
            {
                _context = context;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<AuthResponseDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Se busca la entidad RefreshToken, incluyendo la relacion con el administrador
                var refreshTokenEntity = await _context.refreshToken
                    .Include(rt => rt.administrador)
                    .FirstOrDefaultAsync(rt => rt.token == request.refreshToken);

                //Se verifica que exista y que no esté espirado o marcado como invalido
                if (refreshTokenEntity == null || refreshTokenEntity.expira <= DateTime.UtcNow || refreshTokenEntity.esValido)
                    throw new Exception("Token de actualizacion no válido o expirado");

                //Se obtiene el administrador relacionado
                var administrador = refreshTokenEntity.administrador;

                //Se genera nuevo token
                var newJwtToken = _jwtGenerator.GenerateJwtToken(administrador);
                var newRefreshToken = _jwtGenerator.GenerateRefreshToken();

                //Se actualiza la entidad del refreshToken en la base de datos
                refreshTokenEntity.token = newJwtToken;
                refreshTokenEntity.expira = DateTime.UtcNow.AddDays(7);

                _context.refreshToken.Update(refreshTokenEntity);
                await _context.SaveChangesAsync();

                return new AuthResponseDTO
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken,
                    nombreAdmin = administrador.nombreAdmin
                };
            }
        }
    }
}
