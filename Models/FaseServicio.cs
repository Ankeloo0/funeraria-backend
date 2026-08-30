using FunerariaApp.Models.Enums;

namespace FunerariaApp.Models
{
    public class FaseServicio
    {
        public int Id { get; set; }
        public NumeroFase NumeroFase { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Notas { get; set; }
        public bool Completada { get; set; } = false;
        public DateTime? FechaCompletada { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; } = null!;

        public ICollection<DocumentoFase> Documentos { get; set; } = new List<DocumentoFase>();
    }
}