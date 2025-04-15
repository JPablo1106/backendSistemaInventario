using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendSistemaInventario.Modelo
{
    public class Tableta
    {
        [Key]
        public int idTableta { get; set; }

        [Required]
        public string marca { get; set; }

        [Required]
        public string modelo { get; set; }

        [Required]
        public string numSerie { get; set; }

        public string accesorios { get; set; }

        [Required]
        public int idUsuario { get; set; }

        [ForeignKey("idUsuario")]
        public Usuario usuario { get; set; }
    }
}
