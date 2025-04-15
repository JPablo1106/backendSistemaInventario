namespace backendSistemaInventario.DTOS
{
    public class RadioDTO
    {
        public int idRadio {  get; set; }
        public string marca { get; set; }                  
        public string modelo { get; set; }
        public string issi { get; set; }
        public string numSerie {  get; set; }
        public bool tieneAntena { get; set; }
        public bool tieneClip { get; set; }
        public int idUsuario { get; set; }
        public UsuariosDTO usuario { get; set; }
    }
}
