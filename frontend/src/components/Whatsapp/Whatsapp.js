import React, { useState, useEffect } from "react";

import "../Whatsapp/Whatsapp.css";

import { ReactComponent as Whatsapplogo } from "../../assets/svgs/whatsapp.svg";

import { GetInfoConfiguracion } from "../../services/SettingService";

function WhatsApp() {
  const [numero, setNumero] = useState(null);

  useEffect(() => {
    // Funciónes asincronas
    (async () => {
      try {
        const response = await GetInfoConfiguracion();
        setNumero(response.whatsapp);
      } catch (error) {
        console.log(error);
      }
    })();
  }, []);

  // Si el número no está disponible, retorna null o un componente de carga
  if (!numero || numero === "0") {
    return null; // Puedes retornar un componente de carga si prefieres
  }

  return (
    <a
      href={`https://wa.me/${numero}`}
      target="_blank"
      rel="noreferrer"
      className="js-btn-fixed-bottom btn-whatsapp"
      aria-label="Comunicate por WhatsApp"
    >
      {" "}
      <Whatsapplogo />
    </a>
  );
}

export default WhatsApp;
