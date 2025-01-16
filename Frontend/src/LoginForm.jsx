import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Navbar from "./navbar"; // Navbar bileşeni
import api from "./api";
import "./LoginForm.css";

function LoginForm() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage("");

    try {
      const response = await api.post("/auth/login", { username, password });
      localStorage.setItem("token", response.data.token);
      navigate("/home");
    } catch (error) {
      setMessage(
        error.response?.data?.message || "Giriş başarısız. Lütfen tekrar deneyin."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Navbar />
      <div className="container">
      <div className="card">
        <h2>Giriş Yap</h2>
        <form onSubmit={handleLogin}>
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
          <button type="submit" disabled={loading}>
            {loading ? "Giriş Yapılıyor..." : "Giriş Yap"}
          </button>
        </form>
        {message && <p className="message">{message}</p>}

        <div className="link">
          <p>Henüz bir hesabınız yok mu?</p>
          <button onClick={() => navigate("/register")}>Kayıt Ol</button>
        </div>
      </div>
    </div>
    </>
  );
}

export default LoginForm;
