namespace FunerariaApp.Models
{
    public class Vela
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public string Servicio { get; set; } = string.Empty;
    }
}