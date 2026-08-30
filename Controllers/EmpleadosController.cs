using FunerariaApp.DTOs.Empleados;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpleadosController : ControllerBase
    {
        private readonly EmpleadoService _service;

        public EmpleadosController(EmpleadoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var empleados = await _service.GetAllAsync();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empleado = await _service.GetByIdAsync(id);
            if (empleado is null)
                return NotFound(new { mensaje = "Empleado no encontrado" });

            return Ok(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearEmpleadoDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarEmpleadoDto dto)
        {
            var actualizado = await _service.UpdateAsync(id, dto);
            if (actualizado is null)
                return NotFound(new { mensaje = "Empleado no encontrado" });

            return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Empleado no encontrado" });

            return NoContent();
        }
    }
}