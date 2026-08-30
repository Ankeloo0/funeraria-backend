using FunerariaApp.Data;
using FunerariaApp.DTOs.Dashboard;
using FunerariaApp.DTOs.Rentas;
using FunerariaApp.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Services
{
    public class DashboardService
    {
        private readonly FunerariaDbContext _context;
        private const int DiasAviso = 2;

        public DashboardService(FunerariaDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMes = new DateTime(ahora.Year, ahora.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var limiteAviso = ahora.AddDays(DiasAviso);

            // Ataúdes por estado
            var ataudes = await _context.Ataudes
                .GroupBy(a => a.Estado)
                .Select(g => new { Estado = g.Key, Count = g.Count() })
                .ToListAsync();

            var ataudesDisponibles = ataudes
                .FirstOrDefault(a => a.Estado == EstadoAtaud.Disponible)?.Count ?? 0;
            var ataudesApartados = ataudes
                .FirstOrDefault(a => a.Estado == EstadoAtaud.Apartado)?.Count ?? 0;
            var ataudesVendidos = ataudes
                .FirstOrDefault(a => a.Estado == EstadoAtaud.Vendido)?.Count ?? 0;

            // Equipos disponibles vs rentados
            var totalEquipos = await _context.Equipos.CountAsync();
            var equiposRentados = await _context.Rentas
                .Where(r => r.Estado == EstadoRenta.Activa)
                .Select(r => r.EquipoId)
                .Distinct()
                .CountAsync();
            var equiposDisponibles = totalEquipos - equiposRentados;

            // Servicios activos
            var serviciosActivos = await _context.Servicios
                .CountAsync(s => !s.Completado);

            // Rentas por vencer
            var rentasPorVencer = await _context.Rentas
                .Include(r => r.Equipo)
                .Where(r => r.Estado == EstadoRenta.Activa
                         && r.FechaRegresoEstimada <= limiteAviso
                         && r.FechaRegresoEstimada >= ahora)
                .OrderBy(r => r.FechaRegresoEstimada)
                .ToListAsync();

            // Rentas vencidas
            var rentasVencidas = await _context.Rentas
                .Include(r => r.Equipo)
                .Where(r => r.Estado == EstadoRenta.Activa
                         && r.FechaRegresoEstimada < ahora)
                .ToListAsync();

            // Empleado del mes
            var empleadoMes = await _context.ServicioEmpleados
                .Include(se => se.Empleado)
                .Include(se => se.Servicio)
                .Where(se => se.Servicio.FechaInicio >= inicioMes)
                .GroupBy(se => new
                {
                    se.EmpleadoId,
                    se.Empleado.Nombre,
                    se.Empleado.Apellidos,
                    se.Empleado.FotoUrl
                })
                .Select(g => new EmpleadoMesDto
                {
                    Id = g.Key.EmpleadoId,
                    NombreCompleto = $"{g.Key.Nombre} {g.Key.Apellidos}",
                    FotoUrl = g.Key.FotoUrl,
                    TotalServicios = g.Count()
                })
                .OrderByDescending(e => e.TotalServicios)
                .FirstOrDefaultAsync();

            return new DashboardDto
            {
                AtaudesDisponibles = ataudesDisponibles,
                AtaudesApartados = ataudesApartados,
                AtaudesVendidos = ataudesVendidos,
                EquiposDisponibles = equiposDisponibles,
                EquiposRentados = equiposRentados,
                ServiciosActivos = serviciosActivos,
                RentasPorVencer = rentasPorVencer.Count,
                RentasVencidas = rentasVencidas.Count,
                DetalleRentasPorVencer = rentasPorVencer.Select(r => new RentaDto
                {
                    Id = r.Id,
                    Cliente = r.Cliente,
                    Telefono = r.Telefono,
                    DireccionServicio = r.DireccionServicio,
                    MapaUrl = r.MapaUrl,
                    FechaSalida = r.FechaSalida,
                    FechaRegresoEstimada = r.FechaRegresoEstimada,
                    FechaRegresoReal = r.FechaRegresoReal,
                    Estado = r.Estado.ToString(),
                    Observaciones = r.Observaciones,
                    EquipoId = r.EquipoId,
                    NombreEquipo = r.Equipo?.Nombre ?? string.Empty,
                    DiasRestantes = (int)(r.FechaRegresoEstimada - ahora).TotalDays
                }).ToList(),
                DetalleRentasVencidas = rentasVencidas.Select(r => new RentaDto
                {
                    Id = r.Id,
                    Cliente = r.Cliente,
                    Telefono = r.Telefono,
                    DireccionServicio = r.DireccionServicio,
                    MapaUrl = r.MapaUrl,
                    FechaSalida = r.FechaSalida,
                    FechaRegresoEstimada = r.FechaRegresoEstimada,
                    FechaRegresoReal = r.FechaRegresoReal,
                    Estado = r.Estado.ToString(),
                    Observaciones = r.Observaciones,
                    EquipoId = r.EquipoId,
                    NombreEquipo = r.Equipo?.Nombre ?? string.Empty,
                    DiasRestantes = (int)(r.FechaRegresoEstimada - ahora).TotalDays
                }).ToList(),
                EmpleadoDelMes = empleadoMes
            };
        }
    }
}