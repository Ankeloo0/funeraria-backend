using FunerariaApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Ataudes
{
    public class ActualizarEstadoAtaudDto
    {
        [Required]
        public EstadoAtaud Estado { get; set; }
    }
}