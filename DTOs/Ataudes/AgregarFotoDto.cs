using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Ataudes
{
    public class AgregarFotoDto
    {
        [Required]
        public string Url { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; } = false;
        public int Orden { get; set; } = 0;
    }
}