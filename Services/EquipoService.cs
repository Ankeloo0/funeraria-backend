using FunerariaApp.Data;
using FunerariaApp.DTOs.Equipos;
using FunerariaApp.Models;
using FunerariaApp.Models.Enums;
using FunerariaApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Services
{
    public class EquipoService
    {
        private readonly IRepository<Equipo> _repository;
        private readonly FunerariaDbContext _context;

        public EquipoService(IRepository<Equipo> repository, FunerariaDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<EquipoDto>> GetAllAsync()
        {
            var equipos = await _repository.GetAllAsync();
            var resultado = new List<EquipoDto>();

            foreach (var equipo in equipos)
            {
                var disponible = await EstaDisponibleAsync(equipo.Id);
                resultado.Add(MapToDto(equipo, disponible));
            }

            return resultado;
        }

        public async Task<EquipoDto?> GetByIdAsync(int id)
        {
            var equipo = await _repository.GetByIdAsync(id);
            if (equipo is null) return null;

            var disponible = await EstaDisponibleAsync(id);
            return MapToDto(equipo, disponible);
        }

        public async Task<EquipoDto> CreateAsync(CrearEquipoDto dto)
        {
            var equipo = new Equipo
            {
                Nombre = dto.Nombre,
                Observaciones = dto.Observaciones
            };

            var creado = await _repository.CreateAsync(equipo);
            return MapToDto(creado, true);
        }

        public async Task<EquipoDto?> UpdateAsync(int id, ActualizarEquipoDto dto)
        {
            var equipo = await _repository.GetByIdAsync(id);
            if (equipo is null) return null;

            equipo.Nombre = dto.Nombre;
            equipo.Observaciones = dto.Observaciones;

            var actualizado = await _repository.UpdateAsync(equipo);
            var disponible = await EstaDisponibleAsync(id);
            return MapToDto(actualizado, disponible);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // Calcula disponibilidad en base a rentas activas
        private async Task<bool> EstaDisponibleAsync(int equipoId)
        {
            return !await _context.Rentas
                .AnyAsync(r => r.EquipoId == equipoId
                            && r.Estado == EstadoRenta.Activa);
        }

        private static EquipoDto MapToDto(Equipo equipo, bool disponible)
        {
            return new EquipoDto
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                Observaciones = equipo.Observaciones,
                Disponible = disponible
            };
        }
    }
}