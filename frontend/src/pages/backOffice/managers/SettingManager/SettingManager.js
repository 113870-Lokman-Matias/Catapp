import Swal from "sweetalert2";
import $ from "jquery";
import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Helmet } from "react-helmet";

//#region SVG'S Imports
import { ReactComponent as Edit } from "../../../../assets/svgs/edit.svg";
import { ReactComponent as Update } from "../../../../assets/svgs/update.svg";
import { ReactComponent as Close } from "../../../../assets/svgs/closebtn.svg";
import { ReactComponent as Back } from "../../../../assets/svgs/back.svg";

import { ReactComponent as DireccionInput } from "../../../../assets/svgs/location.svg";
import { ReactComponent as UrlDireccionInput } from "../../../../assets/svgs/locationUrl.svg";
import { ReactComponent as HorariosInput } from "../../../../assets/svgs/horarios.svg";
import { ReactComponent as CbuInput } from "../../../../assets/svgs/cbu.svg";
import { ReactComponent as AliasInput } from "../../../../assets/svgs/alias.svg";
import { ReactComponent as WhatsAppInput } from "../../../../assets/svgs/whatsappBlack.svg";
import { ReactComponent as TelefonoInput } from "../../../../assets/svgs/telefono.svg";
import { ReactComponent as FacebookInput } from "../../../../assets/svgs/facebook.svg";
import { ReactComponent as InstagramInput } from "../../../../assets/svgs/instagram.svg";
import { ReactComponent as UrlFacebookInput } from "../../../../assets/svgs/urlFacebook.svg";
import { ReactComponent as UrlInstagramInput } from "../../../../assets/svgs/urlInstagram.svg";
import { ReactComponent as PedidoInput } from "../../../../assets/svgs/cantidadPedidos.svg";
import { ReactComponent as LogoInput } from "../../../../assets/svgs/imageinput.svg";

import { ReactComponent as Transferencia } from "../../../../assets/svgs/transferencia.svg";
import { ReactComponent as Redes } from "../../../../assets/svgs/redes.svg";
//#endregion

import Loader from "../../../../components/Loaders/LoaderCircle";

import "./SettingManager.css";

import {
  GetInfoConfiguracion,
  UpdateConfiguracion,
} from "../../../../services/SettingService";

