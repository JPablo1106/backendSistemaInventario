using backendSistemaInventario.Aplicacion.Asignaciones;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AsignacionesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegistrarAsignacion")]
        public async Task<ActionResult<Unit>> Crear(RegistroAsignacion.EjecutarRegistroAsignacion data)
        {
            return await _mediator.Send(data);  
        }
    }
}
