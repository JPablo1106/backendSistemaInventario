namespace backendSistemaInventario.DTOS
{
    public class ComponenteDTO
    {
        public int idComponente {  get; set; }
        public string tipoComponente { get; set; }
        public string marcaComponente { get; set; }
        public string? modeloMonitor { get; set; }
        public string? modeloTelefono { get; set; }
        public string? idiomaTeclado { get; set; }

    }
}
