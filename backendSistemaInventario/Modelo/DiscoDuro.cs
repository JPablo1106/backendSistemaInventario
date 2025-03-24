using System.ComponentModel.DataAnnotations;

namespace backendSistemaInventario.Modelo
{
    public class DiscoDuro
    {
        [Key]
        public int idDiscoDuro { get; set; }

        [Required]
        public string marca { get; set; }

        [Required]
        public string modelo { get; set; }

        [Required]
        public int capacidad { get; set; }

        [Required]
        public int c {  get; set; }

        public int d { get; set; }

        public int e {  get; set; }
    }
}
