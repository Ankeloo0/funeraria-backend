using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Servicios
{
    public class CrearServicioDto
    {
        [Required(ErrorMessage = "El nombre del finado es requerido")]
        public string NombreFinado { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección de recolección es requerida")]
        public string DireccionRecoleccion { get; set; } = string.Empty;

        public string? MapaRecoleccionUrl { get; set; }
        public string? Observaciones { get; set; }

        [Required]
        public int SucursalId { get; set; }

        public int? AtaudId { get; set; }
        public int? EquipoId { get; set; }
        public List<int> EmpleadoIds { get; set; } = new();
    }
}