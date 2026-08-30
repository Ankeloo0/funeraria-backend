using FunerariaApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Servicios
{
    public class CrearFaseDto
    {
        [Required]
        public NumeroFase NumeroFase { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string? Notas { get; set; }
    }
}