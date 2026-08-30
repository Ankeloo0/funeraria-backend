using FunerariaApp.DTOs.Ataudes;
using FunerariaApp.Models.Enums;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AtaudesController : ControllerBase
    {
        private readonly AtaudService _service;

        public AtaudesController(AtaudService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ataudes = await _service.GetAllAsync();
            return Ok(ataudes);
        }

        [HttpGet("sucursal/{sucursalId}")]
        public async Task<IActionResult> GetBySucursal(int sucursalId)
        {
            var ataudes = await _service.GetBySucursalAsync(sucursalId);
            return Ok(ataudes);
        }

        [HttpGet("sucursal/{sucursalId}/tipo/{tipo}")]
        public async Task<IActionResult> GetBySucursalYTipo(int sucursalId, TipoAtaud tipo)
        {
            var ataudes = await _service.GetBySucursalYTipoAsync(sucursalId, tipo);
            return Ok(ataudes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ataud = await _service.GetByIdAsync(id);
            if (ataud is null)
                return NotFound(new { mensaje = "Ataúd no encontrado" });

            return Ok(ataud);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearAtaudDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoAtaudDto dto)
        {
            var actualizado = await _service.ActualizarEstadoAsync(id, dto);
            if (actualizado is null)
                return NotFound(new { mensaje = "Ataúd no encontrado" });

            return Ok(actualizado);
        }

        [HttpPost("{id}/fotos")]
        public async Task<IActionResult> AgregarFoto(int id, [FromBody] AgregarFotoDto dto)
        {
            var actualizado = await _service.AgregarFotoAsync(id, dto);
            if (actualizado is null)
                return NotFound(new { mensaje = "Ataúd no encontrado" });

            return Ok(actualizado);
        }

        [HttpDelete("{id}/fotos/{fotoId}")]
        public async Task<IActionResult> EliminarFoto(int id, int fotoId)
        {
            var eliminada = await _service.EliminarFotoAsync(id, fotoId);
            if (!eliminada)
                return NotFound(new { mensaje = "Foto no encontrada" });

            return NoContent();
        }
    }
}