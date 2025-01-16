using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class CryptoDataService
{
    private readonly HttpClient _httpClient;

    public CryptoDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<object> GetCryptoDataAsync(string cryptoId, string interval)
    {
        string days = interval switch
        {
            "daily" => "1",
            "weekly" => "7",
            "monthly" => "30",
            _ => throw new ArgumentException("Geçersiz zaman aralığı."),
        };

        string apiUrl = $"https://api.coingecko.com/api/v3/coins/{cryptoId}/market_chart?vs_currency=usd&days={days}";
        var response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("CoinGecko API isteği başarısız oldu.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<object>(jsonResponse);

        return data; // 
    }
}
