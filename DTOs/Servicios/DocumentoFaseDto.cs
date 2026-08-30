namespace FunerariaApp.DTOs.Servicios
{
    public class DocumentoFaseDto
    {
        public int Id { get; set; }
        public string NombreDocumento { get; set; } = string.Empty;
        public bool Recibido { get; set; }
        public string? ArchivoUrl { get; set; }
        public DateTime? FechaRecepcion { get; set; }
    }
}