using System.ComponentModel.DataAnnotations;

namespace backendSistemaInventario.Modelo
{
    public class EquipoSeguridad
    {
        [Key]
        public int idEquipoSeguridad { get; set; }

        [Required]
        public string marca { get; set; }

        [Required]
        public string modelo { get; set; }

        [Required]
        public string capacidad { get; set; }

        [Required]
        public string tipo { get; set; }
    }
}
