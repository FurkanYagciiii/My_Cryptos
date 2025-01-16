using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

public interface IFavoriteCryptoRepository
{
    Task<List<FavoriteCrypto>> GetUserFavoriteCryptosAsync(int userId);
    Task AddFavoriteCryptoAsync(int userId, string cryptoId);
    Task RemoveFavoriteCryptoAsync(int userId, string cryptoId); // Yeni metod
}

public class FavoriteCryptoRepository : IFavoriteCryptoRepository
{
    private readonly DataContext _context;

    public FavoriteCryptoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task RemoveFavoriteCryptoAsync(int userId, string cryptoId)
    {
        var favoriteCrypto = await _context.FavoriteCryptos
                                           .FirstOrDefaultAsync(fc => fc.UserId == userId && fc.CryptoId == cryptoId);

        if (favoriteCrypto != null)
        {
            _context.FavoriteCryptos.Remove(favoriteCrypto);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<FavoriteCrypto>> GetUserFavoriteCryptosAsync(int userId)
    {
        return await _context.FavoriteCryptos
                             .Where(fc => fc.UserId == userId)
                             .ToListAsync();
    }

    public async Task AddFavoriteCryptoAsync(int userId, string cryptoId)
    {
        var favoriteCrypto = new FavoriteCrypto
        {
            UserId = userId,
            CryptoId = cryptoId
        };

        _context.FavoriteCryptos.Add(favoriteCrypto);
        await _context.SaveChangesAsync();
    }
}
