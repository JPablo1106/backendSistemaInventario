using backendSistemaInventario.Aplicacion.Admin;
using backendSistemaInventario.DTOS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AdministradorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdministradorController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [Route("Registrarse")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> Crear(RegistroAdministrador.EjecutarRegistroAdministrador data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var resultado = await _mediator.Send(new LoginAdministrador.EjecutarLogin
                {
                    usuario = loginDto.usuario,
                    contraseña = loginDto.contraseña
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDTO tokenDto)
        {
            try
            {
                var resultado = await _mediator.Send(new RefreshTokenAdministrador.Ejecuta
                {
                    token = tokenDto.token,
                    refreshToken = tokenDto.refreshToken
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}

