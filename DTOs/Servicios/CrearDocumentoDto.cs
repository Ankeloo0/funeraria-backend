using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Servicios
{
    public class CrearDocumentoDto
    {
        [Required]
        public string NombreDocumento { get; set; } = string.Empty;
        public bool Recibido { get; set; } = false;
        public string? ArchivoUrl { get; set; }
    }
}