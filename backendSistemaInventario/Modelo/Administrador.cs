using System.ComponentModel.DataAnnotations;

namespace backendSistemaInventario.Modelo
{
    public class Administrador
    {
        [Key]
        public int idAdministrador { get; set; }

        [Required]
        [MaxLength(255)]
        public string nombreAdmin { get; set; }

        [Required]
        [EmailAddress]
        public string usuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string contraseña { get; set; }
    }
}
