namespace backendSistemaInventario.DTOS
{
    public class RecuperarContrasenaDTO
    {
        public string usuario { get; set; }
    }

    public class RecuperarContrasenaRespuestaDTO
    {
        public string mensaje { get; set; }
        public string token { get; set; }
        public DateTime expiracion { get; set; }
    }
}

