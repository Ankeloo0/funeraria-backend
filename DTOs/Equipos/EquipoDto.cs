namespace FunerariaApp.DTOs.Equipos
{
    public class EquipoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public bool Disponible { get; set; }
    }
}