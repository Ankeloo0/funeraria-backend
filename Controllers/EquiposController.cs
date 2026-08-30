using FunerariaApp.DTOs.Equipos;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EquiposController : ControllerBase
    {
        private readonly EquipoService _service;

        public EquiposController(EquipoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var equipos = await _service.GetAllAsync();
            return Ok(equipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var equipo = await _service.GetByIdAsync(id);
            if (equipo is null)
                return NotFound(new { mensaje = "Equipo no encontrado" });

            return Ok(equipo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearEquipoDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarEquipoDto dto)
        {
            var actualizado = await _service.UpdateAsync(id, dto);
            if (actualizado is null)
                return NotFound(new { mensaje = "Equipo no encontrado" });

            return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Equipo no encontrado" });

            return NoContent();
        }
    }
}