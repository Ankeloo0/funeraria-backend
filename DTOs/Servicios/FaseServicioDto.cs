namespace FunerariaApp.DTOs.Servicios
{
    public class FaseServicioDto
    {
        public int Id { get; set; }
        public string NumeroFase { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Notas { get; set; }
        public bool Completada { get; set; }
        public DateTime? FechaCompletada { get; set; }
        public List<DocumentoFaseDto> Documentos { get; set; } = new();
    }
}