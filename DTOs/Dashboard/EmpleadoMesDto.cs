namespace FunerariaApp.DTOs.Dashboard
{
    public class EmpleadoMesDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public int TotalServicios { get; set; }
    }
}