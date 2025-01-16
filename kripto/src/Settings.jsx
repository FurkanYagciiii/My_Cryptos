import React, { useState, useEffect } from "react";
import api from "./api";
import Navbar from "./HomeNav";
import "./Settings.css";

const Settings = () => {
  const [user, setUser] = useState({ username: "", password: "" });
  const [message, setMessage] = useState("");

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await api.get("/user/profile", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        });
        setUser(response.data);
      } catch (error) {
        console.error("Kullanıcı bilgileri alınamadı:", error);
      }
    };

    fetchUserData();
  }, []);

  const handleUpdate = async () => {
    try {
      const response = await api.put("/user/profile", user, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      setMessage(response.data);
    } catch (error) {
      console.error("Kullanıcı bilgileri güncellenemedi:", error);
    }
  };

  const handleDelete = async () => {
    const confirmed = window.confirm("Hesabınızı silmek istediğinize emin misiniz?");
    if (!confirmed) return;

    try {
      await api.delete("/user/profile", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      localStorage.removeItem("token");
      window.location.href = "/";
    } catch (error) {
      console.error("Hesap silinemedi:", error);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    window.location.href = "/";
  };

  return (
    <>
      <Navbar />
      <div className="settings-container">
        <h1>Kullanıcı Ayarları</h1>
        {message && <p className="message">{message}</p>}
        <input
          type="text"
          value={user.username}
          onChange={(e) => setUser({ ...user, username: e.target.value })}
          placeholder="Kullanıcı Adı"
        />
        <input
          type="password"
          value={user.password}
          onChange={(e) => setUser({ ...user, password: e.target.value })}
          placeholder="Şifre"
        />
        <div className="settings-buttons">
          <button className="update-button" onClick={handleUpdate}>Güncelle</button>
          <button onClick={handleDelete} className="delete-account-button">
            Hesabı Sil
          </button>
          <button onClick={handleLogout} className="logout-button">
            Çıkış Yap
          </button>
        </div>
      </div>
    </>
  );
  
};

export default Settings;
