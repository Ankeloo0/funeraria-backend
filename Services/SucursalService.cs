using FunerariaApp.DTOs.Sucursales;
using FunerariaApp.Models;
using FunerariaApp.Repositories;

namespace FunerariaApp.Services
{
    public class SucursalService
    {
        private readonly IRepository<Sucursal> _repository;

        public SucursalService(IRepository<Sucursal> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SucursalDto>> GetAllAsync()
        {
            var sucursales = await _repository.GetAllAsync();
            return sucursales.Select(s => new SucursalDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Direccion = s.Direccion,
                Telefono = s.Telefono
            });
        }

        public async Task<SucursalDto?> GetByIdAsync(int id)
        {
            var sucursal = await _repository.GetByIdAsync(id);
            if (sucursal is null) return null;

            return new SucursalDto
            {
                Id = sucursal.Id,
                Nombre = sucursal.Nombre,
                Direccion = sucursal.Direccion,
                Telefono = sucursal.Telefono
            };
        }

        public async Task<SucursalDto> CreateAsync(CrearSucursalDto dto)
        {
            var sucursal = new Sucursal
            {
                Nombre = dto.Nombre,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono
            };

            var creada = await _repository.CreateAsync(sucursal);

            return new SucursalDto
            {
                Id = creada.Id,
                Nombre = creada.Nombre,
                Direccion = creada.Direccion,
                Telefono = creada.Telefono
            };
        }

        public async Task<SucursalDto?> UpdateAsync(int id, ActualizarSucursalDto dto)
        {
            var sucursal = await _repository.GetByIdAsync(id);
            if (sucursal is null) return null;

            sucursal.Nombre = dto.Nombre;
            sucursal.Direccion = dto.Direccion;
            sucursal.Telefono = dto.Telefono;

            var actualizada = await _repository.UpdateAsync(sucursal);

            return new SucursalDto
            {
                Id = actualizada.Id,
                Nombre = actualizada.Nombre,
                Direccion = actualizada.Direccion,
                Telefono = actualizada.Telefono
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}