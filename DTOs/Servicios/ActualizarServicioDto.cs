namespace FunerariaApp.DTOs.Servicios
{
    public class ActualizarServicioDto
    {
        public string NombreFinado { get; set; } = string.Empty;
        public string DireccionRecoleccion { get; set; } = string.Empty;
        public string? MapaRecoleccionUrl { get; set; }
        public string? Observaciones { get; set; }
        public int? AtaudId { get; set; }
        public int? EquipoId { get; set; }
    }
}