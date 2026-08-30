using FunerariaApp.Data;
using FunerariaApp.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FunerariaApp.Services
{
    public class AuthService
    {
        private readonly FunerariaDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(FunerariaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == dto.NombreUsuario);

            if (usuario is null) return null;

            var passwordValido = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
            if (!passwordValido) return null;

            return GenerarToken(usuario.NombreUsuario);
        }

        private TokenDto GenerarToken(string nombreUsuario)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, nombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiracion = int.Parse(_configuration["Jwt:ExpirationHours"]!);
            var fechaExpiracion = DateTime.UtcNow.AddHours(expiracion);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: fechaExpiracion,
                signingCredentials: credenciales
            );

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = fechaExpiracion
            };
        }
    }
}