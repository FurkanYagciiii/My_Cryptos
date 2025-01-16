import React, { useState, useEffect } from "react";
import api from "./api";
import "./FavoriteCryptos.css";
import Navbar from "./HomeNav";
import { jwtDecode } from "jwt-decode";
import { Line } from "react-chartjs-2";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

const FavoriteCryptos = () => {
  const [favorites, setFavorites] = useState([]);
  const [cryptoId, setCryptoId] = useState("");
  const [cryptoList, setCryptoList] = useState([]);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [chartData, setChartData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [selectedCryptoName, setSelectedCryptoName] = useState("");

  const getUserIdFromToken = () => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    try {
      const decodedToken = jwtDecode(token);
      return decodedToken.sub;
    } catch (err) {
      console.error("Token çözümleme hatası:", err);
      return null;
    }
  };

  const userId = getUserIdFromToken();

  useEffect(() => {
    const fetchCryptoList = async () => {
      try {
        const response = await api.get("/cryptocurrencies");
        const formattedData = response.data.map((crypto) => ({
          id: crypto.cryptoId,
          name: crypto.name,
          icon: crypto.iconUrl,
        }));
        setCryptoList(formattedData);
      } catch (err) {
        console.error("Kripto listesi alınamadı:", err);
      }
    };

    fetchCryptoList();
  }, []);

  useEffect(() => {
    const fetchFavorites = async () => {
      if (!userId) return;
      const token = localStorage.getItem("token");
      try {
        const response = await api.get(`/favoritecrypto/user/favorites`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        const favoritesWithDetails = response.data.map((fav) => {
          const cryptoDetails = cryptoList.find(
            (crypto) => crypto.id === fav.cryptoId
          );

          return {
            cryptoId: fav.cryptoId,
            name: cryptoDetails?.name || fav.cryptoId,
            icon: cryptoDetails?.icon || "",
          };
        });

        setFavorites(favoritesWithDetails);
      } catch (err) {
        console.error("Favori kriptolar alınamadı:", err);
      }
    };

    fetchFavorites();
  }, [cryptoList, userId]);

  const handleRemoveFavoriteCrypto = async (cryptoId) => {
    const token = localStorage.getItem("token");
    if (!token) return;

    try {
      const response = await api.delete(
        `/favoritecrypto/user/favorites/remove/${cryptoId}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );

      if (response.status === 200) {
        setFavorites((prevFavorites) =>
          prevFavorites.filter((fav) => fav.cryptoId !== cryptoId)
        );
      }
    } catch (err) {
      console.error("Favori kripto silinirken bir hata oluştu:", err);
    }
  };

  const handleAddFavoriteCrypto = async (e) => {
    e.preventDefault();

    if (favorites.some((fav) => fav.cryptoId === cryptoId)) {
      setErrorMessage("Bu kripto para zaten favorilerde mevcut.");
      return;
    }

    const token = localStorage.getItem("token");

    if (token && cryptoId) {
      try {
        const response = await api.post(
          `/favoritecrypto/user/favorites/add`,
          cryptoId,
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (response.status === 200) {
          const addedCrypto = cryptoList.find(
            (crypto) => crypto.id === cryptoId
          );
          setFavorites((prevFavorites) => [
            ...prevFavorites,
            { cryptoId, name: addedCrypto.name, icon: addedCrypto.icon },
          ]);
          setCryptoId("");
          setErrorMessage("");
        }
      } catch (err) {
        console.error("Favori kripto eklenirken bir hata oluştu:", err);
      }
    }
  };

  const handleShowChart = async (cryptoId, cryptoName, interval) => {
    setLoading(true);
    setSelectedCryptoName(cryptoName);
    try {
      const response = await api.get(`/CryptoChart/${cryptoId}/${interval}`);

      const data = response.data;

      const labels = data.prices.map((price) =>
        new Date(price[0]).toLocaleDateString()
      );
      const values = data.prices.map((price) => price[1]);

      setChartData({
        labels,
        datasets: [
          {
            label: `${cryptoName} - ${interval.toUpperCase()} Fiyat`,
            data: values,
            borderColor: "rgba(75,192,192,1)",
            borderWidth: 2,
            fill: false,
          },
        ],
      });
      setShowModal(true);
    } catch (err) {
      console.error("Grafik verisi alınamadı:", err);
      setErrorMessage("Grafik verisi alınamadı.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Navbar />
      <div className="favorite-crypto-container">
        <h2>Favori Kripto Ekle</h2>
        {errorMessage && <p className="error-message">{errorMessage}</p>}
        <div
          className="custom-dropdown"
          onClick={() => setDropdownOpen(!dropdownOpen)}
        >
          <div className="custom-dropdown-selected">
            {cryptoId
              ? cryptoList.find((crypto) => crypto.id === cryptoId)?.name ||
                "Kripto Para Seçin"
              : "Kripto Para Seçin"}
          </div>
          {dropdownOpen && (
            <ul className="custom-dropdown-options">
              {cryptoList.map((crypto) => (
                <li
                  key={crypto.id}
                  className="custom-dropdown-option"
                  onClick={() => {
                    setCryptoId(crypto.id);
                    setDropdownOpen(false);
                  }}
                >
                  <img src={crypto.icon} alt={crypto.name} width="20" height="20" />
                  <span>{crypto.name}</span>
                </li>
              ))}
            </ul>
          )}
        </div>
        <button
          className="favorite-crypto-button"
          onClick={handleAddFavoriteCrypto}
          disabled={!cryptoId}
        >
          Ekle
        </button>
        <h1 className="favorite-crypto-header">Favori Kriptolar</h1>

        <ul className="favorite-crypto-list">
          {favorites.length === 0 ? (
            <p>Henüz favori kripto eklemediniz.</p>
          ) : (
            favorites.map((crypto, index) => (
              <li key={index} className="favorite-crypto-item">
                <img
                  src={crypto.icon}
                  alt={`${crypto.name} icon`}
                  width="30"
                  height="30"
                />
                <span>{crypto.name}</span>
                <div className="interval-buttons">
                  <button
                    onClick={() =>
                      handleShowChart(crypto.cryptoId, crypto.name, "daily")
                    }
                  >
                    Günlük
                  </button>
                  <button
                    onClick={() =>
                      handleShowChart(crypto.cryptoId, crypto.name, "weekly")
                    }
                  >
                    Haftalık
                  </button>
                  <button
                    onClick={() =>
                      handleShowChart(crypto.cryptoId, crypto.name, "monthly")
                    }
                  >
                    Aylık
                  </button>
                </div>
                <button
                  className="remove-button"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleRemoveFavoriteCrypto(crypto.cryptoId);
                  }}
                >
                  Sil
                </button>
              </li>
            ))
          )}
        </ul>
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button
              className="close-button"
              onClick={() => setShowModal(false)}
            >
              Kapat
            </button>
            <h2>{selectedCryptoName} Grafiği</h2>
            {loading ? <p>Grafik yükleniyor...</p> : <Line data={chartData} />}
          </div>
        </div>
      )}
    </>
  );
};

export default FavoriteCryptos;
