namespace FunerariaApp.Models
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public bool Activa { get; set; } = true;

        public ICollection<Ataud> Ataudes { get; set; } = new List<Ataud>();
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}