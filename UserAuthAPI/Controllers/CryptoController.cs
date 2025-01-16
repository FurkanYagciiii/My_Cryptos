using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using UserAuthAPI.Data;
using UserAuthAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly CryptoService _cryptoService;
        private readonly CryptoRepository _cryptoRepository;

        public CryptoController(DataContext context, CryptoService cryptoService, CryptoRepository cryptoRepository)
        {
            _context = context;
            _cryptoService = cryptoService;
            _cryptoRepository = cryptoRepository;
        }
        [HttpGet("top-cryptos")]
        public async Task<IActionResult> GetTopCryptos()
        {
            try
            {
                // 
                var topCryptos = await _cryptoService.GetTopCryptos();

                // 
                await _cryptoRepository.SaveCryptoData(topCryptos);

                // 
                return Ok(new { message = "Top cryptos fetched and saved successfully.", data = topCryptos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching and saving top cryptos.", error = ex.Message });
            }
        }

    }
}
