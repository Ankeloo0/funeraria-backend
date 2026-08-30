using FunerariaApp.Models.Enums;

namespace FunerariaApp.DTOs.Ataudes
{
    public class AtaudDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int SucursalId { get; set; }
        public string NombreSucursal { get; set; } = string.Empty;
        public List<AtaudFotoDto> Fotos { get; set; } = new();
        public AtaudFotoDto? FotoPrincipal { get; set; }
    }
}