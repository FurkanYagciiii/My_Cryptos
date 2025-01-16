using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Models;

namespace UserAuthAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<FavoriteCrypto> FavoriteCryptos { get; set; }  //
        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    }
}
