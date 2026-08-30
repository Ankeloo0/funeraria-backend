using FunerariaApp.Models.Enums;

namespace FunerariaApp.Models
{
    public class Renta
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string DireccionServicio { get; set; } = string.Empty;
        public string? MapaUrl { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRegresoEstimada { get; set; }
        public DateTime? FechaRegresoReal { get; set; }
        public EstadoRenta Estado { get; set; } = EstadoRenta.Activa;
        public string? Observaciones { get; set; }

        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; } = null!;
    }
}