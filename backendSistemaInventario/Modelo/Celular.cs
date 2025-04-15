using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendSistemaInventario.Modelo
{
    public class Celular
    {
        [Key]
        public int idCelular { get; set; }

        [Required]
        public string marca { get; set; }

        [Required]
        public string modelo { get; set; }

        [Required]
        public string compania { get; set; }

        [Required]
        public string numSerie { get; set;}

        [Required]
        public string imei { get; set; }

        [Required]
        public string numCelular { get; set;}

        [Required]
        public int idUsuario { get; set; }

        [ForeignKey("idUsuario")]
        public Usuario usuario { get; set; }
    }
}
