using FunerariaApp.Data;
using FunerariaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Repositories
{
    public class SucursalRepository : IRepository<Sucursal>
    {
        private readonly FunerariaDbContext _context;

        public SucursalRepository(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sucursal>> GetAllAsync()
        {
            return await _context.Sucursales
                .Where(s => s.Activa)
                .OrderBy(s => s.Nombre)
                .ToListAsync();
        }

        public async Task<Sucursal?> GetByIdAsync(int id)
        {
            return await _context.Sucursales
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sucursal> CreateAsync(Sucursal sucursal)
        {
            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();
            return sucursal;
        }

        public async Task<Sucursal> UpdateAsync(Sucursal sucursal)
        {
            _context.Sucursales.Update(sucursal);
            await _context.SaveChangesAsync();
            return sucursal;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sucursal = await GetByIdAsync(id);
            if (sucursal is null) return false;

            sucursal.Activa = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}