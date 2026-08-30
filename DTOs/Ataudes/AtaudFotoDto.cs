namespace FunerariaApp.DTOs.Ataudes
{
    public class AtaudFotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public int Orden { get; set; }
    }
}