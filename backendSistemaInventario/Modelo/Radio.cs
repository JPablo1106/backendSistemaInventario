using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendSistemaInventario.Modelo
{
    public class Radio
    {
        [Key]
        public int idRadio {  get; set; }

        [Required]
        public string marca {  get; set; }

        [Required]
        public string modelo { get; set; }

        [Required]
        public string issi {  get; set; }

        [Required]
        public string numSerie { get; set; }

        // Nueva propiedad para indicar si cuenta con antena.
        [Required]
        public bool tieneAntena { get; set; }

        // Nueva propiedad para indicar si cuenta con clip.
        [Required]
        public bool tieneClip { get; set; }

        [Required]
        public int idUsuario { get; set; }

        [ForeignKey("idUsuario")]
        public Usuario usuario { get; set; }
    }
}
