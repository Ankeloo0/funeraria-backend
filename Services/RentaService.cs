using FunerariaApp.DTOs.Rentas;
using FunerariaApp.Models;
using FunerariaApp.Models.Enums;
using FunerariaApp.Repositories;

namespace FunerariaApp.Services
{
    public class RentaService
    {
        private readonly IRentaRepository _repository;
        private const int DiasRenta = 10;
        private const int DiasAviso = 2;

        public RentaService(IRentaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RentaDto>> GetAllAsync()
        {
            var rentas = await _repository.GetAllAsync();
            return rentas.Select(MapToDto);
        }

        public async Task<IEnumerable<RentaDto>> GetActivasAsync()
        {
            var rentas = await _repository.GetActivasAsync();
            return rentas.Select(MapToDto);
        }

        public async Task<IEnumerable<RentaDto>> GetPorVencerAsync()
        {
            var rentas = await _repository.GetPorVencerAsync(DiasAviso);
            return rentas.Select(MapToDto);
        }

        public async Task<IEnumerable<RentaDto>> GetVencidasAsync()
        {
            var rentas = await _repository.GetVencidasAsync();
            return rentas.Select(MapToDto);
        }

        public async Task<RentaDto?> GetByIdAsync(int id)
        {
            var renta = await _repository.GetWithEquipoAsync(id);
            if (renta is null) return null;
            return MapToDto(renta);
        }

        public async Task<RentaDto> CreateAsync(CrearRentaDto dto)
        {
            var renta = new Renta
            {
                Cliente = dto.Cliente,
                Telefono = dto.Telefono,
                DireccionServicio = dto.DireccionServicio,
                MapaUrl = dto.MapaUrl,
                FechaSalida = dto.FechaSalida.ToUniversalTime(),
                FechaRegresoEstimada = dto.FechaSalida.ToUniversalTime().AddDays(DiasRenta),
                Estado = EstadoRenta.Activa,
                Observaciones = dto.Observaciones,
                EquipoId = dto.EquipoId
            };

            var creada = await _repository.CreateAsync(renta);
            return MapToDto(creada);
        }

        public async Task<RentaDto?> CerrarRentaAsync(int id, CerrarRentaDto dto)
        {
            var renta = await _repository.GetByIdAsync(id);
            if (renta is null) return null;

            renta.Estado = EstadoRenta.Devuelta;
            renta.FechaRegresoReal = dto.FechaRegresoReal.ToUniversalTime();

            var actualizada = await _repository.UpdateAsync(renta);
            return MapToDto(actualizada);
        }

        private static RentaDto MapToDto(Renta renta)
        {
            var diasRestantes = (int)(renta.FechaRegresoEstimada - DateTime.UtcNow).TotalDays;

            return new RentaDto
            {
                Id = renta.Id,
                Cliente = renta.Cliente,
                Telefono = renta.Telefono,
                DireccionServicio = renta.DireccionServicio,
                MapaUrl = renta.MapaUrl,
                FechaSalida = renta.FechaSalida,
                FechaRegresoEstimada = renta.FechaRegresoEstimada,
                FechaRegresoReal = renta.FechaRegresoReal,
                Estado = renta.Estado.ToString(),
                Observaciones = renta.Observaciones,
                EquipoId = renta.EquipoId,
                NombreEquipo = renta.Equipo?.Nombre ?? string.Empty,
                DiasRestantes = diasRestantes
            };
        }
    }
}