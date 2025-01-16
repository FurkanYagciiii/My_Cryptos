using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        // Kullanıcı bilgilerini alma
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı doğrulanamadı.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(new
            {
                user.Username,
                user.Password, // 
                user.FavoriteCryptos
            });
        }

        // 
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto userUpdateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı doğrulanamadı.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            user.Username = userUpdateDto.Username ?? user.Username;
            user.Password = userUpdateDto.Password ?? user.Password; // 

            await _context.SaveChangesAsync();

            return Ok("Kullanıcı bilgileri güncellendi.");
        }

        // Kullanıcı hesabını silme
        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı doğrulanamadı.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Hesap başarıyla silindi.");
        }

        // Çıkış yapma
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //
            return Ok("Çıkış yapıldı.");
        }
    }

    // 
    public class UserUpdateDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
