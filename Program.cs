using FunerariaApp.Data;
using FunerariaApp.Models;
using FunerariaApp.Repositories;
using FunerariaApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Base de datos
builder.Services.AddDbContext<FunerariaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FunerariaPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Repositorios
builder.Services.AddScoped<IRepository<Sucursal>, SucursalRepository>();
builder.Services.AddScoped<IRepository<Empleado>, EmpleadoRepository>();
builder.Services.AddScoped<IAtaudRepository, AtaudRepository>();
builder.Services.AddScoped<IRepository<Equipo>, EquipoRepository>();
builder.Services.AddScoped<IRentaRepository, RentaRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();

// Servicios
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SucursalService>();
builder.Services.AddScoped<EmpleadoService>();
builder.Services.AddScoped<AtaudService>();
builder.Services.AddScoped<EquipoService>();
builder.Services.AddScoped<RentaService>();
builder.Services.AddScoped<ServicioService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ArchivoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("FunerariaPolicy");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();