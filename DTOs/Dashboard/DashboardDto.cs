using FunerariaApp.DTOs.Rentas;

namespace FunerariaApp.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int AtaudesDisponibles { get; set; }
        public int AtaudesApartados { get; set; }
        public int AtaudesVendidos { get; set; }
        public int EquiposDisponibles { get; set; }
        public int EquiposRentados { get; set; }
        public int ServiciosActivos { get; set; }
        public int RentasPorVencer { get; set; }
        public int RentasVencidas { get; set; }
        public List<RentaDto> DetalleRentasPorVencer { get; set; } = new();
        public List<RentaDto> DetalleRentasVencidas { get; set; } = new();
        public EmpleadoMesDto? EmpleadoDelMes { get; set; }
    }
}   