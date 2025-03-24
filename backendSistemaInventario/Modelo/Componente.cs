using System.ComponentModel.DataAnnotations;

namespace backendSistemaInventario.Modelo
{
    public class Componente
    {
        [Key]
        public int idComponente { get; set; }

        [Required]
        public string tipoComponente { get; set; }

        [Required]
        public string marcaComponente { get; set; }
    }
}
