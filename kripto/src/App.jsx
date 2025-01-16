import React from "react";
import { BrowserRouter as Router, Routes, Route, useNavigate } from "react-router-dom";
import LoginForm from "./LoginForm";
import RegisterForm from "./RegisterForm";
import HomePage from "./HomePage"; // HomePage bileşeni oluşturulmalı
import FavoriteCryptos from "./FavoriteCryptos";
import "./App.css";
import Settings from "./Settings"; // Settings bileşenini import edin

function App() {
  return (
    <Router>
      <Main />
    </Router>
  );
}

function Main() {
  const navigate = useNavigate();

  return (
    <div>
      {/* Rotalar */}
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/register" element={<RegisterForm />} />
        <Route path="/home" element={<HomePage />} />
        <Route path="/favorite-cryptos" element={<FavoriteCryptos />} />
        <Route path="/settings" element={<Settings />} /> {/* Ayarlar rotası */}
      </Routes>
    </div>
  );
}

export default App;
