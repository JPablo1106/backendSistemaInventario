using backendSistemaInventario.Aplicacion.Celulares;
using backendSistemaInventario.Modelo;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CelularesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CelularesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarCelular")]
        public async Task<ActionResult<Unit>> Crear(RegistroCelular.EjecutarRegistroCelular data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        [Route("ConsultarCelulares")]
        public async Task<ActionResult<List<CelularDTO>>> ObtenerCelulares()
        {
            return await _mediator.Send(new ConsultaCelulares.ListaCelulares());
        }

        [HttpGet]
        [Route("ConsultarCelularPorId/{idCelular}")]
        public async Task<ActionResult<CelularDTO>>ObtenerCelularPorId(int idCelular)
        {
            var celular = await _mediator.Send(new ConsultaCelularPorId.EjecutaConsultaCelularPorId { idCelular = idCelular });
            if (celular == null)
            {
                return NotFound(new {mensaje = "No se encontró el celular con el ID especificado. "});
            }
            return Ok(celular);
        }


        [HttpPut]
        [Route("ActualizarCelular")]
        public async Task<ActionResult<CelularDTO>>ActualizarCelular(ActualizarCelular.EjecutarActualizarCelular data)
        {
            return await _mediator.Send(data);
        }

        [HttpDelete]
        [Route("EliminarCelular")]
        public async Task<IActionResult> EliminarCelular([FromBody] EliminarCelular.EjecutarEliminarCelular request)
        {
            await _mediator.Send(request);
            return Ok(new { mensaje = "Celular eliminado correctamenre" });
        }
    }
}
