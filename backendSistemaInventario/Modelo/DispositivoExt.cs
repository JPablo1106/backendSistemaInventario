using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendSistemaInventario.Modelo
{
    public class DispositivoExt
    {
        [Key]
        public int idDispExt { get; set; }

        [Required]
        public string marca { get; set; }

        [Required]
        public string descripcion { get; set; }
    }
}
