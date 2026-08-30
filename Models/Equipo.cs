namespace FunerariaApp.Models
{
    public class Equipo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Observaciones { get; set; }

        public ICollection<Renta> Rentas { get; set; } = new List<Renta>();
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}