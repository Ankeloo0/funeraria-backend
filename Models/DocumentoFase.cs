namespace FunerariaApp.Models
{
    public class DocumentoFase
    {
        public int Id { get; set; }
        public string NombreDocumento { get; set; } = string.Empty;
        public bool Recibido { get; set; } = false;
        public string? ArchivoUrl { get; set; }
        public DateTime? FechaRecepcion { get; set; }

        public int FaseServicioId { get; set; }
        public FaseServicio FaseServicio { get; set; } = null!;
    }
}