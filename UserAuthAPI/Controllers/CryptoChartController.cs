using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CryptoChartController : ControllerBase
{
    private readonly CryptoDataService _cryptoDataService;

    public CryptoChartController(CryptoDataService cryptoDataService)
    {
        _cryptoDataService = cryptoDataService;
    }

    [HttpGet("{cryptoId}/{interval}")]
    public async Task<IActionResult> GetChartData(string cryptoId, string interval)
    {
        try
        {
            if (interval != "daily" && interval != "weekly" && interval != "monthly")
            {
                return BadRequest("Geçersiz zaman aralığı. 'daily', 'weekly', veya 'monthly' olmalı.");
            }

            var data = await _cryptoDataService.GetCryptoDataAsync(cryptoId, interval);
            return Ok(data);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
