using backendSistemaInventario.Aplicacion.EquiposSeguridad;
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
    }
}
