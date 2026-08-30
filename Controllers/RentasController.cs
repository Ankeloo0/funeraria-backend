using FunerariaApp.DTOs.Rentas;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunerariaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RentasController : ControllerBase
    {
        private readonly RentaService _service;

        public RentasController(RentaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rentas = await _service.GetAllAsync();
            return Ok(rentas);
        }

        [HttpGet("activas")]
        public async Task<IActionResult> GetActivas()
        {
            var rentas = await _service.GetActivasAsync();
            return Ok(rentas);
        }

        [HttpGet("por-vencer")]
        public async Task<IActionResult> GetPorVencer()
        {
            var rentas = await _service.GetPorVencerAsync();
            return Ok(rentas);
        }

        [HttpGet("vencidas")]
        public async Task<IActionResult> GetVencidas()
        {
            var rentas = await _service.GetVencidasAsync();
            return Ok(rentas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var renta = await _service.GetByIdAsync(id);
            if (renta is null)
                return NotFound(new { mensaje = "Renta no encontrada" });

            return Ok(renta);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearRentaDto dto)
        {
            var creada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }

        [HttpPatch("{id}/cerrar")]
        public async Task<IActionResult> CerrarRenta(int id, [FromBody] CerrarRentaDto dto)
        {
            var cerrada = await _service.CerrarRentaAsync(id, dto);
            if (cerrada is null)
                return NotFound(new { mensaje = "Renta no encontrada" });

            return Ok(cerrada);
        }
    }
}