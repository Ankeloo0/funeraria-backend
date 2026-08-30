namespace FunerariaApp.DTOs.Rentas
{
    public class RentaDto
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string DireccionServicio { get; set; } = string.Empty;
        public string? MapaUrl { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRegresoEstimada { get; set; }
        public DateTime? FechaRegresoReal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int EquipoId { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public int DiasRestantes { get; set; }
    }
}