// Modelo/PasswordResetToken.cs
namespace backendSistemaInventario.Modelo
{
    public class PasswordResetToken
    {
        public int id { get; set; }
        public string token { get; set; }
        public DateTime expira { get; set; }
        public bool esValido { get; set; }
        public int administradorId { get; set; }
    }
}
