using System.ComponentModel.DataAnnotations;

namespace backendSistemaInventario.Modelo
{
    public class Usuario
    {
        [Key]
        public int idUsuario {  get; set; }

        [Required]
        [MaxLength(255)]
        public string nombreUsuario { get; set; }

        [Required]
        [MaxLength(255)]
        public string area {  get; set; }

        [Required]
        [MaxLength(255)]
        public string departamento { get; set; }

    }
}
