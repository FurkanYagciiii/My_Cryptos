using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class CryptoService
{
    public async Task<List<Crypto>> GetTopCryptos()
    {
        var topCryptos = new List<string>
        {
            "bitcoin", "ethereum", "tron", "tether",
            "cardano", "ripple", "solana", "polkadot",
            "dogecoin", "binancecoin", "avalanche",
            "litecoin", "uniswap", "chainlink", "shiba-inu"
        };

        var ids = string.Join(",", topCryptos);
        var url = $"https://api.coingecko.com/api/v3/simple/price?ids={ids}&vs_currencies=usd&include_24hr_change=true";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var cryptoData = JsonSerializer.Deserialize<Dictionary<string, CryptoData>>(content, options);

        var cryptos = cryptoData.Select(c => new Crypto
        {
            Symbol = c.Key.ToUpper(),
            Price = c.Value.Usd,
            PercentageChange24h = c.Value.Usd_24h_change,
            LastUpdated = DateTime.UtcNow
        }).ToList();

        return cryptos;
    }



    private readonly HttpClient _httpClient;

    public CryptoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, CryptoData>> GetCryptoData(List<string> favoriteCryptos)
    {
        var ids = string.Join(",", favoriteCryptos);
        var url = $"https://api.coingecko.com/api/v3/simple/price?ids={ids}&vs_currencies=usd&include_24hr_change=true";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, CryptoData>>(content);

        return data;
    }
}


public class CryptoData
{
    public decimal Usd { get; set; }
    public decimal Usd_24h_change { get; set; }
}
