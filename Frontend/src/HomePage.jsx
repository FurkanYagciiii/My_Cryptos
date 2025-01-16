import React, { useEffect, useState } from "react";
import api from "./api";
import "./HomePage.css";
import Navbar from "./HomeNav";

function HomePage() {
  const [news, setNews] = useState([]);
  const [topCryptos, setTopCryptos] = useState([]); // Popüler kriptolar
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true); // Yüklenme durumu için state

  // Haberleri almak için useEffect
  useEffect(() => {
    const fetchNews = async () => {
      try {
        const newsResponse = await api.get("/News/get-news"); // /News/get-news endpoint'inden haberleri al
        setNews(newsResponse.data.slice(0, 6)); // Sadece ilk 6 haberi al
      } catch (err) {
        setError("Haberler alınamadı: " + err.message);
      }
    };

    fetchNews();

    // Verileri her 30 saniyede bir güncellemek için interval kullanabilirsiniz
    const newsInterval = setInterval(fetchNews, 300000); // 30 saniyede bir haberleri çek

    return () => clearInterval(newsInterval); // Komponent unmount olduğunda interval'ı temizle
  }, []);

  // Kripto paraları almak için useEffect
  useEffect(() => {
    const fetchCryptos = async () => {
      try {
        const topCryptosResponse = await api.get("/crypto/top-cryptos"); // Popüler kripto paraları al
        setTopCryptos(topCryptosResponse.data.data); // Kripto paraları state'e aktar
      } catch (err) {
        setError("Kripto verileri alınamadı: " + err.message);
      } finally {
        setLoading(false); // Yüklenme durumunu kapat
      }
    };

    fetchCryptos();

    // Kripto verilerini her 30 saniyede bir güncellemek için interval kullanabilirsiniz
    const cryptoInterval = setInterval(fetchCryptos, 300000); // 30 saniyede bir kripto paraları çek

    return () => clearInterval(cryptoInterval); // Komponent unmount olduğunda interval'ı temizle
  }, []);

  if (loading) {
    return <div className="loading">Veriler yükleniyor...</div>;
  }

  return (
    <>
      <Navbar />
      <div className="container">
        <h1>Haberler</h1>
        <div className="news-section">
          {news.map((item, index) => (
            <div key={index} className="news-card">
              <img
                src={item.urlToImage || "https://via.placeholder.com/300"}
                alt={item.title || "Haber görseli"}
              />
              <div className="news-card-content">
                <h3 className="news-card-title">{item.title || "Başlık Yok"}</h3>
                <p className="news-card-description">
                  {item.description || "Açıklama bulunmuyor."}
                </p>
                <a
                  href={item.url}
                  className="news-card-link"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  Habere Git
                </a>
              </div>
            </div>
          ))}
        </div>

        <h1>En Popüler Kripto Paralar</h1>
        <div className="crypto-table-container">
          <table className="crypto-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Ad</th>
                <th>Fiyat (USD)</th>
                <th>24 Saatlik Değişim</th>
              </tr>
            </thead>
            <tbody>
              {topCryptos.map((crypto, index) => (
                <tr key={index}>
                  <td>{index + 1}</td>
                  <td>{crypto.symbol}</td>
                  <td>${crypto.price.toFixed(2)}</td>
                  <td
                    className={
                      crypto.percentageChange24h > 0 ? "positive" : "negative"
                    }
                  >
                    {crypto.percentageChange24h.toFixed(2)}%
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {error && <p className="error-message">{error}</p>}
      </div>
    </>
  );
}

export default HomePage;
