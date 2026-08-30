using FunerariaApp.Models;

namespace FunerariaApp.Repositories
{
    public interface IRentaRepository : IRepository<Renta>
    {
        Task<IEnumerable<Renta>> GetActivasAsync();
        Task<IEnumerable<Renta>> GetPorVencerAsync(int diasAviso);
        Task<IEnumerable<Renta>> GetVencidasAsync();
        Task<Renta?> GetWithEquipoAsync(int id);
    }
}