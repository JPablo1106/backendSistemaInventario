namespace backendSistemaInventario.DTOS
{
    public class TabletaDTO
    {

        public int idTableta {  get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string numSerie { get; set; }
        public string accesorios { get; set; }
        public int idUsuario { get; set; }
        public UsuariosDTO usuario { get; set; }
    }
}
