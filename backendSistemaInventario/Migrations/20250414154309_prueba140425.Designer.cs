﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backendSistemaInventario.Persistencia;

#nullable disable

namespace backendSistemaInventario.Migrations
{
    [DbContext(typeof(ContextoBD))]
    [Migration("20250414154309_prueba140425")]
    partial class prueba140425
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Asignacion", b =>
                {
                    b.Property<int>("idAsignacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idAsignacion"));

                    b.Property<DateTime>("fechaAsignacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.HasKey("idAsignacion");

                    b.HasIndex("idUsuario");

                    b.ToTable("asignaciones");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Administrador", b =>
                {
                    b.Property<int>("idAdministrador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idAdministrador"));

                    b.Property<string>("contraseña")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("nombreAdmin")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("usuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idAdministrador");

                    b.ToTable("administrador");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Componente", b =>
                {
                    b.Property<int>("idComponente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idComponente"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("marcaComponente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tipoComponente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idComponente");

                    b.ToTable("componentes");

                    b.HasDiscriminator().HasValue("Componente");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.DetalleAsignacion", b =>
                {
                    b.Property<int>("idDetalleAsignacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDetalleAsignacion"));

                    b.Property<int>("idAsignacion")
                        .HasColumnType("int");

                    b.Property<int?>("idComponente")
                        .HasColumnType("int");

                    b.Property<int?>("idDispExt")
                        .HasColumnType("int");

                    b.Property<int?>("idEquipo")
                        .HasColumnType("int");

                    b.Property<int?>("idEquipoSeguridad")
                        .HasColumnType("int");

                    b.Property<string>("ipAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ipCpuRed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numSerieComponente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numSerieDispExt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numSerieEquipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numSerieEquipoSeg")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idDetalleAsignacion");

                    b.HasIndex("idAsignacion");

                    b.HasIndex("idComponente");

                    b.HasIndex("idDispExt");

                    b.HasIndex("idEquipo");

                    b.HasIndex("idEquipoSeguridad");

                    b.ToTable("detalleAsignaciones");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.DiscoDuro", b =>
                {
                    b.Property<int>("idDiscoDuro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDiscoDuro"));

                    b.Property<int>("c")
                        .HasColumnType("int");

                    b.Property<int>("capacidad")
                        .HasColumnType("int");

                    b.Property<int>("d")
                        .HasColumnType("int");

                    b.Property<int>("e")
                        .HasColumnType("int");

                    b.Property<string>("marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("modelo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idDiscoDuro");

                    b.ToTable("discoDuro");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.DispositivoExt", b =>
                {
                    b.Property<int>("idDispExt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDispExt"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idDispExt");

                    b.ToTable("dispositivosExt");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Equipo", b =>
                {
                    b.Property<int>("idEquipo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idEquipo"));

                    b.Property<int>("idDiscoDuro")
                        .HasColumnType("int");

                    b.Property<string>("marca")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("memoriaRam")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("modelo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("tipoEquipo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("tipoMemoriaRam")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("tipoProcesador")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("velocidadProcesador")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("idEquipo");

                    b.HasIndex("idDiscoDuro");

                    b.ToTable("equipos");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.EquipoSeguridad", b =>
                {
                    b.Property<int>("idEquipoSeguridad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idEquipoSeguridad"));

                    b.Property<string>("capacidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("modelo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idEquipoSeguridad");

                    b.ToTable("equiposSeguridad");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.PasswordResetToken", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("administradorId")
                        .HasColumnType("int");

                    b.Property<bool>("esValido")
                        .HasColumnType("bit");

                    b.Property<DateTime>("expira")
                        .HasColumnType("datetime2");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("passwordResetTokens");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.RefreshToken", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("administradorId")
                        .HasColumnType("int");

                    b.Property<bool>("esValido")
                        .HasColumnType("bit");

                    b.Property<DateTime>("expira")
                        .HasColumnType("datetime2");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("administradorId");

                    b.ToTable("refreshToken");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Usuario", b =>
                {
                    b.Property<int>("idUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idUsuario"));

                    b.Property<string>("area")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("departamento")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("nombreUsuario")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("idUsuario");

                    b.ToTable("usuarios");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Monitores", b =>
                {
                    b.HasBaseType("backendSistemaInventario.Modelo.Componente");

                    b.Property<string>("modeloMonitor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Monitores");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Mouse", b =>
                {
                    b.HasBaseType("backendSistemaInventario.Modelo.Componente");

                    b.HasDiscriminator().HasValue("Mouse");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Teclado", b =>
                {
                    b.HasBaseType("backendSistemaInventario.Modelo.Componente");

                    b.Property<string>("idiomaTeclado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Teclado");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Telefono", b =>
                {
                    b.HasBaseType("backendSistemaInventario.Modelo.Componente");

                    b.Property<string>("modeloTelefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Telefono");
                });

            modelBuilder.Entity("Asignacion", b =>
                {
                    b.HasOne("backendSistemaInventario.Modelo.Usuario", "usuario")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("usuario");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.DetalleAsignacion", b =>
                {
                    b.HasOne("Asignacion", "asignacion")
                        .WithMany("detalleAsignaciones")
                        .HasForeignKey("idAsignacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backendSistemaInventario.Modelo.Componente", "componente")
                        .WithMany()
                        .HasForeignKey("idComponente");

                    b.HasOne("backendSistemaInventario.Modelo.DispositivoExt", "dispositivoExt")
                        .WithMany()
                        .HasForeignKey("idDispExt");

                    b.HasOne("backendSistemaInventario.Modelo.Equipo", "equipo")
                        .WithMany()
                        .HasForeignKey("idEquipo");

                    b.HasOne("backendSistemaInventario.Modelo.EquipoSeguridad", "equipoSeguridad")
                        .WithMany()
                        .HasForeignKey("idEquipoSeguridad");

                    b.Navigation("asignacion");

                    b.Navigation("componente");

                    b.Navigation("dispositivoExt");

                    b.Navigation("equipo");

                    b.Navigation("equipoSeguridad");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.Equipo", b =>
                {
                    b.HasOne("backendSistemaInventario.Modelo.DiscoDuro", "discoDuro")
                        .WithMany()
                        .HasForeignKey("idDiscoDuro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("discoDuro");
                });

            modelBuilder.Entity("backendSistemaInventario.Modelo.RefreshToken", b =>
                {
                    b.HasOne("backendSistemaInventario.Modelo.Administrador", "administrador")
                        .WithMany()
                        .HasForeignKey("administradorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("administrador");
                });

            modelBuilder.Entity("Asignacion", b =>
                {
                    b.Navigation("detalleAsignaciones");
                });
#pragma warning restore 612, 618
        }
    }
}
