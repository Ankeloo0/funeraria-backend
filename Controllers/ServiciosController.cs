using FunerariaApp.DTOs.Servicios;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServiciosController : ControllerBase
    {
        private readonly ServicioService _service;

        public ServiciosController(ServicioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var servicios = await _service.GetAllAsync();
            return Ok(servicios);
        }

        [HttpGet("activos")]
        public async Task<IActionResult> GetActivos()
        {
            var servicios = await _service.GetActivosAsync();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var servicio = await _service.GetByIdAsync(id);
            if (servicio is null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(servicio);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearServicioDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPatch("{id}/completar")]
        public async Task<IActionResult> Completar(int id)
        {
            var completado = await _service.CompletarAsync(id);
            if (completado is null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(completado);
        }

        [HttpGet("completados")]
        public async Task<IActionResult> GetCompletados()
        {
            var servicios = await _service.GetCompletadosAsync();
            return Ok(servicios);
        }

        [HttpPost("{id}/empleados/{empleadoId}")]
        public async Task<IActionResult> AgregarEmpleado(int id, int empleadoId)
        {
            var resultado = await _service.AgregarEmpleadoAsync(id, empleadoId);
            if (resultado is null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(resultado);
        }

        [HttpDelete("{id}/empleados/{empleadoId}")]
        public async Task<IActionResult> RemoverEmpleado(int id, int empleadoId)
        {
            var resultado = await _service.RemoverEmpleadoAsync(id, empleadoId);
            if (resultado is null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(resultado);
        }

        [HttpDelete("documentos/{documentoId}")]
        public async Task<IActionResult> EliminarDocumento(int documentoId)
        {
            var eliminado = await _service.EliminarDocumentoAsync(documentoId);
            if (!eliminado)
                return NotFound(new { mensaje = "Documento no encontrado" });

            return NoContent();
        }

        [HttpPatch("documentos/{documentoId}/archivo")]
        public async Task<IActionResult> ActualizarArchivoDocumento(int documentoId, [FromBody] string archivoUrl)
        {
            var resultado = await _service.ActualizarArchivoDocumentoAsync(documentoId, archivoUrl);
            if (resultado is null)
                return NotFound(new { mensaje = "Documento no encontrado" });

            return Ok(resultado);
        }

        [HttpPost("{id}/fases")]
        public async Task<IActionResult> AgregarFase(int id, [FromBody] CrearFaseDto dto)
        {
            var fase = await _service.AgregarFaseAsync(id, dto);
            if (fase is null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(fase);
        }

        [HttpPatch("fases/{faseId}/completar")]
        public async Task<IActionResult> CompletarFase(int faseId)
        {
            var fase = await _service.CompletarFaseAsync(faseId);
            if (fase is null)
                return NotFound(new { mensaje = "Fase no encontrada" });

            return Ok(fase);
        }

        [HttpPost("fases/{faseId}/documentos")]
        public async Task<IActionResult> AgregarDocumento(int faseId, [FromBody] CrearDocumentoDto dto)
        {
            var documento = await _service.AgregarDocumentoAsync(faseId, dto);
            if (documento is null)
                return NotFound(new { mensaje = "Fase no encontrada" });

            return Ok(documento);
        }

        [HttpPatch("documentos/{documentoId}/recibido")]
        public async Task<IActionResult> MarcarDocumentoRecibido(int documentoId)
        {
            var documento = await _service.MarcarDocumentoRecibidoAsync(documentoId);
            if (documento is null)
                return NotFound(new { mensaje = "Documento no encontrado" });

            return Ok(documento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return NoContent();
        }

        [HttpDelete("fases/{faseId}")]
        public async Task<IActionResult> EliminarFase(int faseId)
        {
            var eliminado = await _service.EliminarFaseAsync(faseId);
            if (!eliminado)
                return NotFound(new { mensaje = "Fase no encontrada" });

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarServicioDto dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);
            if (actualizado is null)
                return NotFound(new { mensaje = "Servicio no encontrado" });

            return Ok(actualizado);
        }
    }
}