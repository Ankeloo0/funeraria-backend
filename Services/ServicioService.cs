using FunerariaApp.Data;
using FunerariaApp.DTOs.Servicios;
using FunerariaApp.Models;
using FunerariaApp.Models.Enums;
using FunerariaApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FunerariaApp.Services
{
    public class ServicioService
    {
        private readonly IServicioRepository _repository;
        private readonly FunerariaDbContext _context;

        public ServicioService(IServicioRepository repository, FunerariaDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<ServicioDto>> GetAllAsync()
        {
            var servicios = await _repository.GetAllAsync();
            return servicios.Select(MapToDto);
        }

        public async Task<IEnumerable<ServicioDto>> GetActivosAsync()
        {
            var servicios = await _repository.GetActivosAsync();
            return servicios.Select(MapToDto);
        }

        public async Task<ServicioDto?> GetByIdAsync(int id)
        {
            var servicio = await _repository.GetWithDetallesAsync(id);
            if (servicio is null) return null;
            return MapToDto(servicio);
        }

        public async Task<ServicioDto> CreateAsync(CrearServicioDto dto)
        {
            var servicio = new Servicio
            {
                NombreFinado = dto.NombreFinado,
                DireccionRecoleccion = dto.DireccionRecoleccion,
                MapaRecoleccionUrl = dto.MapaRecoleccionUrl,
                Observaciones = dto.Observaciones,
                SucursalId = dto.SucursalId,
                AtaudId = dto.AtaudId,
                EquipoId = dto.EquipoId,
                FechaInicio = DateTime.UtcNow
            };

            var creado = await _repository.CreateAsync(servicio);

            // Agregar empleados al servicio
            foreach (var empleadoId in dto.EmpleadoIds)
                await _repository.AgregarEmpleadoAsync(creado.Id, empleadoId);

            // Actualizar estado del ataúd si se asignó uno
            if (dto.AtaudId.HasValue)
                await ActualizarEstadoAtaudAsync(dto.AtaudId.Value, EstadoAtaud.Vendido);

            return MapToDto(await _repository.GetWithDetallesAsync(creado.Id) ?? creado);
        }

        public async Task<ServicioDto?> CompletarAsync(int id)
        {
            var servicio = await _repository.GetByIdAsync(id);
            if (servicio is null) return null;

            servicio.Completado = true;
            var actualizado = await _repository.UpdateAsync(servicio);
            return MapToDto(actualizado);
        }

        public async Task<DocumentoFaseDto?> ActualizarArchivoDocumentoAsync(int documentoId, string archivoUrl)
        {
            var documento = await _context.DocumentosFase
                .FirstOrDefaultAsync(d => d.Id == documentoId);

            if (documento is null) return null;

            documento.ArchivoUrl = archivoUrl;
            documento.Recibido = true;
            documento.FechaRecepcion = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new DocumentoFaseDto
            {
                Id = documento.Id,
                NombreDocumento = documento.NombreDocumento,
                Recibido = documento.Recibido,
                ArchivoUrl = documento.ArchivoUrl,
                FechaRecepcion = documento.FechaRecepcion
            };
        }

        public async Task<ServicioDto?> AgregarEmpleadoAsync(int servicioId, int empleadoId)
        {
            var servicio = await _repository.GetByIdAsync(servicioId);
            if (servicio is null) return null;

            await _repository.AgregarEmpleadoAsync(servicioId, empleadoId);
            return MapToDto(await _repository.GetWithDetallesAsync(servicioId) ?? servicio);
        }

        public async Task<ServicioDto?> RemoverEmpleadoAsync(int servicioId, int empleadoId)
        {
            var servicio = await _repository.GetByIdAsync(servicioId);
            if (servicio is null) return null;

            await _repository.RemoverEmpleadoAsync(servicioId, empleadoId);
            return MapToDto(await _repository.GetWithDetallesAsync(servicioId) ?? servicio);
        }

        public async Task<FaseServicioDto?> AgregarFaseAsync(int servicioId, CrearFaseDto dto)
        {
            var servicio = await _repository.GetByIdAsync(servicioId);
            if (servicio is null) return null;

            var fase = new FaseServicio
            {
                ServicioId = servicioId,
                NumeroFase = dto.NumeroFase,
                Nombre = dto.Nombre,
                Notas = dto.Notas
            };

            _context.FasesServicio.Add(fase);
            await _context.SaveChangesAsync();

            return new FaseServicioDto
            {
                Id = fase.Id,
                NumeroFase = fase.NumeroFase.ToString(),
                Nombre = fase.Nombre,
                Notas = fase.Notas,
                Completada = fase.Completada,
                Documentos = new()
            };
        }

        public async Task<IEnumerable<ServicioDto>> GetCompletadosAsync()
        {
            var servicios = await _context.Servicios
                .Include(s => s.Sucursal)
                .Include(s => s.Ataud)
                .Include(s => s.Equipo)
                .Include(s => s.ServicioEmpleados)
                    .ThenInclude(se => se.Empleado)
                .Include(s => s.Fases)
                    .ThenInclude(f => f.Documentos)
                .Where(s => s.Completado)
                .OrderByDescending(s => s.FechaInicio)
                .ToListAsync();

            return servicios.Select(MapToDto);
        }

        public async Task<FaseServicioDto?> CompletarFaseAsync(int faseId)
        {
            var fase = await _context.FasesServicio
                .Include(f => f.Documentos)
                .FirstOrDefaultAsync(f => f.Id == faseId);

            if (fase is null) return null;

            fase.Completada = true;
            fase.FechaCompletada = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapFaseToDto(fase);
        }

        public async Task<DocumentoFaseDto?> AgregarDocumentoAsync(int faseId, CrearDocumentoDto dto)
        {
            var fase = await _context.FasesServicio
                .FirstOrDefaultAsync(f => f.Id == faseId);

            if (fase is null) return null;

            var documento = new DocumentoFase
            {
                FaseServicioId = faseId,
                NombreDocumento = dto.NombreDocumento,
                Recibido = dto.Recibido,
                ArchivoUrl = dto.ArchivoUrl,
                FechaRecepcion = dto.Recibido ? DateTime.UtcNow : null
            };

            _context.DocumentosFase.Add(documento);
            await _context.SaveChangesAsync();

            return new DocumentoFaseDto
            {
                Id = documento.Id,
                NombreDocumento = documento.NombreDocumento,
                Recibido = documento.Recibido,
                ArchivoUrl = documento.ArchivoUrl,
                FechaRecepcion = documento.FechaRecepcion
            };
        }

        public async Task<DocumentoFaseDto?> MarcarDocumentoRecibidoAsync(int documentoId)
        {
            var documento = await _context.DocumentosFase
                .FirstOrDefaultAsync(d => d.Id == documentoId);

            if (documento is null) return null;

            documento.Recibido = true;
            documento.FechaRecepcion = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new DocumentoFaseDto
            {
                Id = documento.Id,
                NombreDocumento = documento.NombreDocumento,
                Recibido = documento.Recibido,
                ArchivoUrl = documento.ArchivoUrl,
                FechaRecepcion = documento.FechaRecepcion
            };
        }

        public async Task<bool> EliminarDocumentoAsync(int documentoId)
        {
            var documento = await _context.DocumentosFase
                .FirstOrDefaultAsync(d => d.Id == documentoId);

            if (documento is null) return false;

            _context.DocumentosFase.Remove(documento);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task ActualizarEstadoAtaudAsync(int ataudId, EstadoAtaud estado)
        {
            var ataud = await _context.Ataudes.FindAsync(ataudId);
            if (ataud is not null)
            {
                ataud.Estado = estado;
                await _context.SaveChangesAsync();
            }
        }

        private static ServicioDto MapToDto(Servicio s)
        {
            return new ServicioDto
            {
                Id = s.Id,
                NombreFinado = s.NombreFinado,
                FechaInicio = s.FechaInicio,
                DireccionRecoleccion = s.DireccionRecoleccion,
                MapaRecoleccionUrl = s.MapaRecoleccionUrl,
                Observaciones = s.Observaciones,
                Completado = s.Completado,
                SucursalId = s.SucursalId,
                NombreSucursal = s.Sucursal?.Nombre ?? string.Empty,
                AtaudId = s.AtaudId,
                TipoAtaud = s.Ataud?.Tipo.ToString(),
                EquipoId = s.EquipoId,
                NombreEquipo = s.Equipo?.Nombre,
                Empleados = s.ServicioEmpleados?.Select(se => new EmpleadoResumenDto
                {
                    Id = se.Empleado.Id,
                    NombreCompleto = $"{se.Empleado.Nombre} {se.Empleado.Apellidos}",
                    FotoUrl = se.Empleado.FotoUrl
                }).ToList() ?? new(),
                Fases = s.Fases?.Select(MapFaseToDto).ToList() ?? new()
            };
        }

        private static FaseServicioDto MapFaseToDto(FaseServicio f)
        {
            return new FaseServicioDto
            {
                Id = f.Id,
                NumeroFase = f.NumeroFase.ToString(),
                Nombre = f.Nombre,
                Notas = f.Notas,
                Completada = f.Completada,
                FechaCompletada = f.FechaCompletada,
                Documentos = f.Documentos?.Select(d => new DocumentoFaseDto
                {
                    Id = d.Id,
                    NombreDocumento = d.NombreDocumento,
                    Recibido = d.Recibido,
                    ArchivoUrl = d.ArchivoUrl,
                    FechaRecepcion = d.FechaRecepcion
                }).ToList() ?? new()
            };
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var servicio = await _repository.GetByIdAsync(id);
            if (servicio is null) return false;

            // Si tenía ataúd asignado, regresarlo a disponible
            if (servicio.AtaudId.HasValue)
                await ActualizarEstadoAtaudAsync(servicio.AtaudId.Value, EstadoAtaud.Disponible);

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> EliminarFaseAsync(int faseId)
        {
            var fase = await _context.FasesServicio
                .Include(f => f.Documentos)
                .FirstOrDefaultAsync(f => f.Id == faseId);

            if (fase is null) return false;

            _context.DocumentosFase.RemoveRange(fase.Documentos);
            _context.FasesServicio.Remove(fase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ServicioDto?> ActualizarAsync(int id, ActualizarServicioDto dto)
        {
            var servicio = await _repository.GetByIdAsync(id);
            if (servicio is null) return null;

            // Si cambia el ataúd, actualizar estados
            if (servicio.AtaudId != dto.AtaudId)
            {
                // Regresar el ataúd anterior a disponible
                if (servicio.AtaudId.HasValue)
                    await ActualizarEstadoAtaudAsync(servicio.AtaudId.Value, EstadoAtaud.Disponible);

                // Marcar el nuevo ataúd como vendido
                if (dto.AtaudId.HasValue)
                    await ActualizarEstadoAtaudAsync(dto.AtaudId.Value, EstadoAtaud.Vendido);
            }

            servicio.NombreFinado = dto.NombreFinado;
            servicio.DireccionRecoleccion = dto.DireccionRecoleccion;
            servicio.MapaRecoleccionUrl = dto.MapaRecoleccionUrl;
            servicio.Observaciones = dto.Observaciones;
            servicio.AtaudId = dto.AtaudId;
            servicio.EquipoId = dto.EquipoId;

            var actualizado = await _repository.UpdateAsync(servicio);
            return MapToDto(await _repository.GetWithDetallesAsync(actualizado.Id) ?? actualizado);
        }
    }
}