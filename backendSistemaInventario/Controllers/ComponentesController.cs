using backendSistemaInventario.Aplicacion.Componentes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComponentesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComponentesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarComponente")]
        public async Task<ActionResult<Unit>> Crear(RegistroComponente.EjecutarRegistroComponente data)
        {
            return await _mediator.Send(data);
        }

        [HttpPut]
        [Route("ActualizarComponente")]
        public async Task<ActionResult<Unit>> ActualizarComponente(ActualizarComponente.EjecutarActualizarComponente data)
        {
            return await (_mediator.Send(data));
        }
    }
}
