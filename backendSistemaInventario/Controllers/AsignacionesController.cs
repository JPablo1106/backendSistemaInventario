using backendSistemaInventario.Aplicacion.Asignaciones;
using backendSistemaInventario.Aplicacion.Servicios;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace backendSistemaInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionesController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly AsignacionesHelper _asignacionesHelper;

        public AsignacionesController(IMediator mediator)
        {
            _mediator = mediator;
            _asignacionesHelper = new AsignacionesHelper(mediator);

        }

        [HttpPost]
        [Route("RegistrarAsignacion")]
        public async Task<ActionResult<Unit>> Crear(RegistroAsignacion.EjecutarRegistroAsignacion data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet("ConsultarAsignacionId/{id}")]
        public async Task<ActionResult<AsignacionDTO>> ObtenerAsignacionPorId(int id)
        {
            var asignacion = await _mediator.Send(new ConsultarAsignacionPorId.Ejecutar(id));

            if (asignacion == null)
            {
                return NotFound(new { mensaje = "No se encontró la asignación con el ID especificado." });
            }

            return Ok(asignacion);
        }

        [HttpGet]
        [Route("ConsultarAsignacionesCompletas")]
        public async Task<IActionResult> ObtenerAsignacionesCompleta()
        {
            var asignaciones = await _mediator.Send(new ConsultaAsignaciones.Ejecutar());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);
            return Content(jsonResponse, "application/json");
        }

        [HttpPut]
        [Route("ActualizarAsignacion")]
        public async Task<ActionResult<Unit>> ActualizarAsignacion(ActualizarAsignacion.EjecutarEditarAsignacion data)
        {
            return await _mediator.Send(data);
        }

        [HttpDelete]
        [Route("EliminarAsignacion/{idAsignacion}")]
        public async Task<ActionResult<Unit>> EliminarAsignacion(int idAsignacion)
        {
            return await _mediator.Send(new EliminarAsignacion.EjecutarEliminarAsignacion
            {
                idAsignacion = idAsignacion
            });
        }



        [HttpGet]
        [Route("ConsultarAsignaciones")]
        public async Task<IActionResult> ObtenerAsignaciones([FromQuery] string tipo)
        {
            var asignaciones = await _asignacionesHelper.ObtenerAsignacionesAsync(tipo);
            if (asignaciones == null)
            {
                return BadRequest("Tipo de asignación no reconocido.");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);
            return Content(jsonResponse, "application/json");
        }

        [HttpGet]
        [Route("ConteoAsignaciones")]
        public async Task<IActionResult> ObtenerConteoAsignaciones()
        {
            var resultado = await _asignacionesHelper.ObtenerConteoAsignacionesAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string jsonResponse = JsonSerializer.Serialize(resultado, options);
            return Content(jsonResponse, "application/json");
        }
        /*
        [HttpGet]
        [Route("ConsultarAsignacionesEquipos")]
        public async Task<IActionResult> ObtenerAsignacionesEquipos()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesEquipos.EjecutarConsulta());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }

        [HttpGet]
        [Route("ConsultarAsignacionesMonitores")]
        public async Task<IActionResult> ObtenerAsignacionesMonitores()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesMonitores.EjecutarConsultaMonitores());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }

        [HttpGet]
        [Route("ConsultarAsignacionesTeclados")]
        public async Task<IActionResult> ObtenerAsignacionesTeclados()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesTeclados.EjecutarConsultaTeclados());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }

        [HttpGet]
        [Route("ConsultarAsignacionesTelefonos")]
        public async Task<IActionResult> ObtenerAsignacionesTelefonos()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesTelefonos.EjecutarConsultaTelefonos());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }

        [HttpGet]
        [Route("ConsultarAsignacionesMouse")]
        public async Task<IActionResult> ObtenerAsignacionesMouse()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesMouse.EjecutarConsultaMouse());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }

        [HttpGet]
        [Route("ConsultarAsignacionesEquiposSeguridad")]
        public async Task<IActionResult> ObtenerAsignacionesEquiposSeguridad()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesEquiposSeguridad.EjecutarConsultaEquiposSeguridad());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }


        [HttpGet]
        [Route("ConsultarAsignacionesDispositivosExt")]
        public async Task<IActionResult> ObtenerAsignacionesDispositivosExt()
        {
            var asignaciones = await _mediator.Send(new ConsultarAsignacionesDispositivosExt.EjecutarConsultaDispositivosExt());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            // Serializar la respuesta ignorando los campos no deseados
            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);

            return Content(jsonResponse, "application/json");
        }*/

    }
}
