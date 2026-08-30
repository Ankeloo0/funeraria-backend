using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Equipos
{
    public class CrearEquipoDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Observaciones { get; set; }
    }
}