function SettingManager() {
  //#region Constantes
  const [isLoading, setIsLoading] = useState(false);

  const [idConfiguracion, setIdConfiguracion] = useState("");

  const [direccion, setDireccion] = useState("");
  const [prevDireccion, setPrevDireccion] = useState("");

  const [urlDireccion, setUrlDireccion] = useState("");
  const [prevUrlDireccion, setPrevUrlDireccion] = useState("");

  const [horarios, setHorarios] = useState("");
  const [prevHorarios, setPrevHorarios] = useState("");

  const [cbu, setCbu] = useState("");
  const [prevCbu, setPrevCbu] = useState("");

  const [alias, setAlias] = useState("");
  const [prevAlias, setPrevAlias] = useState("");

  const [whatsapp, setWhatsapp] = useState("");
  const [prevWhatsapp, setPrevWhatsapp] = useState("");

  const [telefono, setTelefono] = useState("");
  const [prevTelefono, setPrevTelefono] = useState("");

  const [facebook, setFacebook] = useState("");
  const [prevFacebook, setPrevFacebook] = useState("");

  const [urlFacebook, setUrlFacebook] = useState("");
  const [prevUrlFacebook, setPrevUrlFacebook] = useState("");

  const [instagram, setInstagram] = useState("");
  const [prevInstagram, setPrevInstagram] = useState("");

  const [urlInstagram, setUrlInstagram] = useState("");
  const [prevUrlInstagram, setPrevUrlInstagram] = useState("");

  const [cantidadMayorista, setCantidadMayorista] = useState("");
  const [prevCantidadMayorista, setPrevCantidadMayorista] = useState("");

  const [urlLogo, setUrlLogo] = useState("");
  const [prevUrlLogo, setPrevUrlLogo] = useState("");

  const [infoConfiguracion, setInfoConfiguracion] = useState([]);

  const token = localStorage.getItem("token"); // Obtener el token del localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  //#region constantes para mostrar secciones
  const [mostrarDireccion, setMostrarDireccion] = useState(false);
  const [mostrarHorarios, setMostrarHorarios] = useState(false);
  const [mostrarTransferencia, setMostrarTransferencia] = useState(false);
  const [mostrarTelefonos, setMostrarTelefonos] = useState(false);
  const [mostrarCantidadMayorista, setMostrarCantidadMayorista] =
    useState(false);
  const [mostrarRedes, setMostrarRedes] = useState(false);
  const [mostrarLogo, setMostrarLogo] = useState(false);

  const mostrarAccion =
    mostrarDireccion ||
    mostrarHorarios ||
    mostrarTransferencia ||
    mostrarTelefonos ||
    mostrarCantidadMayorista ||
    mostrarRedes ||
    mostrarLogo;

  const showNombreInputDireccion = () => {
    return (
      mostrarDireccion &&
      !mostrarHorarios &&
      !mostrarTransferencia &&
      !mostrarTelefonos &&
      !mostrarCantidadMayorista &&
      !mostrarRedes &&
      !mostrarLogo
    );
  };

  const showNombreInputHorarios = () => {
    return (
      mostrarHorarios &&
      (!mostrarDireccion || mostrarDireccion) &&
      !mostrarTransferencia &&
      !mostrarTelefonos &&
      !mostrarCantidadMayorista &&
      !mostrarRedes &&
      !mostrarLogo
    );
  };

  const showNombreInputTransferencia = () => {
    return (
      mostrarTransferencia &&
      (!mostrarDireccion || mostrarDireccion) &&
      (!mostrarHorarios || mostrarHorarios) &&
      !mostrarTelefonos &&
      !mostrarCantidadMayorista &&
      !mostrarRedes &&
      !mostrarLogo
    );
  };

  const showNombreInputTelefonos = () => {
    return (
      mostrarTelefonos &&
      (!mostrarDireccion || mostrarDireccion) &&
      (!mostrarHorarios || mostrarHorarios) &&
      (!mostrarTransferencia || mostrarTransferencia) &&
      !mostrarCantidadMayorista &&
      !mostrarRedes &&
      !mostrarLogo
    );
  };

  const showNombreInputRedes = () => {
    return (
      mostrarRedes &&
      (!mostrarDireccion || mostrarDireccion) &&
      (!mostrarHorarios || mostrarHorarios) &&
      (!mostrarTransferencia || mostrarTransferencia) &&
      (!mostrarTelefonos || mostrarTelefonos) &&
      !mostrarCantidadMayorista &&
      !mostrarLogo
    );
  };

  const showNombreInputCantidadMayorista = () => {
    return (
      mostrarCantidadMayorista &&
      (!mostrarDireccion || mostrarDireccion) &&
      (!mostrarHorarios || mostrarHorarios) &&
      (!mostrarTransferencia || mostrarTransferencia) &&
      (!mostrarTelefonos || mostrarTelefonos) &&
      (!mostrarRedes || mostrarRedes) &&
      !mostrarLogo
    );
  };
  //#endregion

  //#region UseEffect
  useEffect(() => {
    (async () => {
      setIsLoading(true);

      try {
        const response = await GetInfoConfiguracion();
        setInfoConfiguracion(response);
        setIsLoading(false);
      } catch (error) {
        // Manejar errores aquí si es necesario
        setIsLoading(false);
      }
    })();
  }, []);
  //#endregion

  //#region Función para limpiar el valor del input del formulario
  function ClearConfiguracionesInputs() {
    setIdConfiguracion("");

    setDireccion("");
    setUrlDireccion("");
    setHorarios("");
    setCbu("");
    setAlias("");
    setWhatsapp("");
    setTelefono("");
    setFacebook("");
    setUrlFacebook("");
    setInstagram("");
    setUrlInstagram("");
    setCantidadMayorista("");
    setUrlLogo("");
  }
  //#endregion

  //#region Función para obtener los valores almacenados de las configuraciones y cargarlos en su inputs correspondientes
  function RetrieveConfiguracionInputs(configuracion) {
    setIdConfiguracion(configuracion.idConfiguracion);
    setDireccion(configuracion.direccion);
    setUrlDireccion(configuracion.urlDireccion);
    setHorarios(configuracion.horarios);
    setCbu(configuracion.cbu);
    setAlias(configuracion.alias);
    setWhatsapp(configuracion.whatsapp);
    setTelefono(configuracion.telefono);
    setFacebook(configuracion.facebook);
    setUrlFacebook(configuracion.urlFacebook);
    setInstagram(configuracion.instagram);
    setUrlInstagram(configuracion.urlInstagram);
    setCantidadMayorista(configuracion.cantidadMayorista);
    setUrlLogo(configuracion.urlLogo);

    setPrevDireccion(configuracion.direccion);
    setPrevUrlDireccion(configuracion.urlDireccion);
    setPrevHorarios(configuracion.horarios);
    setPrevCbu(configuracion.cbu);
    setPrevAlias(configuracion.alias);
    setPrevWhatsapp(configuracion.whatsapp);
    setPrevTelefono(configuracion.telefono);
    setPrevFacebook(configuracion.facebook);
    setPrevUrlFacebook(configuracion.urlFacebook);
    setPrevInstagram(configuracion.instagram);
    setPrevUrlInstagram(configuracion.urlInstagram);
    setPrevCantidadMayorista(configuracion.cantidadMayorista);
    setPrevUrlLogo(configuracion.urlLogo);
  }
  //#endregion

  //#region Funcion para cerrar el modal manualmente mediante el codigo
  function CloseModal() {
    $(document).ready(function () {
      $("#btn-close-modal").click();
    });
  }
  //#endregion

  //#region Funcion para verificar si el valore ingresado a traves del input es correcto
  function IsValid() {
    // if (precio === "") {
    //   Swal.fire({
    //     icon: "error",
    //     title: "El costo de envío no puede estar vacío",
    //     text: "Complete el campo, si no tiene costo ingrese 0",
    //     confirmButtonText: "Aceptar",
    //     confirmButtonColor: "#f27474",
    //   }).then(function () {
    //     setTimeout(function () {
    //       $("#precio").focus();
    //     }, 500);
    //   });
    //   return false;
    // }
    return true;
  }
  //#endregion

  //#region Funcion para verificar si se actualizaron los valores de los inputs
  function IsUpdated() {
    if (
      prevDireccion !== direccion ||
      prevUrlDireccion !== urlDireccion ||
      prevHorarios !== horarios ||
      prevCbu !== cbu ||
      prevAlias !== alias ||
      prevWhatsapp !== whatsapp ||
      prevTelefono !== telefono ||
      prevFacebook !== facebook ||
      prevUrlFacebook !== urlFacebook ||
      prevInstagram !== instagram ||
      prevUrlInstagram !== urlInstagram ||
      prevCantidadMayorista !== cantidadMayorista ||
      prevUrlLogo !== urlLogo
    ) {
      return true;
    }
    return false;
  }
  //#endregion

  //#region Funcion para actualizar las configuraciones ya existentes
  async function UpdateConfiguracionFunc(event) {
    event.preventDefault();

    if (IsUpdated() === false) {
      Swal.fire({
        icon: "error",
        title:
          "No puede actualizar las configuraciones sin modificar ningun campo",
        text: "Modifique algun campo para poder actualizarlas",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#F27474",
      });
    } else if (IsValid() === true && IsUpdated() === true) {
      try {
        await UpdateConfiguracion(
          {
            idConfiguracion: idConfiguracion,
            direccion: direccion,
            urlDireccion: urlDireccion,
            horarios: horarios,
            cbu: cbu,
            alias: alias,
            whatsapp: whatsapp,
            telefono: telefono,
            facebook: facebook,
            urlFacebook: urlFacebook,
            instagram: instagram,
            urlInstagram: urlInstagram,
            cantidadMayorista: cantidadMayorista,
            urlLogo: urlLogo,
          },
          headers
        );
        Swal.fire({
          icon: "success",
          title: "Configuración actualizada exitosamente!",
          showConfirmButton: false,
          timer: 2000,
        });
        CloseModal();
        const response = await GetInfoConfiguracion();
        setInfoConfiguracion(response);

        // InitialState();
        ClearConfiguracionesInputs();
      } catch (err) {
        console.log(err);
      }
    }
  }
  //#endregion

  //#region Return
  return (
    <div>
      <Helmet>
        <title>Catapp | Administrar Configuraciones</title>
      </Helmet>

      <section className="general-container">
        <div className="general-content">
          <div className="general-title">
            <div className="title-header">
              <Link to="/panel" className="btn btn-info btn-back">
                <div className="btn-back-content">
                  <Back className="back" />
                  <p className="p-back">Regresar</p>
                </div>
              </Link>

              <h2 className="title title-general">
                Detalle de Configuraciones
              </h2>
            </div>

            {!isLoading && (
              <div className="header-detalles-report">
                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders direccion ${
                    mostrarDireccion ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarDireccion
                      ? "Ocultar detalles de dirección"
                      : "Ver detalles de dirección"
                  }
                  onClick={() => {
                    setMostrarDireccion(!mostrarDireccion);
                  }}
                >
                  <DireccionInput
                    className={`show-orders ${
                      mostrarDireccion ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>

                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders horarios ${
                    mostrarHorarios ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarHorarios
                      ? "Ocultar detalle de horarios"
                      : "Ver detalle de horarios"
                  }
                  onClick={() => {
                    setMostrarHorarios(!mostrarHorarios);
                  }}
                >
                  <HorariosInput
                    className={`show-orders ${
                      mostrarHorarios ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>

                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders transferencia ${
                    mostrarTransferencia ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarTransferencia
                      ? "Ocultar detalles de transferencia"
                      : "Ver detalles de transferencia"
                  }
                  onClick={() => {
                    setMostrarTransferencia(!mostrarTransferencia);
                  }}
                >
                  <Transferencia
                    className={`show-orders ${
                      mostrarTransferencia ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>

                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders whatsapp ${
                    mostrarTelefonos ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarTelefonos
                      ? "Ocultar detalles de teléfono"
                      : "Ver detalles de teléfono"
                  }
                  onClick={() => {
                    setMostrarTelefonos(!mostrarTelefonos);
                  }}
                >
                  <TelefonoInput
                    className={`show-orders ${
                      mostrarTelefonos ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>

                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders mayorista ${
                    mostrarCantidadMayorista ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarCantidadMayorista
                      ? "Ocultar detalle de cantidad mayorista"
                      : "Ver detalle de cantidad mayorista"
                  }
                  onClick={() => {
                    setMostrarCantidadMayorista(!mostrarCantidadMayorista);
                  }}
                >
                  <PedidoInput
                    className={`show-orders ${
                      mostrarCantidadMayorista ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>

                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders facebook ${
                    mostrarRedes ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarRedes
                      ? "Ocultar detalles de redes sociales"
                      : "Ver detalles de redes sociales"
                  }
                  onClick={() => {
                    setMostrarRedes(!mostrarRedes);
                  }}
                >
                  <Redes
                    className={`show-orders ${
                      mostrarRedes ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>

                <button
                  type="button"
                  className={`btn btn-dark btn-show-orders logo ${
                    mostrarLogo ? "btn-seleccionado" : ""
                  }`}
                  title={
                    mostrarLogo
                      ? "Ocultar detalle de logo"
                      : "Ver detalle de logo"
                  }
                  onClick={() => {
                    setMostrarLogo(!mostrarLogo);
                  }}
                >
                  <LogoInput
                    className={`show-orders ${
                      mostrarLogo ? "show-orders-seleccionado" : ""
                    }`}
                  />
                </button>
              </div>
            )}
          </div>

          {/* modal con el formulario para actualizar las configuraciones */}
          <div
            className="modal fade"
            id="modal"
            data-bs-backdrop="static"
            data-bs-keyboard="false"
            tabIndex="-1"
            aria-labelledby="staticBackdropLabel"
            aria-hidden="true"
          >
            <div className="modal-dialog">
              <div className="modal-content">
                <div className="modal-header">
                  <h1 className="modal-title" id="exampleModalLabel">
                    Actualizar Configuraciones
                  </h1>
                </div>
                <div className="modal-body">
                  <div className="container mt-4">
                    <form>
                      <div className="form-group">
                        <input
                          type="text"
                          className="input"
                          id="idConfiguracion"
                          hidden
                          value={idConfiguracion}
                          onChange={(event) => {
                            setIdConfiguracion(event.target.value);
                          }}
                        />

                        {mostrarDireccion && (
                          <>
                            <label className="label">Dirección:</label>
                            <div className="form-group-input nombre-input">
                              <span className="input-group-text">
                                <DireccionInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="direccion"
                                value={direccion}
                                onChange={(event) => {
                                  setDireccion(event.target.value);
                                }}
                              />
                            </div>

                            <label className="label">
                              URL de la Dirección:
                            </label>
                            <div
                              className={`form-group-input ${
                                !showNombreInputDireccion()
                                  ? "nombre-input"
                                  : ""
                              }`}
                            >
                              <span className="input-group-text">
                                <UrlDireccionInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="urlDireccion"
                                value={urlDireccion}
                                onChange={(event) => {
                                  setUrlDireccion(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {mostrarHorarios && (
                          <>
                            <label className="label">Horarios:</label>
                            <div
                              className={`form-group-input ${
                                !showNombreInputHorarios() ? "nombre-input" : ""
                              }`}
                            >
                              <span className="input-group-text">
                                <HorariosInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="horarios"
                                value={horarios}
                                onChange={(event) => {
                                  setHorarios(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {mostrarTransferencia && (
                          <>
                            <label className="label">CBU:</label>
                            <div className="form-group-input nombre-input">
                              <span className="input-group-text">
                                <CbuInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="cbu"
                                value={cbu}
                                onChange={(event) => {
                                  setCbu(event.target.value);
                                }}
                              />
                            </div>

                            <label className="label">Alias:</label>
                            <div
                              className={`form-group-input ${
                                !showNombreInputTransferencia()
                                  ? "nombre-input"
                                  : ""
                              }`}
                            >
                              <span className="input-group-text">
                                <AliasInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="alias"
                                value={alias}
                                onChange={(event) => {
                                  setAlias(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {mostrarTelefonos && (
                          <>
                            <label className="label">Número de WhatsApp:</label>
                            <div className="form-group-input nombre-input">
                              <span className="input-group-text">
                                <WhatsAppInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="whatsapp"
                                value={whatsapp}
                                onChange={(event) => {
                                  setWhatsapp(event.target.value);
                                }}
                              />
                            </div>

                            <label className="label">Número de Teléfono:</label>
                            <div
                              className={`form-group-input ${
                                !showNombreInputTelefonos()
                                  ? "nombre-input"
                                  : ""
                              }`}
                            >
                              <span className="input-group-text">
                                <TelefonoInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="telefono"
                                value={telefono}
                                onChange={(event) => {
                                  setTelefono(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {mostrarRedes && (
                          <>
                            <label className="label">Nombre de Facebook:</label>
                            <div className="form-group-input nombre-input">
                              <span className="input-group-text">
                                <FacebookInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="facebook"
                                value={facebook}
                                onChange={(event) => {
                                  setFacebook(event.target.value);
                                }}
                              />
                            </div>

                            <label className="label">URL de Facebook:</label>
                            <div className="form-group-input nombre-input">
                              <span className="input-group-text">
                                <UrlFacebookInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="urlFacebook"
                                value={urlFacebook}
                                onChange={(event) => {
                                  setUrlFacebook(event.target.value);
                                }}
                              />
                            </div>

                            <label className="label">
                              Nombre de Instagram:
                            </label>
                            <div className="form-group-input nombre-input">
                              <span className="input-group-text">
                                <InstagramInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="instagram"
                                value={instagram}
                                onChange={(event) => {
                                  setInstagram(event.target.value);
                                }}
                              />
                            </div>

                            <label className="label">URL de Instagram:</label>
                            <div
                              className={`form-group-input ${
                                !showNombreInputRedes() ? "nombre-input" : ""
                              }`}
                            >
                              <span className="input-group-text">
                                <UrlInstagramInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="urlInstagram"
                                value={urlInstagram}
                                onChange={(event) => {
                                  setUrlInstagram(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {mostrarCantidadMayorista && (
                          <>
                            <label className="label">Cantidad Mayorista:</label>
                            <div
                              className={`form-group-input ${
                                !showNombreInputCantidadMayorista()
                                  ? "nombre-input"
                                  : ""
                              }`}
                            >
                              <span className="input-group-text">
                                <PedidoInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="cantidadMayorista"
                                value={cantidadMayorista}
                                onChange={(event) => {
                                  setCantidadMayorista(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {mostrarLogo && (
                          <>
                            <label className="label">URL del Logo:</label>
                            <div className="form-group-input">
                              <span className="input-group-text">
                                <LogoInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="urlLogo"
                                value={urlLogo}
                                onChange={(event) => {
                                  setUrlLogo(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}
                      </div>
                      <div>
                        <div id="div-btn-update">
                          <button
                            className="btn btn-warning btn-edit-color"
                            id="btn-update"
                            onClick={UpdateConfiguracionFunc}
                          >
                            <div className="btn-save-update-close">
                              <Update className="update-btn" />
                              <p className="p-save-update-close">Actualizar</p>
                            </div>
                          </button>
                        </div>
                      </div>
                    </form>
                  </div>
                </div>
                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => {
                      if (IsUpdated() === false) {
                        ClearConfiguracionesInputs();
                        CloseModal();
                      } else {
                        Swal.fire({
                          icon: "warning",
                          title:
                            "¿Está seguro de que desea cerrar el formulario?",
                          text: "Se perderán todos los datos modificados",
                          confirmButtonText: "Aceptar",
                          showCancelButton: true,
                          cancelButtonText: "Cancelar",
                          confirmButtonColor: "#f8bb86",
                          cancelButtonColor: "#6c757d",
                        }).then((result) => {
                          if (result.isConfirmed) {
                            ClearConfiguracionesInputs();
                            CloseModal();
                          }
                        });
                      }
                    }}
                  >
                    <div className="btn-save-update-close">
                      <Close className="close-btn" />
                      <p className="p-save-update-close">Cerrar</p>
                    </div>
                  </button>

                  <button
                    type="button"
                    className="btn-close-modal"
                    id="btn-close-modal"
                    data-bs-dismiss="modal"
                  ></button>
                </div>
              </div>
            </div>
          </div>

          <br />

          {/* tabla con las configuraciones */}
          {isLoading ? (
            <div className="loading-generaltable-div">
              <Loader />
              <p className="bold-loading">Cargando configuraciones...</p>
            </div>
          ) : (
            <table
              className="table table-dark table-bordered table-list table-list-orders table-orders"
              align="center"
            >
              <thead>
                <tr className="table-header table-header-orders">
                  {mostrarDireccion && (
                    <th
                      className="table-title table-title-orders direccion"
                      scope="col"
                    >
                      <DireccionInput className="input-group-svg-white" />
                      Dirección
                    </th>
                  )}
                  {mostrarDireccion && (
                    <th
                      className="table-title table-title-orders direccion"
                      scope="col"
                    >
                      <UrlDireccionInput className="input-group-svg-white" />
                      URL Dirección
                    </th>
                  )}
                  {mostrarHorarios && (
                    <th
                      className="table-title table-title-orders horarios"
                      scope="col"
                    >
                      <HorariosInput className="input-group-svg-white" />
                      Horarios
                    </th>
                  )}
                  {mostrarTransferencia && (
                    <th
                      className="table-title table-title-orders transferencia"
                      scope="col"
                    >
                      <CbuInput className="input-group-svg-white" />
                      CBU
                    </th>
                  )}
                  {mostrarTransferencia && (
                    <th
                      className="table-title table-title-orders transferencia"
                      scope="col"
                    >
                      <AliasInput className="input-group-svg-white" />
                      ALIAS
                    </th>
                  )}
                  {mostrarTelefonos && (
                    <th
                      className="table-title table-title-orders whatsapp"
                      scope="col"
                    >
                      <TelefonoInput className="input-group-svg-white" />
                      Teléfono
                    </th>
                  )}
                  {mostrarTelefonos && (
                    <th
                      className="table-title table-title-orders whatsapp"
                      scope="col"
                    >
                      <WhatsAppInput className="input-group-svg-white" />
                      WhatsApp
                    </th>
                  )}
                  {mostrarCantidadMayorista && (
                    <th
                      className="table-title table-title-orders mayorista"
                      scope="col"
                    >
                      <PedidoInput className="input-group-svg-white" />
                      Cantidad minima mayorista
                    </th>
                  )}
                  {mostrarRedes && (
                    <th
                      className="table-title table-title-orders facebook"
                      scope="col"
                    >
                      <FacebookInput className="input-group-svg-white" />
                      Facebook
                    </th>
                  )}
                  {mostrarRedes && (
                    <th
                      className="table-title table-title-orders facebook"
                      scope="col"
                    >
                      <UrlFacebookInput className="input-group-svg-white" />
                      URL Facebook
                    </th>
                  )}
                  {mostrarRedes && (
                    <th
                      className="table-title table-title-orders instagram"
                      scope="col"
                    >
                      <InstagramInput className="input-group-svg-white" />
                      Instagram
                    </th>
                  )}
                  {mostrarRedes && (
                    <th
                      className="table-title table-title-orders instagram"
                      scope="col"
                    >
                      <UrlInstagramInput className="input-group-svg-white" />
                      URL Instagram
                    </th>
                  )}
                  {mostrarLogo && (
                    <th
                      className="table-title table-title-orders logo"
                      scope="col"
                    >
                      <LogoInput className="input-group-svg-white" />
                      URL Logo
                    </th>
                  )}
                  {mostrarAccion && (
                    <th className="table-title table-title-orders" scope="col">
                      Acción
                    </th>
                  )}
                </tr>
              </thead>

              {infoConfiguracion ? (
                <tbody key={1 + infoConfiguracion.idConfiguracion}>
                  <tr>
                    {mostrarDireccion && (
                      <td className="table-name table-name-orders direccion">
                        {infoConfiguracion.direccion}
                      </td>
                    )}
                    {mostrarDireccion && (
                      <td className="table-name table-name-orders direccion">
                        <p className="long-text">
                          {infoConfiguracion.urlDireccion}
                        </p>
                      </td>
                    )}
                    {mostrarHorarios && (
                      <td className="table-name table-name-orders horarios">
                        <p className="long-text">
                          {infoConfiguracion.horarios}
                        </p>
                      </td>
                    )}
                    {mostrarTransferencia && (
                      <td className="table-name table-name-orders transferencia">
                        {infoConfiguracion.cbu}
                      </td>
                    )}
                    {mostrarTransferencia && (
                      <td className="table-name table-name-orders transferencia">
                        {infoConfiguracion.alias}
                      </td>
                    )}
                    {mostrarTelefonos && (
                      <td className="table-name table-name-orders whatsapp">
                        {infoConfiguracion.telefono}
                      </td>
                    )}
                    {mostrarTelefonos && (
                      <td className="table-name table-name-orders whatsapp">
                        {infoConfiguracion.whatsapp}
                      </td>
                    )}
                    {mostrarCantidadMayorista && (
                      <td className="table-name table-name-orders mayorista">
                        {infoConfiguracion.cantidadMayorista}
                      </td>
                    )}
                    {mostrarRedes && (
                      <td className="table-name table-name-orders facebook">
                        {infoConfiguracion.facebook}
                      </td>
                    )}
                    {mostrarRedes && (
                      <td className="table-name table-name-orders facebook">
                        <p className="long-text">
                          {infoConfiguracion.urlFacebook}
                        </p>
                      </td>
                    )}
                    {mostrarRedes && (
                      <td className="table-name table-name-orders instagram">
                        {infoConfiguracion.instagram}
                      </td>
                    )}
                    {mostrarRedes && (
                      <td className="table-name table-name-orders instagram">
                        <p className="long-text">
                          {infoConfiguracion.urlInstagram}
                        </p>
                      </td>
                    )}
                    {mostrarLogo && (
                      <td className="table-name table-name-orders logo">
                        {infoConfiguracion.urlLogo}
                      </td>
                    )}

                    {mostrarAccion && (
                      <td className="table-name table-name-orders">
                        <button
                          type="button"
                          className="btn btn-warning btn-edit"
                          aria-label="Modificar"
                          data-bs-toggle="modal"
                          data-bs-target="#modal"
                          onClick={() => {
                            RetrieveConfiguracionInputs(infoConfiguracion);
                          }}
                        >
                          <Edit className="edit" />
                        </button>
                      </td>
                    )}
                  </tr>
                </tbody>
              ) : (
                <tbody>
                  <tr className="tr-name1">
                    <td
                      className="table-name table-name-orders table-name1"
                      colSpan={4}
                    >
                      Sin registros
                    </td>
                  </tr>
                </tbody>
              )}
            </table>
          )}
        </div>
      </section>
    </div>
  );
  //#endregion
}

export default SettingManager;
