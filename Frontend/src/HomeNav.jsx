import React from "react";
import { useNavigate } from "react-router-dom";
import "./Homenav.css";

function Navbar() {
  const navigate = useNavigate();

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <h1 className="navbar-title" onClick={() => navigate("/")}>MY CRYPTOS</h1>
        <div className="navbar-buttons">
          <button onClick={() => navigate("/home")} className="navbar-btn">Anasayfa</button>
          <button onClick={() => navigate("/favorite-cryptos")} className="navbar-btn">Favoriler</button>
          <button onClick={() => navigate("/settings")} className="navbar-btn">Ayarlar</button>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
