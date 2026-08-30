using FunerariaApp.Data;
using FunerariaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Repositories
{
    public class EmpleadoRepository : IRepository<Empleado>
    {
        private readonly FunerariaDbContext _context;

        public EmpleadoRepository(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados
                .Where(e => e.Activo)
                .OrderBy(e => e.Apellidos)
                .ToListAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Empleado> CreateAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Empleado> UpdateAsync(Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await GetByIdAsync(id);
            if (empleado is null) return false;

            empleado.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}