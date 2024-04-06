import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

import "./Header.css";

import logo from "../../assets/images/logo.png";

function Header() {
  const [color, setColor] = useState(false);
  const changeColor = () => {
    if (window.scrollY >= 100) {
      setColor(true);
    } else {
      setColor(false);
    }
  };

  window.addEventListener("scroll", changeColor);

  useEffect(() => {
    (async () => [])();
  }, []);

  return (
    <nav className={color ? "header-nav header-nav-bg" : "header-nav"}>
      <Link
        to="/"
        className={color ? "header-a-logo  header-a-logo-bg " : "header-a-logo"}
      >
        <img
          className={color ? "header-logo  header-logo-bg " : "header-logo"}
          width={140}
          src={logo}
          alt="logo"
        />
      </Link>
    </nav>
  );
}

export default Header;
