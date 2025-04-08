using backendSistemaInventario.Aplicacion.Asignaciones;
using backendSistemaInventario.Aplicacion.Servicios;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> ConsultarAsignaciones(string tipo)
        {
            var asignaciones = await _asignacionesHelper.ObtenerAsignacionesAsync(tipo);

            if (asignaciones == null)
            {
                return NotFound(new { mensaje = $"No se encontraron asignaciones para el tipo '{tipo}'." });
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string jsonResponse = JsonSerializer.Serialize(asignaciones, options);
            return Content(jsonResponse, "application/json");
        }



        // Endpoint para consultar asignaciones con filtros por número de serie, marca y/o modelo
        [HttpGet]
        [Route("ConsultarAsignacionesBusqueda")]
        public async Task<IActionResult> ConsultarAsignacionesBusqueda(
            [FromQuery] string? numeroSerie,
            [FromQuery] string? marca,
            [FromQuery] string? modelo)
        {
            var request = new ConsultaAsignacionBusqueda.Ejecutar
            {
                NumeroSerie = numeroSerie,
                Marca = marca,
                Modelo = modelo
            };

            var asignaciones = await _mediator.Send(request);

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
    }
}
