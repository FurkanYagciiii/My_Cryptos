using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using UserAuthAPI.Models;
using Microsoft.Extensions.Configuration;

public class NewsService
{
    private readonly NewsApiClient _newsApiClient;

    public NewsService(IConfiguration configuration)
    {
        var apiKey = configuration["ApiSettings:NewsApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("NewsAPI key is missing in configuration.");
        }

        // NewsApiClient nesnesini başlatıyoruz
        _newsApiClient = new NewsApiClient(apiKey);
    }

    public List<News> FetchCryptoNews()
    {

        var articlesResponse = _newsApiClient.GetEverything(new EverythingRequest
        {
            Q = "crypto",
            SortBy = SortBys.Popularity,
            Language = Languages.EN,
            From = DateTime.UtcNow.AddDays(-7)
        });

        if (articlesResponse.Status != Statuses.Ok)
        {
            throw new Exception($"NewsAPI Error: {articlesResponse.Error?.Message}");
        }


        return articlesResponse.Articles.Select(a => new News
        {
            Title = a.Title ?? "No Title",
            Description = a.Description ?? "No Description",
            Url = a.Url ?? "No URL",
            UrlToImage = a.UrlToImage ?? "No Image",
            PublishedAt = a.PublishedAt ?? DateTime.UtcNow
        }).ToList();
    }
}
