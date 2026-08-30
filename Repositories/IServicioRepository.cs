using FunerariaApp.Models;

namespace FunerariaApp.Repositories
{
    public interface IServicioRepository : IRepository<Servicio>
    {
        Task<Servicio?> GetWithDetallesAsync(int id);
        Task<IEnumerable<Servicio>> GetActivosAsync();
        Task<IEnumerable<Servicio>> GetBySucursalAsync(int sucursalId);
        Task AgregarEmpleadoAsync(int servicioId, int empleadoId);
        Task RemoverEmpleadoAsync(int servicioId, int empleadoId);
    }
}