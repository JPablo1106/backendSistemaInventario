using backendSistemaInventario.Aplicacion.Usuarios;
using backendSistemaInventario.DTOS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarUsuario")]
        public async Task<ActionResult<Unit>> Crear(RegistroUsuarios.EjecutarRegistroUsuario data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        [Route("ConsultarUsuarios")]
        public async Task<ActionResult<List<UsuariosDTO>>> GetUsuarios()
        {
            return await _mediator.Send(new ConsultaUsuarios.ListaUsuarios());
        }

        [HttpGet("ConsultarUsuarioPorId/{idUsuario}")]
        public async Task<ActionResult<UsuariosDTO>> ObtenerUsuarioPorId(int idUsuario)
        {
            var usuario = await _mediator.Send(new ConsultaUsuarioPorId.Ejecuta { idUsuario = idUsuario });

            if (usuario == null)
            {
                return NotFound(new { mensaje = "No se encontró el usuario con el ID especificado." });
            }

            return Ok(usuario);
        }


        [HttpPut]
        [Route("ActualizarUsuario")]
        public async Task<ActionResult<UsuariosDTO>> ActualizarUsuario(ActualizarUsuario.EjecutarActualizarUsuario data)
        {
            return await _mediator.Send(data);
        }

        [HttpDelete]
        [Route("EliminarUsuario")]
        public async Task<IActionResult> EliminarUsuario([FromBody] EliminarUsuario.EjecutarEliminarUsuario request)
        {
            await _mediator.Send(request);
            return Ok(new {mensaje = "Usuario eliminado correctamente"});
        }

    }
}
