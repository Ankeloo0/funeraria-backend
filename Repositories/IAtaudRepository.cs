using FunerariaApp.Models;
using FunerariaApp.Models.Enums;

namespace FunerariaApp.Repositories
{
    public interface IAtaudRepository : IRepository<Ataud>
    {
        Task<IEnumerable<Ataud>> GetBySucursalAsync(int sucursalId);
        Task<IEnumerable<Ataud>> GetBySucursalYTipoAsync(int sucursalId, TipoAtaud tipo);
        Task<Ataud?> GetWithFotosAsync(int id);
    }
}