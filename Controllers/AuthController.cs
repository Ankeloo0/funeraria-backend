using FunerariaApp.DTOs.Auth;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var resultado = await _service.LoginAsync(dto);

            if (resultado is null)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

            return Ok(resultado);
        }

        // SOLO PARA DESARROLLO — borrar después
        [HttpPost("seed")]
        public IActionResult GenerarHash([FromBody] string password)
        {
            return Ok(new { hash = BCrypt.Net.BCrypt.HashPassword(password) });
        }
    }
}