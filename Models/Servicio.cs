namespace FunerariaApp.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string NombreFinado { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
        public string DireccionRecoleccion { get; set; } = string.Empty;
        public string? MapaRecoleccionUrl { get; set; }
        public string? Observaciones { get; set; }

        public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; } = null!;

        public int? AtaudId { get; set; }
        public Ataud? Ataud { get; set; }

        public int? EquipoId { get; set; }
        public Equipo? Equipo { get; set; }

        public bool Completado { get; set; } = false;

        public ICollection<ServicioEmpleado> ServicioEmpleados { get; set; } = new List<ServicioEmpleado>();
        public ICollection<FaseServicio> Fases { get; set; } = new List<FaseServicio>();
    }
}