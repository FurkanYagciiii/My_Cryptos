using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using UserAuthAPI.Models;
using Microsoft.Extensions.Configuration;

public interface IFavoriteCryptoService
{
    Task<List<FavoriteCrypto>> GetUserFavoriteCryptosAsync(int userId);
    Task AddFavoriteCryptoAsync(int userId, string cryptoId);
    Task RemoveFavoriteCryptoAsync(int userId, string cryptoId); // Silme fonksiyonu
}

public class FavoriteCryptoService : IFavoriteCryptoService
{
    private readonly IFavoriteCryptoRepository _favoriteCryptoRepository;

    public FavoriteCryptoService(IFavoriteCryptoRepository favoriteCryptoRepository)
    {
        _favoriteCryptoRepository = favoriteCryptoRepository;
    }

    public async Task<List<FavoriteCrypto>> GetUserFavoriteCryptosAsync(int userId)
    {
        return await _favoriteCryptoRepository.GetUserFavoriteCryptosAsync(userId);
    }
    public async Task RemoveFavoriteCryptoAsync(int userId, string cryptoId)
    {
        await _favoriteCryptoRepository.RemoveFavoriteCryptoAsync(userId, cryptoId);
    }

    public async Task AddFavoriteCryptoAsync(int userId, string cryptoId)
    {
        await _favoriteCryptoRepository.AddFavoriteCryptoAsync(userId, cryptoId);
    }
}
