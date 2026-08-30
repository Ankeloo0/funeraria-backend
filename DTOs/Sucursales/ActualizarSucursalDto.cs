using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Sucursales
{
    public class ActualizarSucursalDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es requerida")]
        public string Direccion { get; set; } = string.Empty;

        public string? Telefono { get; set; }
    }
}