using FunerariaApp.Models.Enums;

namespace FunerariaApp.Models
{
    public class Ataud
    {
        public int Id { get; set; }
        public TipoAtaud Tipo { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public EstadoAtaud Estado { get; set; } = EstadoAtaud.Disponible;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; } = null!;

        public ICollection<AtaudFoto> Fotos { get; set; } = new List<AtaudFoto>();
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}