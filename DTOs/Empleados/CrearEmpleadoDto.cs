using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Empleados
{
    public class CrearEmpleadoDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        public string? FotoUrl { get; set; }

        [Range(10, 80)]
        public int? Edad { get; set; }
    }
}