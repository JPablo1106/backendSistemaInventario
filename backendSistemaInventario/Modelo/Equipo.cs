using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendSistemaInventario.Modelo
{
    public class Equipo
    {
        [Key]
        public int idEquipo { get; set; }

        [Required]
        [MaxLength(255)]
        public string marca { get; set; }

        [Required]
        [MaxLength(255)]
        public string modelo { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string tipoEquipo { get; set; }

        [Required]
        [MaxLength(255)]
        public string velocidadProcesador { get; set; }

        [Required]
        [MaxLength(255)]
        public string tipoProcesador { get; set; }

        [Required]
        [MaxLength(255)]
        public string memoriaRam { get; set; }

        [Required]
        [MaxLength(255)]
        public string tipoMemoriaRam { get; set; }

        [Required]
        public int idDiscoDuro { get; set; }

        [ForeignKey("idDiscoDuro")]
        public DiscoDuro discoDuro { get; set; }
    }
}
