namespace FunerariaApp.Models
{
    public class ServicioEmpleado
    {
        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; } = null!;

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; } = null!;
    }
}