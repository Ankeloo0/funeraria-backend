using FunerariaApp.Data;
using FunerariaApp.Models;
using FunerariaApp.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Repositories
{
    public class AtaudRepository : IAtaudRepository
    {
        private readonly FunerariaDbContext _context;

        public AtaudRepository(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ataud>> GetAllAsync()
        {
            return await _context.Ataudes
                .Include(a => a.Fotos)
                .Include(a => a.Sucursal)
                .OrderBy(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<Ataud?> GetByIdAsync(int id)
        {
            return await _context.Ataudes
                .Include(a => a.Fotos)
                .Include(a => a.Sucursal)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Ataud>> GetBySucursalAsync(int sucursalId)
        {
            return await _context.Ataudes
                .Include(a => a.Fotos)
                .Where(a => a.SucursalId == sucursalId)
                .OrderBy(a => a.Estado)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ataud>> GetBySucursalYTipoAsync(int sucursalId, TipoAtaud tipo)
        {
            return await _context.Ataudes
                .Include(a => a.Fotos)
                .Where(a => a.SucursalId == sucursalId && a.Tipo == tipo)
                .ToListAsync();
        }

        public async Task<Ataud?> GetWithFotosAsync(int id)
        {
            return await _context.Ataudes
                .Include(a => a.Fotos.OrderBy(f => f.Orden))
                .Include(a => a.Sucursal)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Ataud> CreateAsync(Ataud ataud)
        {
            _context.Ataudes.Add(ataud);
            await _context.SaveChangesAsync();
            return ataud;
        }

        public async Task<Ataud> UpdateAsync(Ataud ataud)
        {
            _context.Ataudes.Update(ataud);
            await _context.SaveChangesAsync();
            return ataud;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ataud = await GetByIdAsync(id);
            if (ataud is null) return false;

            ataud.Estado = EstadoAtaud.Vendido;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}