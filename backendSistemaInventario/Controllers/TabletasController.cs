using backendSistemaInventario.Aplicacion.Tabletas;
using backendSistemaInventario.DTOS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabletasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TabletasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarTableta")]
        public async Task<ActionResult<Unit>>Crear(RegistroTableta.EjecutarRegistroTabletas data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        [Route("ConsultarTabletas")]
        public async Task<ActionResult<List<TabletaDTO>>> ObtenerTabletas()
        {
            return await _mediator.Send(new ConsultarTabletas.ListaTabletas());
        }

        [HttpGet]
        [Route("ConsultarTabletaPorId/{idTableta}")]
        public async Task<ActionResult<TabletaDTO>>ObtenerTabletaPorId(int idTableta)
        {
            var tableta = await _mediator.Send(new ConsultaTabletaPorId.EjecutaConsultaTabletaPorId { idTableta = idTableta});
            if(tableta == null)
            {
                return NotFound(new {mensaje = "No se encontró la tableta con el ID especificado."});
            }
            return Ok(tableta);
        }

        [HttpPut]
        [Route("ActualizarTableta")]
        public async Task<ActionResult<TabletaDTO>>ActualizarTableta(ActualizarTableta.EjecutarActualizarTableta data)
        {
            return await _mediator.Send(data);
        }

        [HttpDelete]
        [Route("EliminarTableta")]
        public async Task<IActionResult> EliminarTableta([FromBody] EliminarTableta.EjecutarEliminarTableta request)
        {
            await _mediator.Send(request);
            return Ok(new { mensaje = "Tableta eliminada correctamente" });
        }
    }
}
