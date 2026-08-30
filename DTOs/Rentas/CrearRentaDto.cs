using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Rentas
{
    public class CrearRentaDto
    {
        [Required(ErrorMessage = "El cliente es requerido")]
        public string Cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es requerida")]
        public string DireccionServicio { get; set; } = string.Empty;

        public string? MapaUrl { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Required]
        public int EquipoId { get; set; }

        public string? Observaciones { get; set; }
    }
}