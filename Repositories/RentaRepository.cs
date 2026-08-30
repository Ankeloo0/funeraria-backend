using FunerariaApp.Data;
using FunerariaApp.Models;
using FunerariaApp.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Repositories
{
    public class RentaRepository : IRentaRepository
    {
        private readonly FunerariaDbContext _context;

        public RentaRepository(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Renta>> GetAllAsync()
        {
            return await _context.Rentas
                .Include(r => r.Equipo)
                .OrderByDescending(r => r.FechaSalida)
                .ToListAsync();
        }

        public async Task<Renta?> GetByIdAsync(int id)
        {
            return await _context.Rentas
                .Include(r => r.Equipo)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Renta?> GetWithEquipoAsync(int id)
        {
            return await _context.Rentas
                .Include(r => r.Equipo)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Renta>> GetActivasAsync()
        {
            return await _context.Rentas
                .Include(r => r.Equipo)
                .Where(r => r.Estado == EstadoRenta.Activa)
                .OrderBy(r => r.FechaRegresoEstimada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Renta>> GetPorVencerAsync(int diasAviso)
        {
            var limite = DateTime.UtcNow.AddDays(diasAviso);
            return await _context.Rentas
                .Include(r => r.Equipo)
                .Where(r => r.Estado == EstadoRenta.Activa
                         && r.FechaRegresoEstimada <= limite
                         && r.FechaRegresoEstimada >= DateTime.UtcNow)
                .OrderBy(r => r.FechaRegresoEstimada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Renta>> GetVencidasAsync()
        {
            return await _context.Rentas
                .Include(r => r.Equipo)
                .Where(r => r.Estado == EstadoRenta.Activa
                         && r.FechaRegresoEstimada < DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<Renta> CreateAsync(Renta renta)
        {
            _context.Rentas.Add(renta);
            await _context.SaveChangesAsync();
            return renta;
        }

        public async Task<Renta> UpdateAsync(Renta renta)
        {
            _context.Rentas.Update(renta);
            await _context.SaveChangesAsync();
            return renta;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var renta = await GetByIdAsync(id);
            if (renta is null) return false;

            _context.Rentas.Remove(renta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}