using UserAuthAPI.Models;

public class CryptoCurrenciesService
{
    private readonly CryptoCurrenciesRepository _repository;

    public CryptoCurrenciesService(CryptoCurrenciesRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Cryptocurrency>> GetAllCryptocurrenciesAsync()
    {
        return await _repository.GetAllCryptocurrenciesAsync();
    }
}
