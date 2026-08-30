using FunerariaApp.DTOs.Sucursales;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SucursalesController : ControllerBase
    {
        private readonly SucursalService _service;

        public SucursalesController(SucursalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sucursales = await _service.GetAllAsync();
            return Ok(sucursales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sucursal = await _service.GetByIdAsync(id);
            if (sucursal is null)
                return NotFound(new { mensaje = "Sucursal no encontrada" });

            return Ok(sucursal);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearSucursalDto dto)
        {
            var creada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarSucursalDto dto)
        {
            var actualizada = await _service.UpdateAsync(id, dto);
            if (actualizada is null)
                return NotFound(new { mensaje = "Sucursal no encontrada" });

            return Ok(actualizada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminada = await _service.DeleteAsync(id);
            if (!eliminada)
                return NotFound(new { mensaje = "Sucursal no encontrada" });

            return NoContent();
        }
    }
}