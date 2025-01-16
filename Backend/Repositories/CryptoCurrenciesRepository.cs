using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

public class CryptoCurrenciesRepository
{
    private readonly DataContext _context;

    public CryptoCurrenciesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Cryptocurrency>> GetAllCryptocurrenciesAsync()
    {
        return await _context.Cryptocurrencies.ToListAsync();
    }
}
