using FunerariaApp.DTOs.Empleados;
using FunerariaApp.Models;
using FunerariaApp.Repositories;

namespace FunerariaApp.Services
{
    public class EmpleadoService
    {
        private readonly IRepository<Empleado> _repository;

        public EmpleadoService(IRepository<Empleado> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmpleadoDto>> GetAllAsync()
        {
            var empleados = await _repository.GetAllAsync();
            return empleados.Select(e => new EmpleadoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                FotoUrl = e.FotoUrl,
                Edad = e.Edad
            });
        }

        public async Task<EmpleadoDto?> GetByIdAsync(int id)
        {
            var empleado = await _repository.GetByIdAsync(id);
            if (empleado is null) return null;

            return new EmpleadoDto
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre,
                Apellidos = empleado.Apellidos,
                FotoUrl = empleado.FotoUrl,
                Edad = empleado.Edad
            };
        }

        public async Task<EmpleadoDto> CreateAsync(CrearEmpleadoDto dto)
        {
            var empleado = new Empleado
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                FotoUrl = dto.FotoUrl,
                Edad = dto.Edad
            };

            var creado = await _repository.CreateAsync(empleado);

            return new EmpleadoDto
            {
                Id = creado.Id,
                Nombre = creado.Nombre,
                Apellidos = creado.Apellidos,
                FotoUrl = creado.FotoUrl,
                Edad = creado.Edad
            };
        }

        public async Task<EmpleadoDto?> UpdateAsync(int id, ActualizarEmpleadoDto dto)
        {
            var empleado = await _repository.GetByIdAsync(id);
            if (empleado is null) return null;

            empleado.Nombre = dto.Nombre;
            empleado.Apellidos = dto.Apellidos;
            empleado.FotoUrl = dto.FotoUrl;
            empleado.Edad = dto.Edad;

            var actualizado = await _repository.UpdateAsync(empleado);

            return new EmpleadoDto
            {
                Id = actualizado.Id,
                Nombre = actualizado.Nombre,
                Apellidos = actualizado.Apellidos,
                FotoUrl = actualizado.FotoUrl,
                Edad = actualizado.Edad
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}