using System.ComponentModel.DataAnnotations;

namespace backendSistemaInventario.Modelo
{
    public class RefreshToken
    {
        [Key]
        public int id { get; set; }
        public string token { get; set; }
        public DateTime expira { get; set; }
        public bool esValido { get; set; } = true;

        // Relación con Administrador
        public int administradorId { get; set; }
        public Administrador administrador { get; set; }
    }
}
