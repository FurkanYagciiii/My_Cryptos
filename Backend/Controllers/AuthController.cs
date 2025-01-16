using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // kayit olma endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            // kullanicicin var olup olmamasina bakiyor
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Boyle bir kullanici adi var.");
            }

            // kullaniciyi database ekler
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Kayit Basarili.");
        }

        // Giris endopinti jwt tokenine kullanarak
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            // kullanici adini bulma
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return BadRequest("Kullanici bulunamadi.");
            }

            // sifre kontrolu
            if (user.Password != request.Password)
            {
                return BadRequest("Yanlis sifre.");
            }

            // token olusturuyor
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token
            });
        }

        // JWT token olusturma
        private string GenerateJwtToken(User user)
        {
            // JWT oluşturmak için kullanılan anahtarı alır
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

            // Anahtar ile şifreleme kimlik bilgilerini oluşturur
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token'de yer alacak claimler
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // id
        new Claim(JwtRegisteredClaimNames.Sub, user.Username), // kullanıcı adı
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // 
        
    };

            // JWT token oluşturma  
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],       // 
                audience: _configuration["JwtSettings:Issuer"],     // 
                claims: claims,                                     // 
                expires: DateTime.Now.AddDays(1),                  // 
                signingCredentials: creds                          // 
            );

            // Token'i string olarak döndür
            return new JwtSecurityTokenHandler().WriteToken(token);
        }





        // Protected Endpoint 
        [Authorize]
        [HttpGet("protected-endpoint")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok("This is a protected endpoint. You are authenticated!");
        }
    }

    // DTO for login requests
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
