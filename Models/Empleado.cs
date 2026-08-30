namespace FunerariaApp.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public int? Edad { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<ServicioEmpleado> ServicioEmpleados { get; set; } = new List<ServicioEmpleado>();
    }
}