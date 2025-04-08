using backendSistemaInventario.Aplicacion.Componentes;
using backendSistemaInventario.Aplicacion.EquiposSeguridad;
using backendSistemaInventario.DTOS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposSeguridadController : ControllerBase
    {

        private readonly IMediator _mediator;

        public EquiposSeguridadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarEquipoSeguridad")]
        public async Task<ActionResult<Unit>> Crear(RegistroEquipoSeguridad.EjecutarRegistroEquipoSeguridad data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        [Route("ConsultarEquiposSeguridad")]
        public async Task<ActionResult<List<EquipoSeguridadDTO>>> GetEquiposSeguridad()
        {
            return await _mediator.Send(new ConsultarEquiposSeguridad.ListaEquiposSeguridad());
        }

        [HttpPut]
        [Route("ActualizarEquipoSeguridad")]
        public async Task<ActionResult<EquipoSeguridadDTO>> ActualizarEquipoSeguridad(ActualizarEquipoSeguridad.EjecutarActualizarEquipoSeguridad data)
        {
            return await _mediator.Send(data);
        }

        [HttpDelete]
        [Route("EliminarEquipoSeguridad")]
        public async Task<IActionResult> EliminarEquipoSeguridad([FromBody] EliminarEquipoSeguridad.EjecutarEliminarEquipoSeguridad request)
        {
            await _mediator.Send(request);
            return Ok(new { mensaje = "Equipo de seguridad eliminado correctamente" });
        }
    }
}
