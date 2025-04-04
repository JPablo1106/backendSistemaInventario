using backendSistemaInventario.Modelo;
using Microsoft.EntityFrameworkCore;

namespace backendSistemaInventario.Persistencia
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        public DbSet<Administrador> administrador {  get; set; }

        public DbSet<RefreshToken> refreshToken {  get; set; }

        public DbSet<PasswordResetToken> passwordResetTokens { get; set; }

        public DbSet<Usuario> usuarios { get; set; }

        public DbSet<Equipo> equipos { get; set; }

        public DbSet<DiscoDuro> discoDuro { get; set;}
        
        public DbSet<Componente> componentes { get; set; }

        public DbSet<Monitores> monitores { get; set; }

        public DbSet<Teclado> teclados { get; set; }

        public DbSet<Mouse> mouse { get; set; }

        public DbSet<Telefono> telefonos { get; set; }

        public DbSet<EquipoSeguridad> equiposSeguridad { get; set; }

        public DbSet<DispositivoExt> dispositivosExt { get; set; }

        public DbSet<Asignacion> asignaciones { get; set; }

        public DbSet<DetalleAsignacion> detalleAsignaciones { get; set; }
    }
}
