using backendSistemaInventario.Aplicacion.Asignaciones;
using MediatR;

namespace backendSistemaInventario.Helpers
{
    public class AsignacionesHelper
    {
        private readonly IMediator _mediator;

        public AsignacionesHelper(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<object> ObtenerConteoAsignacionesAsync()
        {
            var conteoEquipos = await _mediator.Send(new ConsultarAsignacionesEquipos.EjecutarConsulta());
            var conteoMonitores = await _mediator.Send(new ConsultarAsignacionesMonitores.EjecutarConsultaMonitores());
            var conteoTeclados = await _mediator.Send(new ConsultarAsignacionesTeclados.EjecutarConsultaTeclados());
            var conteoMouse = await _mediator.Send(new ConsultarAsignacionesMouse.EjecutarConsultaMouse());
            var conteoTelefonos = await _mediator.Send(new ConsultarAsignacionesTelefonos.EjecutarConsultaTelefonos());
            var conteoEquiposSeg = await _mediator.Send(new ConsultarAsignacionesEquiposSeguridad.EjecutarConsultaEquiposSeguridad());
            var conteoDispositivosExt = await _mediator.Send(new ConsultarAsignacionesDispositivosExt.EjecutarConsultaDispositivosExt());

            var resultado = new
            {
                Equipos = conteoEquipos.Count,
                Monitores = conteoMonitores.Count,
                Teclados = conteoTeclados.Count,
                Mouse = conteoMouse.Count,
                Telefonos = conteoTelefonos.Count,
                EquiposSeguridad = conteoEquiposSeg.Count,
                DispositivosExternos = conteoDispositivosExt.Count
            };

            return resultado;
        }

        public async Task<object> ObtenerAsignacionesAsync(string tipo)
        {
            object consulta = tipo.ToLower() switch
            {
                "equipos" => new ConsultarAsignacionesEquipos.EjecutarConsulta(),
                "monitores" => new ConsultarAsignacionesMonitores.EjecutarConsultaMonitores(),
                "teclados" => new ConsultarAsignacionesTeclados.EjecutarConsultaTeclados(),
                "mouse" => new ConsultarAsignacionesMouse.EjecutarConsultaMouse(),
                "telefonos" => new ConsultarAsignacionesTelefonos.EjecutarConsultaTelefonos(),
                "seguridad" => new ConsultarAsignacionesEquiposSeguridad.EjecutarConsultaEquiposSeguridad(),
                "externos" => new ConsultarAsignacionesDispositivosExt.EjecutarConsultaDispositivosExt(),
                _ => null
            };

            if (consulta == null)
            {
                return null;
            }

            var asignaciones = await _mediator.Send(consulta);
            return asignaciones;
        }

    }

}
