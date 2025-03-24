namespace backendSistemaInventario.DTOS
{
    public class AsignacionDTO
    {
        public int idAsignacion { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public int idUsuario { get; set; }
        public UsuariosDTO usuario { get; set; }

        // Nueva lista para múltiples componentes asignados
        public List<DetalleAsignacionDTO>? detallesAsignacion { get; set; }
    }
}
