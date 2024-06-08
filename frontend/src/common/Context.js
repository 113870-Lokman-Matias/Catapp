import React, { createContext, useState, useEffect } from "react";
import { GetInfoConfiguracion } from "../services/SettingService";
import * as signalR from "@microsoft/signalr";

const Context = createContext();

const AppProvider = ({ children }) => {
  const [isLoading, setIsLoading] = useState(true);

  const [infoConfiguracion, setInfoConfiguracion] = useState(null);

  const [urlLogo, setUrlLogo] = useState(null);

  const [direccionAuto, setDireccionAuto] = useState(null);
  const [urlDireccionAuto, setUrlDireccionAuto] = useState(null);

  const [horariosAtencion, setHorariosAtencion] = useState(null);

  const [cbu, setCbu] = useState(null);
  const [alias, setAlias] = useState(null);

  const [montoMayorista, setMontoMayorista] = useState(null);

  const [telefonoEmpresa, setTelefonoEmpresa] = useState(null);
  const [whatsapp, setWhatsapp] = useState(null);

  const [facebook, setFacebook] = useState(null);
  const [urlFacebook, setUrlFacebook] = useState(null);

  const [instagram, setInstagram] = useState(null);
  const [urlInstagram, setUrlInstagram] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await GetInfoConfiguracion();
        setInfoConfiguracion(response);

        setUrlLogo(response.urlLogo);
        setDireccionAuto(response.direccion);
        setUrlDireccionAuto(response.urlDireccion);

        setHorariosAtencion(response.horarios);

        setCbu(response.cbu);
        setAlias(response.alias);

        setMontoMayorista(response.montoMayorista);

        setTelefonoEmpresa(response.telefono);
        setWhatsapp(response.whatsapp);

        setFacebook(response.facebook);
        setUrlFacebook(response.urlFacebook);

        setInstagram(response.instagram);
        setUrlInstagram(response.urlInstagram);

        setIsLoading(false);
      } catch (error) {
        console.log(error);
        setIsLoading(false);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7207/GeneralHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection
      .start()
      .then(() => {
        // console.log("Conexión establecida con el servidor SignalR");
      })
      .catch((err) => console.error(err.toString()));

    connection.on("MensajeUpdateConfiguracion", async () => {
      try {
        const response = await GetInfoConfiguracion();
        setInfoConfiguracion(response);

        setUrlLogo(response.urlLogo);
        setDireccionAuto(response.direccion);
        setUrlDireccionAuto(response.urlDireccion);

        setHorariosAtencion(response.horarios);

        setCbu(response.cbu);
        setAlias(response.alias);

        setMontoMayorista(response.montoMayorista);

        setTelefonoEmpresa(response.telefono);
        setWhatsapp(response.whatsapp);

        setFacebook(response.facebook);
        setUrlFacebook(response.urlFacebook);

        setInstagram(response.instagram);
        setUrlInstagram(response.urlInstagram);

        setIsLoading(false);
      } catch (error) {
        console.error(
          "Error al obtener la informacion de las configuraciones: " + error
        );
        setIsLoading(false);
      }
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <Context.Provider
      value={{
        isLoading,
        infoConfiguracion,
        urlLogo,
        direccionAuto,
        urlDireccionAuto,
        telefonoEmpresa,
        horariosAtencion,
        cbu,
        alias,
        montoMayorista,
        whatsapp,
        facebook,
        urlFacebook,
        instagram,
        urlInstagram,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export { Context, AppProvider };
