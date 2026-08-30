using FunerariaApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Ataudes
{
    public class CrearAtaudDto
    {
        [Required]
        public TipoAtaud Tipo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Color { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int SucursalId { get; set; }
    }
}