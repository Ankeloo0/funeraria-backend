using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArchivosController : ControllerBase
    {
        private readonly ArchivoService _service;

        public ArchivosController(ArchivoService service)
        {
            _service = service;
        }

        [HttpPost("imagen")]
        public async Task<IActionResult> SubirImagen(IFormFile archivo)
        {
            try
            {
                var resultado = await _service.SubirImagenAsync(archivo);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("pdf")]
        public async Task<IActionResult> SubirPdf(IFormFile archivo)
        {
            try
            {
                var resultado = await _service.SubirPdfAsync(archivo);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult EliminarArchivo([FromQuery] string url)
        {
            var eliminado = _service.EliminarArchivo(url);
            if (!eliminado)
                return NotFound(new { mensaje = "Archivo no encontrado" });

            return NoContent();
        }
    }
}