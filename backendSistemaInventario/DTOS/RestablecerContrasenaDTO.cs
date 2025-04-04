namespace backendSistemaInventario.DTOS
{
    public class RestablecerContrasenaDTO
    {
        public string token { get; set; }
        public string nuevaContrasena { get; set; }
    }

    public class RestablecerContrasenaRespuestaDTO
    {
        public string mensaje { get; set; }
    }
}
