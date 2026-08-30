using FunerariaApp.DTOs.Ataudes;
using FunerariaApp.Models;
using FunerariaApp.Models.Enums;
using FunerariaApp.Repositories;

namespace FunerariaApp.Services
{
    public class AtaudService
    {
        private readonly IAtaudRepository _repository;

        public AtaudService(IAtaudRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AtaudDto>> GetAllAsync()
        {
            var ataudes = await _repository.GetAllAsync();
            return ataudes.Select(MapToDto);
        }

        public async Task<IEnumerable<AtaudDto>> GetBySucursalAsync(int sucursalId)
        {
            var ataudes = await _repository.GetBySucursalAsync(sucursalId);
            return ataudes.Select(MapToDto);
        }

        public async Task<IEnumerable<AtaudDto>> GetBySucursalYTipoAsync(int sucursalId, TipoAtaud tipo)
        {
            var ataudes = await _repository.GetBySucursalYTipoAsync(sucursalId, tipo);
            return ataudes.Select(MapToDto);
        }

        public async Task<AtaudDto?> GetByIdAsync(int id)
        {
            var ataud = await _repository.GetWithFotosAsync(id);
            if (ataud is null) return null;
            return MapToDto(ataud);
        }

        public async Task<AtaudDto> CreateAsync(CrearAtaudDto dto)
        {
            var ataud = new Ataud
            {
                Tipo = dto.Tipo,
                Color = dto.Color,
                Descripcion = dto.Descripcion,
                SucursalId = dto.SucursalId
            };

            var creado = await _repository.CreateAsync(ataud);
            return MapToDto(creado);
        }

        public async Task<AtaudDto?> ActualizarEstadoAsync(int id, ActualizarEstadoAtaudDto dto)
        {
            var ataud = await _repository.GetByIdAsync(id);
            if (ataud is null) return null;

            ataud.Estado = dto.Estado;
            var actualizado = await _repository.UpdateAsync(ataud);
            return MapToDto(actualizado);
        }

        public async Task<AtaudDto?> AgregarFotoAsync(int id, AgregarFotoDto dto)
        {
            var ataud = await _repository.GetWithFotosAsync(id);
            if (ataud is null) return null;

            // Si esta foto es principal, quita el flag de las demás
            if (dto.EsPrincipal)
            {
                foreach (var foto in ataud.Fotos)
                    foto.EsPrincipal = false;
            }

            ataud.Fotos.Add(new AtaudFoto
            {
                Url = dto.Url,
                EsPrincipal = dto.EsPrincipal,
                Orden = dto.Orden,
                AtaudId = id
            });

            var actualizado = await _repository.UpdateAsync(ataud);
            return MapToDto(actualizado);
        }

        public async Task<bool> EliminarFotoAsync(int ataudId, int fotoId)
        {
            var ataud = await _repository.GetWithFotosAsync(ataudId);
            if (ataud is null) return false;

            var foto = ataud.Fotos.FirstOrDefault(f => f.Id == fotoId);
            if (foto is null) return false;

            ataud.Fotos.Remove(foto);
            await _repository.UpdateAsync(ataud);
            return true;
        }

        // Método privado reutilizable — patrón mapper
        private static AtaudDto MapToDto(Ataud ataud)
        {
            var fotos = ataud.Fotos?.Select(f => new AtaudFotoDto
            {
                Id = f.Id,
                Url = f.Url,
                EsPrincipal = f.EsPrincipal,
                Orden = f.Orden
            }).ToList() ?? new List<AtaudFotoDto>();

            return new AtaudDto
            {
                Id = ataud.Id,
                Tipo = ataud.Tipo.ToString(),
                Color = ataud.Color,
                Descripcion = ataud.Descripcion,
                Estado = ataud.Estado.ToString(),
                FechaRegistro = ataud.FechaRegistro,
                SucursalId = ataud.SucursalId,
                NombreSucursal = ataud.Sucursal?.Nombre ?? string.Empty,
                Fotos = fotos,
                FotoPrincipal = fotos.FirstOrDefault(f => f.EsPrincipal)
            };
        }
    }
}