using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class CryptocurrenciesController : ControllerBase
{
    private readonly CryptoCurrenciesService _service;

    public CryptocurrenciesController(CryptoCurrenciesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cryptocurrency>>> Get()
    {
        var cryptocurrencies = await _service.GetAllCryptocurrenciesAsync();
        return Ok(cryptocurrencies);
    }
}
