using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendSistemaInventario.Modelo
{
    public class DetalleAsignacion
    {
        [Key]
        public int idDetalleAsignacion { get; set; }
        public int idAsignacion { get; set; }
        public int? idEquipo { get; set; }
        public string? numSerieEquipo { get; set; }
        public string? ipAddress { get; set; }
        public string? ipCpuRed { get; set; }

        public int? idComponente { get; set; }
        public string? numSerieComponente { get; set; }

        public int? idDispExt { get; set; } // Nuevo campo para dispositivos externos
        public string? numSerieDispExt { get; set; } // Número de serie del dispositivo externo

        public int? idEquipoSeguridad { get; set; }
        public string? numSerieEquipoSeg {  get; set; }

        [ForeignKey("idAsignacion")]
        public Asignacion asignacion { get; set; }

        [ForeignKey("idEquipo")]
        public Equipo equipo { get; set; }

        [ForeignKey("idComponente")]
        public Componente? componente { get; set; }

        [ForeignKey("idDispExt")]
        public DispositivoExt? dispositivoExt { get; set; }

        [ForeignKey("idEquipoSeguridad")]
        public EquipoSeguridad? equipoSeguridad { get; set; }
    }
}
