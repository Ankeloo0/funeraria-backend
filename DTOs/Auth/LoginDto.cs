using System.ComponentModel.DataAnnotations;

namespace FunerariaApp.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}