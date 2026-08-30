using FunerariaApp.DTOs.Archivos;

namespace FunerariaApp.Services
{
    public class ArchivoService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        private readonly string[] _tiposImagenPermitidos = {
            "image/jpeg", "image/png", "image/webp"
        };

        private readonly string[] _tiposPdfPermitidos = {
            "application/pdf"
        };

        private const long MaxTamanoBytes = 10 * 1024 * 1024; // 10MB

        public ArchivoService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public async Task<ArchivoDto> SubirImagenAsync(IFormFile archivo)
        {
            ValidarArchivo(archivo, _tiposImagenPermitidos);
            return await GuardarArchivoAsync(archivo, "imagenes");
        }

        public async Task<ArchivoDto> SubirPdfAsync(IFormFile archivo)
        {
            ValidarArchivo(archivo, _tiposPdfPermitidos);
            return await GuardarArchivoAsync(archivo, "documentos");
        }

        private void ValidarArchivo(IFormFile archivo, string[] tiposPermitidos)
        {
            if (archivo.Length == 0)
                throw new ArgumentException("El archivo está vacío");

            if (archivo.Length > MaxTamanoBytes)
                throw new ArgumentException("El archivo supera el tamaño máximo de 10MB");

            if (!tiposPermitidos.Contains(archivo.ContentType.ToLower()))
                throw new ArgumentException($"Tipo de archivo no permitido: {archivo.ContentType}");
        }

        private async Task<ArchivoDto> GuardarArchivoAsync(IFormFile archivo, string subcarpeta)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", subcarpeta);

            // Crear carpeta si no existe
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Generar nombre único para evitar colisiones
            var extension = Path.GetExtension(archivo.FileName).ToLower();
            var nombreUnico = $"{Guid.NewGuid()}{extension}";
            var rutaCompleta = Path.Combine(uploadsPath, nombreUnico);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            // Construir URL pública
            var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:7065";
            var url = $"{baseUrl}/uploads/{subcarpeta}/{nombreUnico}";

            return new ArchivoDto
            {
                Url = url,
                NombreArchivo = archivo.FileName,
                Tipo = archivo.ContentType
            };
        }

        public bool EliminarArchivo(string url)
        {
            try
            {
                var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:7065";
                var rutaRelativa = url.Replace(baseUrl, "").TrimStart('/');
                var rutaCompleta = Path.Combine(_env.WebRootPath, rutaRelativa.Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}