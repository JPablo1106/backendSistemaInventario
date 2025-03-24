namespace backendSistemaInventario.DTOS
{
    public class EquipoDTO
    {
        public int idEquipo {  get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string tipoEquipo { get; set; }
        public string velocidadProcesador {  get; set; }
        public string tipoProcesador { get; set; }
        public string memoriaRam {  get; set; }
        public string tipoMemoriaRam { get; set; }

        public int idDiscoDuro { get; set; }
        public DiscoDuroDTO discoDuro { get; set; }
    }
}
