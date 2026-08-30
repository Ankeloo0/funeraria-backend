using FunerariaApp.Data;
using FunerariaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Repositories
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly FunerariaDbContext _context;

        public ServicioRepository(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> GetAllAsync()
        {
            return await _context.Servicios
                .Include(s => s.Sucursal)
                .Include(s => s.Ataud)
                .Include(s => s.Equipo)
                .Include(s => s.ServicioEmpleados)
                    .ThenInclude(se => se.Empleado)
                .OrderByDescending(s => s.FechaInicio)
                .ToListAsync();
        }

        public async Task<Servicio?> GetByIdAsync(int id)
        {
            return await _context.Servicios
                .Include(s => s.Sucursal)
                .Include(s => s.Ataud)
                .Include(s => s.Equipo)
                .Include(s => s.ServicioEmpleados)
                    .ThenInclude(se => se.Empleado)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Servicio?> GetWithDetallesAsync(int id)
        {
            return await _context.Servicios
                .Include(s => s.Sucursal)
                .Include(s => s.Ataud)
                    .ThenInclude(a => a!.Fotos)
                .Include(s => s.Equipo)
                .Include(s => s.ServicioEmpleados)
                    .ThenInclude(se => se.Empleado)
                .Include(s => s.Fases)
                    .ThenInclude(f => f.Documentos)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Servicio>> GetActivosAsync()
        {
            return await _context.Servicios
                .Include(s => s.Sucursal)
                .Include(s => s.Equipo)
                .Include(s => s.ServicioEmpleados)
                    .ThenInclude(se => se.Empleado)
                .Where(s => !s.Completado)
                .OrderByDescending(s => s.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Servicio>> GetBySucursalAsync(int sucursalId)
        {
            return await _context.Servicios
                .Include(s => s.Sucursal)
                .Include(s => s.Equipo)
                .Where(s => s.SucursalId == sucursalId)
                .OrderByDescending(s => s.FechaInicio)
                .ToListAsync();
        }

        public async Task<Servicio> CreateAsync(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<Servicio> UpdateAsync(Servicio servicio)
        {
            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var servicio = await GetByIdAsync(id);
            if (servicio is null) return false;

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AgregarEmpleadoAsync(int servicioId, int empleadoId)
        {
            var existe = await _context.ServicioEmpleados
                .AnyAsync(se => se.ServicioId == servicioId && se.EmpleadoId == empleadoId);

            if (!existe)
            {
                _context.ServicioEmpleados.Add(new ServicioEmpleado
                {
                    ServicioId = servicioId,
                    EmpleadoId = empleadoId
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverEmpleadoAsync(int servicioId, int empleadoId)
        {
            var registro = await _context.ServicioEmpleados
                .FirstOrDefaultAsync(se => se.ServicioId == servicioId
                                        && se.EmpleadoId == empleadoId);

            if (registro is not null)
            {
                _context.ServicioEmpleados.Remove(registro);
                await _context.SaveChangesAsync();
            }
        }
    }
}