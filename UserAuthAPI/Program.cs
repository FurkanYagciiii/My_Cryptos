using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Diğer servisler...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabanı bağlama
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servis ve repository kayıtları
builder.Services.AddScoped<IFavoriteCryptoService, FavoriteCryptoService>();
builder.Services.AddScoped<IFavoriteCryptoRepository, FavoriteCryptoRepository>();

// CryptoService ve NewsService
builder.Services.AddHttpClient<CryptoService>(); // CryptoService için HttpClient eklendi
builder.Services.AddScoped<CryptoService>();     // CryptoService scoped olarak tanımlandı
builder.Services.AddScoped<CryptoRepository>();

builder.Services.AddHttpClient<NewsService>();
builder.Services.AddScoped<NewsService>(); // Singleton yerine scoped kullanımı
builder.Services.AddScoped<NewsRepository>();
// CryptoDataService servisini ekliyoruz
builder.Services.AddHttpClient<CryptoDataService>();
builder.Services.AddScoped<CryptoDataService>();

builder.Services.AddScoped<CryptoCurrenciesRepository>();
builder.Services.AddScoped<CryptoCurrenciesService>();

// CORS ayarları
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// JWT Authentication ayarları
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
{
    throw new Exception("JWT ayarları eksik veya geçersiz.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

var app = builder.Build();

// Swagger UI ve geliştirme ortamı
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS politikası
app.UseCors("AllowAll");

// JWT Authentication
app.UseAuthentication();
app.UseAuthorization();

// Kontrolleri ekle
app.MapControllers();

app.Run();
