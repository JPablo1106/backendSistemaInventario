using backendSistemaInventario.Aplicacion.Radios;
using backendSistemaInventario.DTOS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RadiosController : ControllerBase
    {

        private readonly IMediator _mediator;

        public RadiosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarRadio")]
        public async Task<ActionResult<Unit>>Crear(RegistroRadios.EjecutarRegistroRadios data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        [Route("ConsultarRadios")]
        public async Task<ActionResult<List<RadioDTO>>> ObtenerRadios()
        {
            return await _mediator.Send(new ConsultarRadios.ListaRadios());
        }

        [HttpGet]
        [Route("ConsultarRadioPorId")]
        public async Task<ActionResult<RadioDTO>>ObtenerRadioPorId(int idRadio)
        {
            var radio = await _mediator.Send(new ConsultarRadioPorId.EjecutaConsultaRadioPorId { idRadio = idRadio });
            if(radio == null)
            {
                return NotFound(new { mensaje = "No se encontró el radio con el ID especificado." });
            }
            return Ok(radio);
        }

        [HttpDelete]
        [Route("EliminarRadio")]
        public async Task<IActionResult> EliminarRadio([FromBody] EliminarRadio.EjecutarEliminarRadio request)
        {
            await _mediator.Send(request);
            return Ok(new {mensaje = "Radio eliminado correctamente"});
        }
    }
}
