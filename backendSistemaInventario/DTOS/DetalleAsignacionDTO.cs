namespace backendSistemaInventario.DTOS
{
    public class DetalleAsignacionDTO
    {
        public int idDetalleAsignacion { get; set; }
        public int idAsignacion { get; set; }
        public AsignacionDTO asignacion { get; set; }
        public int? idEquipo { get; set; }
        public EquipoDTO? equipo { get; set; }
        public string? numSerieEquipo { get; set; }
        public string? ipAddress { get; set; }
        public string? ipCpuRed { get; set; }
        public int? idComponente { get; set; }
        public ComponenteDTO? componente { get; set; }
        public string? numSerieComponente { get; set; }

        public int? idDispExt { get; set; } // Nuevo campo para dispositivos externos
        public DispositivoExtDTO? dispositivoExt { get; set; } // Referencia al objeto dispositivo externo
        public string? numSerieDispExt { get; set; } // Número de serie del dispositivo externo

        public int? idEquipoSeguridad {  get; set; }
        public EquipoSeguridadDTO? equipoSeguridad { get; set; }
        public string? numSerieEquipoSeg {  get; set; }

    }
}
