import React from "react";
import { useNavigate } from "react-router-dom";
import "./navbar.css"; // CSS dosyasını isteğe bağlı olarak oluşturabilirsiniz

function Navbar() {
  const navigate = useNavigate();

  return (
    <div className="navbar">
      <h1 className="navbar-title">MY CRYPTOS</h1>
    </div>
  );
}

export default Navbar;
