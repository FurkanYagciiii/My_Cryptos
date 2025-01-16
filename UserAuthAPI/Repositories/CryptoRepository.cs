using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

public class CryptoRepository
{
    private readonly DataContext _context;

    public CryptoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task SaveCryptoData(List<Crypto> cryptos)
    {
        foreach (var crypto in cryptos)
        {

            var existingCrypto = await _context.Cryptos.FirstOrDefaultAsync(c => c.Symbol == crypto.Symbol);
            if (existingCrypto == null)
            {
                _context.Cryptos.Add(crypto);
            }
            else
            {
                existingCrypto.Price = crypto.Price;
                existingCrypto.PercentageChange24h = crypto.PercentageChange24h;
                existingCrypto.LastUpdated = crypto.LastUpdated;
            }
        }

        await _context.SaveChangesAsync();
    }




}
