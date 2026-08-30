namespace FunerariaApp.Models
{
    public class AtaudFoto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; } = false;
        public int Orden { get; set; }

        // Relación
        public int AtaudId { get; set; }
        public Ataud Ataud { get; set; } = null!;
    }
}