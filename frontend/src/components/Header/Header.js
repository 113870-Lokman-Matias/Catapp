import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

import "./Header.css";

import { GetInfoConfiguracion } from "../../services/SettingService";

function Header() {
  const [color, setColor] = useState(false);
  const changeColor = () => {
    if (window.scrollY >= 100) {
      setColor(true);
    } else {
      setColor(false);
    }
  };

  const [urlLogo, setUrlLogo] = useState(null);

  window.addEventListener("scroll", changeColor);

  useEffect(() => {
    // Funciónes asincronas
    (async () => {
      try {
        const response = await GetInfoConfiguracion();
        setUrlLogo(response.urlLogo);
      } catch (error) {
        console.log(error);
      }
    })();
  }, []);

  return (
    <nav className={color ? "header-nav header-nav-bg" : "header-nav"}>
      {urlLogo && urlLogo !== "" && (
        <Link
          to="/"
          className={
            color ? "header-a-logo  header-a-logo-bg " : "header-a-logo"
          }
        >
          <img
            className={color ? "header-logo  header-logo-bg " : "header-logo"}
            width={140}
            src={urlLogo}
            alt="logo"
          />
        </Link>
      )}
    </nav>
  );
}

export default Header;
