namespace FunerariaApp.DTOs.Empleados
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public int? Edad { get; set; }
    }
}