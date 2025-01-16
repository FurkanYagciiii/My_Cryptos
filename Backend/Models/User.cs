namespace UserAuthAPI.Models
{
    // Kullanıcı modelini güncelleme
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<FavoriteCrypto> FavoriteCryptos { get; set; } = new List<FavoriteCrypto>();
    }

    // Favori kripto para modeli
    public class FavoriteCrypto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CryptoId { get; set; }
        public User User { get; set; }
    }
}
