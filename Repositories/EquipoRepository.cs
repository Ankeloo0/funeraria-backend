using FunerariaApp.Data;
using FunerariaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Repositories
{
    public class EquipoRepository : IRepository<Equipo>
    {
        private readonly FunerariaDbContext _context;

        public EquipoRepository(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipo>> GetAllAsync()
        {
            return await _context.Equipos
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<Equipo?> GetByIdAsync(int id)
        {
            return await _context.Equipos
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipo> CreateAsync(Equipo equipo)
        {
            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
            return equipo;
        }

        public async Task<Equipo> UpdateAsync(Equipo equipo)
        {
            _context.Equipos.Update(equipo);
            await _context.SaveChangesAsync();
            return equipo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var equipo = await GetByIdAsync(id);
            if (equipo is null) return false;

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}