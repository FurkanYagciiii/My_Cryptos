using UserAuthAPI.Data;
using UserAuthAPI.Models;
using Microsoft.EntityFrameworkCore;

public class NewsRepository
{
    private readonly DataContext _context;

    public NewsRepository(DataContext context)
    {
        _context = context;
    }


    public async Task SaveNewsAsync(List<News> newsList)
    {
        if (newsList == null || newsList.Count == 0)
        {
            throw new ArgumentException("The news list is empty or null.");
        }

        foreach (var news in newsList)
        {
            // 
            var exists = await _context.News.AnyAsync(n => n.Url == news.Url);
            if (!exists)
            {
                await _context.News.AddAsync(news);
            }
        }

        // Değişiklikleri kaydediyoruz
        await _context.SaveChangesAsync();
    }


    public async Task<List<News>> GetAllNewsAsync()
    {
        return await _context.News
            .OrderByDescending(n => n.PublishedAt) // Yayınlanma tarihine göre sıralarrr 
            .ToListAsync();
    }
}
