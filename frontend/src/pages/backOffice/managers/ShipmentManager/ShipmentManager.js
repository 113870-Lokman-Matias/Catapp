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

import { ReactComponent as PriceInput } from "../../../../assets/svgs/priceinput.svg";
//#endregion

import Loader from "../../../../components/Loaders/LoaderCircle";

import "./ShipmentManager.css";

import {
  GetInfoEnvio,
  UpdateCostoEnvio,
} from "../../../../services/ShipmentService";

import { formatDate } from "../../../../utils/DateFormat";

function ShipmentManager() {
  //#region Constantes
  const [isLoading, setIsLoading] = useState(false);

  const [idEnvio, setIdEnvio] = useState("");

  const [habilitado, setHabilitado] = useState("");
  const [prevHabilitado, setPrevHabilitado] = useState("");

  const [precio, setPrecio] = useState("");
  const [prevPrecio, setPrevPrecio] = useState("");

  const [infoEnvio, setInfoEnvio] = useState([]);

  const token = localStorage.getItem("token"); // Obtener el token del localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };
  //#endregion

  //#region UseEffect
  useEffect(() => {
    (async () => {
      setIsLoading(true);

      try {
        const response = await GetInfoEnvio();
        setInfoEnvio(response);
        setIsLoading(false);
      } catch (error) {
        // Manejar errores aquí si es necesario
        setIsLoading(false);
      }
    })();
  }, []);
  //#endregion

  //#region Función para limpiar el valor del input del formulario
  function ClearCostoEnvioInputs() {
    setIdEnvio("");

    setHabilitado("");
    setPrecio("");
  }
  //#endregion

  //#region Función para obtener los valores almacenados del envío y cargarlo en su input correspondiente
  function RetrieveEnvioInputs(envio) {
    setIdEnvio(envio.idEnvio);
    setHabilitado(envio.habilitado);
    setPrecio(envio.precio);

    setPrevHabilitado(envio.habilitado);
    setPrevPrecio(envio.precio);
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
    if (habilitado === "") {
      Swal.fire({
        icon: "error",
        title: "Debe indicar si se encuentra habilitado",
        text: "Clickeé el botón en caso de que el envio a domicilio se encuentre habilitado",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      return false;
    } else if (precio === "") {
      Swal.fire({
        icon: "error",
        title: "El costo de envío no puede estar vacío",
        text: "Complete el campo, si no tiene costo ingrese 0",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#precio").focus();
        }, 500);
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Funcion para verificar si se actualizaron los valores de los inputs (habilitado y costo de envío)
  function IsUpdated() {
    if (prevHabilitado !== habilitado || prevPrecio !== precio) {
      return true;
    }
    return false;
  }
  //#endregion

  //#region Funcion para actualizar la habilitacion y costo de envío ya existentes
  async function UpdateCostoEnvioFunc(event) {
    event.preventDefault();

    if (IsUpdated() === false) {
      Swal.fire({
        icon: "error",
        title:
          "No puede actualizar el envío sin modificar su costo o su habilitación",
        text: "Modifique el costo o habilitacion para poder actualizarlo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#F27474",
      });
    } else if (IsValid() === true && IsUpdated() === true) {
      try {
        await UpdateCostoEnvio(
          {
            idEnvio: idEnvio,
            habilitado: habilitado,
            precio: precio,
          },
          headers
        );
        Swal.fire({
          icon: "success",
          title: "Envío actualizado exitosamente!",
          showConfirmButton: false,
          timer: 2000,
        });
        CloseModal();
        const response = await GetInfoEnvio();
        setInfoEnvio(response);

        // InitialState();
        ClearCostoEnvioInputs();
      } catch (err) {
        Swal.fire({
          icon: "error",
          title: "No puede actualizar el costo de envío con caracteres",
          text: "Ingrese un número, en caso de que el costo sea gratuito ingrese 0",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#F27474",
        });
      }
    }
  }
  //#endregion

  //#region Return
  return (
    <div>
      <Helmet>
        <title>Catapp | Administrar Envío</title>
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

              <h2 className="title title-general">Detalle de Envío</h2>
            </div>
          </div>

          {/* modal con el formulario para actualizar la habilitacion y el costo de envío */}
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
                    Actualizar Envío
                  </h1>
                </div>
                <div className="modal-body">
                  <div className="container mt-4">
                    <form>
                      <div className="form-group">
                        <input
                          type="text"
                          className="input"
                          id="idEnvio"
                          hidden
                          value={idEnvio}
                          onChange={(event) => {
                            setIdEnvio(event.target.value);
                          }}
                        />

                        <div className="form-group2 ocultar2">
                          <label className="label">Habilitado</label>
                          <input
                            type="checkbox"
                            className="form-check-input tick"
                            id="habilitado"
                            checked={habilitado}
                            onChange={(e) => {
                              setHabilitado(e.target.checked);
                            }}
                          />
                          <label
                            htmlFor="habilitado"
                            className="lbl-switch"
                          ></label>
                        </div>

                        <label className="label">Costo de envío:</label>
                        <div className="form-group-input">
                          <span className="input-group-text">
                            <PriceInput className="input-group-svg" />
                          </span>
                          <input
                            type="number"
                            step="1"
                            min={0}
                            className="input"
                            id="precio"
                            value={precio}
                            onChange={(event) => {
                              setPrecio(event.target.value);
                            }}
                          />
                        </div>
                      </div>
                      <div>
                        <div id="div-btn-update">
                          <button
                            className="btn btn-warning btn-edit-color"
                            id="btn-update"
                            onClick={UpdateCostoEnvioFunc}
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
                        ClearCostoEnvioInputs();
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
                            ClearCostoEnvioInputs();
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

          {/* tabla con el envío */}
          {isLoading ? (
            <div className="loading-generaltable-div">
              <Loader />
              <p className="bold-loading">Cargando envío...</p>
            </div>
          ) : (
            <table
              className="table table-dark table-bordered table-hover table-list"
              align="center"
            >
              <thead>
                <tr className="table-header">
                  <th className="table-title" scope="col">
                    Habilitado
                  </th>
                  <th className="table-title" scope="col">
                    Costo de envío
                  </th>
                  <th className="table-title" scope="col">
                    Modificado por
                  </th>
                  <th className="table-title" scope="col">
                    Fecha de modificación
                  </th>
                  <th className="table-title" scope="col">
                    Acción
                  </th>
                </tr>
              </thead>

              {infoEnvio ? (
                <tbody key={1 + infoEnvio.idEnvio}>
                  <tr>
                    {infoEnvio.habilitado ? (
                      <td className="table-name">Si</td>
                    ) : (
                      <td className="table-name">No</td>
                    )}

                    <td className="table-name table-costoenvio">
                      {infoEnvio && infoEnvio.precio !== undefined
                        ? `$${infoEnvio.precio.toLocaleString()}`
                        : ""}
                    </td>

                    <td className="table-name table-costoenvio">
                      {infoEnvio.ultimoModificador}
                    </td>

                    <td className="table-name table-costoenvio">
                      {formatDate(infoEnvio.fechaModificacion)}
                    </td>

                    <td className="table-name">
                      <button
                        type="button"
                        className="btn btn-warning btn-edit"
                        aria-label="Modificar"
                        data-bs-toggle="modal"
                        data-bs-target="#modal"
                        onClick={() => {
                          RetrieveEnvioInputs(infoEnvio);
                        }}
                      >
                        <Edit className="edit" />
                      </button>
                    </td>
                  </tr>
                </tbody>
              ) : (
                <tbody>
                  <tr className="tr-name1">
                    <td className="table-name table-name1" colSpan={4}>
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

export default ShipmentManager;
