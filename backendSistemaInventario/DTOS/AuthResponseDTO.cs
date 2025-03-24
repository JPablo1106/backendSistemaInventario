namespace backendSistemaInventario.DTOS
{
    public class AuthResponseDTO
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public DateTime expira { get; set; }

        // Datos del administrador
        public int idAdministrador { get; set; }
        public string nombreAdmin { get; set; }
        public string usuario { get; set; }
    }
}
