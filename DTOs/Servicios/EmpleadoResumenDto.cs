namespace FunerariaApp.DTOs.Servicios
{
    public class EmpleadoResumenDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
    }
}