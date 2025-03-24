using backendSistemaInventario.Modelo;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Asignacion
{
    [Key]
    public int idAsignacion { get; set; }

    [Required]
    public DateTime fechaAsignacion { get; set; }

    [Required]
    public int idUsuario { get; set; }

    [ForeignKey("idUsuario")]
    public Usuario usuario { get; set; }

    // Relación con DetalleAsignacion para manejar múltiples componentes
    public List<DetalleAsignacion> detallesAsignacion { get; set; } = new List<DetalleAsignacion>();
}
