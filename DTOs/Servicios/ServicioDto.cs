namespace FunerariaApp.DTOs.Servicios
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string NombreFinado { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public string DireccionRecoleccion { get; set; } = string.Empty;
        public string? MapaRecoleccionUrl { get; set; }
        public string? Observaciones { get; set; }
        public bool Completado { get; set; }
        public int SucursalId { get; set; }
        public string NombreSucursal { get; set; } = string.Empty;
        public int? AtaudId { get; set; }
        public string? TipoAtaud { get; set; }
        public int? EquipoId { get; set; }
        public string? NombreEquipo { get; set; }
        public List<EmpleadoResumenDto> Empleados { get; set; } = new();
        public List<FaseServicioDto> Fases { get; set; } = new();
    }
}