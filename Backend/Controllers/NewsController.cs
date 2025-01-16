using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly NewsService _newsService;
    private readonly NewsRepository _newsRepository;

    public NewsController(NewsService newsService, NewsRepository newsRepository)
    {
        _newsService = newsService;
        _newsRepository = newsRepository;
    }

    // 
    [HttpGet("fetch-and-save")]
    public IActionResult FetchAndSaveNews()
    {
        try
        {
            // 
            var newsList = _newsService.FetchCryptoNews();

            // 
            _newsRepository.SaveNewsAsync(newsList).Wait();

            return Ok(new { message = "News fetched and saved successfully.", count = newsList.Count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching news.", error = ex.Message });
        }
    }

    // 
    [HttpGet("get-news")]
    public async Task<IActionResult> GetNews()
    {
        try
        {
            var newsList = await _newsRepository.GetAllNewsAsync();
            return Ok(newsList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving news.", error = ex.Message });
        }
    }
}
