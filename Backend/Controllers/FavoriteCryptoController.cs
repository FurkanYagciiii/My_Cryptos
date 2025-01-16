using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class FavoriteCryptoController : ControllerBase
{
    private readonly IFavoriteCryptoService _favoriteCryptoService;

    public FavoriteCryptoController(IFavoriteCryptoService favoriteCryptoService)
    {
        _favoriteCryptoService = favoriteCryptoService;
    }

    // 
    [Authorize]
    [HttpGet("user/favorites")]
    public async Task<IActionResult> GetUserFavoriteCryptos()
    {
        try
        {
            // 
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var favoriteCryptos = await _favoriteCryptoService.GetUserFavoriteCryptosAsync(userId);
            if (favoriteCryptos == null || !favoriteCryptos.Any())
                return NotFound("No favorite cryptos found for this user.");

            return Ok(favoriteCryptos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // 
    [Authorize]
    [HttpPost("user/favorites/add")]
    public async Task<IActionResult> AddFavoriteCrypto([FromBody] string cryptoId)
    {
        try
        {
            if (string.IsNullOrEmpty(cryptoId))
            {
                return BadRequest("Crypto ID is required.");
            }

            // JWT Token'dan kullanıcı ID'sini al
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _favoriteCryptoService.AddFavoriteCryptoAsync(userId, cryptoId);
            return Ok($"Favorite crypto {cryptoId} added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpDelete("user/favorites/remove/{cryptoId}")]
    public async Task<IActionResult> RemoveFavoriteCrypto(string cryptoId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _favoriteCryptoService.RemoveFavoriteCryptoAsync(userId, cryptoId);
            return Ok($"Favorite crypto {cryptoId} removed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
