using Microsoft.EntityFrameworkCore;
using FunerariaApp.Models;

namespace FunerariaApp.Data
{
    public class FunerariaDbContext : DbContext
    {
        public FunerariaDbContext(DbContextOptions<FunerariaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Ataud> Ataudes { get; set; }
        public DbSet<AtaudFoto> AtaudFotos { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Renta> Rentas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ServicioEmpleado> ServicioEmpleados { get; set; }
        public DbSet<FaseServicio> FasesServicio { get; set; }
        public DbSet<DocumentoFase> DocumentosFase { get; set; }
        public DbSet<Vela> Velas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Ataud ──────────────────────────────────────────────
            modelBuilder.Entity<Ataud>(entity =>
            {
                entity.Property(a => a.Tipo)
                      .HasConversion<string>()
                      .HasMaxLength(50);

                entity.Property(a => a.Estado)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(a => a.Color)
                      .HasMaxLength(50);

                entity.HasOne(a => a.Sucursal)
                      .WithMany(s => s.Ataudes)
                      .HasForeignKey(a => a.SucursalId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── AtaudFoto ──────────────────────────────────────────
            modelBuilder.Entity<AtaudFoto>(entity =>
            {
                entity.HasOne(f => f.Ataud)
                      .WithMany(a => a.Fotos)
                      .HasForeignKey(f => f.AtaudId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Renta ──────────────────────────────────────────────
            modelBuilder.Entity<Renta>(entity =>
            {
                entity.Property(r => r.Estado)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.HasOne(r => r.Equipo)
                      .WithMany(e => e.Rentas)
                      .HasForeignKey(r => r.EquipoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Servicio ───────────────────────────────────────────
            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.HasOne(s => s.Sucursal)
                      .WithMany(su => su.Servicios)
                      .HasForeignKey(s => s.SucursalId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Ataud es opcional
                entity.HasOne(s => s.Ataud)
                      .WithMany(a => a.Servicios)
                      .HasForeignKey(s => s.AtaudId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);

                // Equipo es opcional
                entity.HasOne(s => s.Equipo)
                      .WithMany(e => e.Servicios)
                      .HasForeignKey(s => s.EquipoId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ── ServicioEmpleado — clave compuesta ─────────────────
            modelBuilder.Entity<ServicioEmpleado>(entity =>
            {
                entity.HasKey(se => new { se.ServicioId, se.EmpleadoId });

                entity.HasOne(se => se.Servicio)
                      .WithMany(s => s.ServicioEmpleados)
                      .HasForeignKey(se => se.ServicioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(se => se.Empleado)
                      .WithMany(e => e.ServicioEmpleados)
                      .HasForeignKey(se => se.EmpleadoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── FaseServicio ───────────────────────────────────────
            modelBuilder.Entity<FaseServicio>(entity =>
            {
                entity.Property(f => f.NumeroFase)
                      .HasConversion<string>()
                      .HasMaxLength(30);

                entity.HasOne(f => f.Servicio)
                      .WithMany(s => s.Fases)
                      .HasForeignKey(f => f.ServicioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── DocumentoFase ──────────────────────────────────────
            modelBuilder.Entity<DocumentoFase>(entity =>
            {
                entity.HasOne(d => d.FaseServicio)
                      .WithMany(f => f.Documentos)
                      .HasForeignKey(d => d.FaseServicioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}