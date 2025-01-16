import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // Sayfa yönlendirme için
import api from "./api";
import "./RegisterForm.css"; // CSS dosyasını import edin
import Navbar from "./navbar";

function RegisterForm() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const navigate = useNavigate(); // Yönlendirme için hook

  const handleRegister = async (e) => {
    e.preventDefault();

    try {
      // Token olmadan kayıt isteği gönder
      const response = await api.post(
        "auth/register",
        { username, password },
        { headers: { useToken: false } } // Token kullanımı kapalı
      );
      setMessage(response.data || "Kayıt başarılı!");
    } catch (error) {
      setMessage(
        error.response?.data?.message || "Kayıt başarısız. Lütfen tekrar deneyin."
      );
    }
  };

  return (
    <>
      <Navbar />
      <div className="container">
      <div className="card">
        <h2>Kayıt Ol</h2>
        <form onSubmit={handleRegister}>
          <div>
            <label>Kullanıcı Adı:</label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Kullanıcı Adınızı Girin"
              required
            />
          </div>
          <div>
            <label>Şifre:</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Şifrenizi Girin"
              required
            />
          </div>
          <button type="submit">Kayıt Ol</button>
        </form>
        {message && <p className="message">{message}</p>}

        <div className="link">
          <p>Zaten bir hesabınız var mı?</p>
          <button onClick={() => navigate("/")}>Giriş Yap</button>
        </div>
      </div>
    </div>
    </>
  );
}

export default RegisterForm;
