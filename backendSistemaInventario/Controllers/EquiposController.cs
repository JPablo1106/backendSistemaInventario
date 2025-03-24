using backendSistemaInventario.Aplicacion.Equipos;
using backendSistemaInventario.DTOS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backendSistemaInventario.Controllers
{
    [Route("api/equipos")]
    [ApiController]
    [Authorize]
    public class EquiposController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EquiposController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarEquipo")]
        public async Task<ActionResult<EquipoDTO>> RegistrarEquipo(RegistroEquipos.EjecutarRegistroEquipo data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        [Route("ConsultarEquipos")]
        
        public async Task<ActionResult<List<EquipoDTO>>> GetEquipos()
        {
            return await _mediator.Send(new ConsultarEquipos.ListaEquipos());
        }
       
        [HttpPut]
        [Route("ActualizarEquipo")]
        public async Task<ActionResult<EquipoDTO>> ActualizarEquipo(ActualizarEquipo.EjecutarActualizarEquipo data)
        {
            return await _mediator.Send(data);
        }

        [HttpDelete]
        [Route("EliminarEquipo")]
        public async Task<IActionResult> EliminarEquipo([FromBody] EliminarEquipo.EjecutarEliminarEquipo request)
        {
            await _mediator.Send(request);
            return Ok(new { mensaje = "Equipo eliminado correctamente" });
        }
    }
}
