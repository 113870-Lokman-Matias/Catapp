import Swal from "sweetalert2";
import { ReactComponent as Lupa } from "../../../../assets/svgs/lupa.svg";
import { useDownloadExcel } from "react-export-table-to-excel";
import $ from "jquery";
import { useNavigate } from "react-router-dom";
import React, { useEffect, useState, useRef } from "react";
import { Link } from "react-router-dom";
import { Helmet } from "react-helmet";

//#region SVG'S Imports
import { ReactComponent as Edit } from "../../../../assets/svgs/edit.svg";
import { ReactComponent as Delete } from "../../../../assets/svgs/delete.svg";
import { ReactComponent as Update } from "../../../../assets/svgs/update.svg";
import { ReactComponent as Close } from "../../../../assets/svgs/closebtn.svg";
import { ReactComponent as Back } from "../../../../assets/svgs/back.svg";
import { ReactComponent as Verificar } from "../../../../assets/svgs/verificar.svg";
import { ReactComponent as Pendiente } from "../../../../assets/svgs/pendiente.svg";
import { ReactComponent as Filter } from "../../../../assets/svgs/filter.svg";

import { ReactComponent as Excel } from "../../../../assets/svgs/excel.svg";

import { ReactComponent as CostoEnvioInput } from "../../../../assets/svgs/shipment.svg";
import { ReactComponent as SellerInput } from "../../../../assets/svgs/seller.svg";
import { ReactComponent as EntregaInput } from "../../../../assets/svgs/entregainput.svg";
import { ReactComponent as PaymentInput } from "../../../../assets/svgs/paymentInput.svg";
//#endregion

import {
  GetOrders,
  UpdateOrders,
  DeleteOrders,
  UpdateOrdersVerified,
} from "../../../../services/OrderService";
import { GetUsersSellers } from "../../../../services/UserService";
import { GetCostoEnvioUnicamente } from "../../../../services/ShipmentService";
import { formatDate } from "../../../../utils/DateFormat";

function OrderManager() {
  //#region Constantes
  const [idPedido, setIdPedido] = useState("");

  const [tipo, setTipo] = useState("");

  const [entrega, setEntrega] = useState("");
  const [prevEntrega, setPrevEntrega] = useState("");

  const [vendedor, setVendedor] = useState("");
  const [prevVendedor, setPrevVendedor] = useState("");

  const [costoEnvio, setCostoEnvio] = useState("");
  const [prevCostoEnvio, setPrevCostoEnvio] = useState("");

  const [abono, setAbono] = useState("");
  const [prevAbono, setPrevAbono] = useState("");

  const [listaNombresVendedores, setListaNombresVendedores] = useState(true);
  const [costoEnvioDomicilio, setCostoEnvioDomicilio] = useState(0);

  const [orders, setOrders] = useState([]);

  const [originalOrdersList, setOriginalOrdersList] = useState(orders);

  const [title, setTitle] = useState(["Detalles de Pedidos"]);

  const [filterName, setFilterName] = useState("");

  const [filterType, setFilterType] = useState("");

  const [query, setQuery] = useState("");

  const [pending, setPending] = useState(false);

  const tableRef = useRef(null);

  const { onDownload } = useDownloadExcel({
    currentTableRef: tableRef.current,
    filename: `${title}`,
    sheet: `${title}`,
  });

  const token = localStorage.getItem("token"); // Obtener el token del localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };
  const rolUsuario = JSON.parse(atob(token.split(".")[1])).role;

  const navigate = useNavigate();

  const [cantidadPedidosPendientes, setCantidadPedidosPendientes] =
    useState("");

  //#region Constantes de la paginacion
  const [currentPage, setCurrentPage] = useState(1);
  const [ordersPerPage, setOrdersPerPage] = useState(20);
  const lastIndex = currentPage * ordersPerPage;
  const firstIndex = lastIndex - ordersPerPage;
  const ordersTable = orders.slice(firstIndex, lastIndex);
  const npage = Math.ceil(orders.length / ordersPerPage);
  const numbers = [...Array(npage + 1).keys()].slice(1);

  const [maxPageNumbersToShow, setMaxPageNumbersToShow] = useState(9);
  const minPageNumbersToShow = 0;
  //#endregion
  //#endregion

  //#region UseEffect
  useEffect(() => {
    (async () =>
      await [
        GetOrders(setOrders),
        GetOrders(setOriginalOrdersList),
        GetUsersSellers(setListaNombresVendedores),
        GetCostoEnvioUnicamente(setCostoEnvioDomicilio),
      ])();

    if (window.matchMedia("(max-width: 500px)").matches) {
      setOrdersPerPage(1);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 600px)").matches) {
      setOrdersPerPage(1);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 700px)").matches) {
      setOrdersPerPage(1);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 800px)").matches) {
      setOrdersPerPage(2);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 900px)").matches) {
      setOrdersPerPage(2);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 1000px)").matches) {
      setOrdersPerPage(2);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 1140px)").matches) {
      setOrdersPerPage(7);
      setMaxPageNumbersToShow(1);
    } else {
      setOrdersPerPage(10);
      setMaxPageNumbersToShow(9);
    }

    const token = localStorage.getItem("token");

    if (token) {
      const expiracionEnSegundos = JSON.parse(atob(token.split(".")[1])).exp;
      const expiracionEnMilisegundos = expiracionEnSegundos * 1000;
      const fechaExpiracion = new Date(expiracionEnMilisegundos);
      const fechaActual = new Date();

      if (fechaExpiracion <= fechaActual) {
        localStorage.removeItem("token");
        navigate("/login");
      }

      const temporizador = setInterval(() => {
        const token = localStorage.getItem("token");
        if (!token) {
          clearInterval(temporizador);
          return;
        }

        const expiracionEnSegundos = JSON.parse(atob(token.split(".")[1])).exp;
        const expiracionEnMilisegundos = expiracionEnSegundos * 1000;
        const fechaExpiracion = new Date(expiracionEnMilisegundos);
        const fechaActual = new Date();

        if (fechaExpiracion <= fechaActual) {
          localStorage.removeItem("token");
          Swal.fire({
            icon: "warning",
            title: "Tu sesión ha expirado",
            text: "Te estamos redirigiendo a la página de autenticación...",
            timer: 4500,
            timerProgressBar: true,
            showConfirmButton: false,
          });
          navigate("/login");
        }
      }, 3 * 60 * 60 * 1000); // 3 horas

      return () => {
        clearInterval(temporizador);
      };
    }
  }, [navigate, costoEnvio, entrega]);

  useEffect(() => {
    setCantidadPedidosPendientes(
      orders.filter((order) => !order.verificado).length
    );
  }, [orders]);
  //#endregion

  //#region Función para ajustar el costo de envío y el total según la forma de entrega seleccionada
  const ajustarCostoYTotal = (selectedEntrega) => {
    if (selectedEntrega === "1") {
      // Si la nueva forma de entrega tiene un costo de envío de $0
      // Restablecer el costo de envío a 0 y restar el costo de envío actual del total
      setCostoEnvio(0);
    } else {
      // Si la nueva forma de entrega tiene un costo de envío diferente de $0
      // Actualizar el costo de envío y sumar el nuevo costo al total
      setCostoEnvio(costoEnvioDomicilio);
    }
  };
  //#endregion

  //#region Función para cambiar el estado de un pedido a pendiente
  const Pending = async (order) => {
    const idPedido = order.idPedido; // Obtener el idPedido del objeto order

    Swal.fire({
      icon: "warning",
      title: "¿Está seguro de que desea quitar la verificacion del pedido?",
      text: "Se cambiará el estado del pedido a pendiente",
      confirmButtonText: "Aceptar",
      showCancelButton: true,
      cancelButtonText: "Cancelar",
      confirmButtonColor: "#f8bb86",
      cancelButtonColor: "#6c757d",
    }).then(async (result) => {
      if (result.isConfirmed) {
        try {
          await UpdateOrdersVerified(
            orders.find((u) => u.idPedido === idPedido).idPedido || idPedido,
            {
              idPedido: idPedido,
              verificado: false,
            },
            headers
          );
          Swal.fire({
            icon: "success",
            title: "Estado del pedido actualizado exitosamente!",
            showConfirmButton: false,
            timer: 2000,
          });
          await GetOrders(setOrders);

          setOrders((prevOrders) => {
            setOriginalOrdersList(prevOrders);

            if (filterType === "type") {
              const result = prevOrders.filter((order) => {
                return order.tipo === filterName;
              });

              setTitle(`Detalles de Pedidos de ${filterName}`);
              setOrders(result);
              setQuery("");
              document.getElementById("clear-filter").style.display = "flex";
              document.getElementById("clear-filter2").style.display = "flex";
              setFilterName(filterName);
              setFilterType("type");
              ClearPending();
              setCurrentPage(1);
            }

            if (filterType === "pending") {
              const result = prevOrders.filter((order) => {
                return order.verificado === false;
              });
              setTitle("Detalles de Pedidos pendientes");
              setOrders(result);
              setQuery("");
              document.getElementById("clear-filter").style.display = "flex";
              document.getElementById("clear-filter2").style.display = "flex";
              setFilterName("Pendiente");
              setFilterType("pending");
              setCurrentPage(1);
            }

            if (filterType === "search") {
              const result = prevOrders.filter((order) => {
                return order.idPedido
                  .toLowerCase()
                  .includes(filterName.toLowerCase());
              });
              setOrders(result);
              document.getElementById("clear-filter").style.display = "flex";
              document.getElementById("clear-filter2").style.display = "flex";
              setTitle(`Detalles de Pedidos con ID: "${filterName}"`);
              setFilterName(filterName);
              setFilterType("search");
              ClearPending();
              setCurrentPage(1);
              if (filterName === "") {
                document.getElementById("clear-filter").style.display = "none";
                document.getElementById("clear-filter2").style.display = "none";
                setFilterType("");
                setTitle("Detalles de Pedidos");
              }
            }
            if (filterType === "other") {
              setOrders(prevOrders);
            } else {
              return prevOrders;
            }
          });
        } catch (error) {
          Swal.fire({
            title: error,
            icon: "error",
            confirmButtonText: "Aceptar",
            confirmButtonColor: "#f27474",
          });
        }
      }
    });
  };
  //#endregion

  //#region Función para cambiar el estado de un pedido a verificado
  const Verify = async (order) => {
    const idPedido = order.idPedido; // Obtener el idPedido del objeto order

    Swal.fire({
      icon: "warning",
      title: "¿Está seguro de que desea verificar el pedido?",
      text: "Se cambiará el estado del pedido a verificado",
      confirmButtonText: "Aceptar",
      showCancelButton: true,
      cancelButtonText: "Cancelar",
      confirmButtonColor: "#f8bb86",
      cancelButtonColor: "#6c757d",
    }).then(async (result) => {
      if (result.isConfirmed) {
        try {
          await UpdateOrdersVerified(
            orders.find((u) => u.idPedido === idPedido).idPedido || idPedido,
            {
              idPedido: idPedido,
              verificado: true,
            },
            headers
          );
          Swal.fire({
            icon: "success",
            title: "Estado del pedido actualizado exitosamente!",
            showConfirmButton: false,
            timer: 2000,
          });
          await GetOrders(setOrders);

          setOrders((prevOrders) => {
            setOriginalOrdersList(prevOrders);

            if (filterType === "type") {
              const result = prevOrders.filter((order) => {
                return order.tipo === filterName;
              });

              setTitle(`Detalles de Pedidos de ${filterName}`);
              setOrders(result);
              setQuery("");
              document.getElementById("clear-filter").style.display = "flex";
              document.getElementById("clear-filter2").style.display = "flex";
              setFilterName(filterName);
              setFilterType("type");
              ClearPending();
              setCurrentPage(1);
            }

            if (filterType === "pending") {
              const result = prevOrders.filter((order) => {
                return order.verificado === false;
              });
              setTitle("Detalles de Pedidos pendientes");
              setOrders(result);
              setQuery("");
              document.getElementById("clear-filter").style.display = "flex";
              document.getElementById("clear-filter2").style.display = "flex";
              setFilterName("Pendiente");
              setFilterType("pending");
              setCurrentPage(1);
            }

            if (filterType === "search") {
              const result = prevOrders.filter((order) => {
                return order.idPedido
                  .toLowerCase()
                  .includes(filterName.toLowerCase());
              });
              setOrders(result);
              document.getElementById("clear-filter").style.display = "flex";
              document.getElementById("clear-filter2").style.display = "flex";
              setTitle(`Detalles de Pedidos con ID: "${filterName}"`);
              setFilterName(filterName);
              setFilterType("search");
              ClearPending();
              setCurrentPage(1);
              if (filterName === "") {
                document.getElementById("clear-filter").style.display = "none";
                document.getElementById("clear-filter2").style.display = "none";
                setFilterType("");
                setTitle("Detalles de Pedidos");
              }
            }
            if (filterType === "other") {
              setOrders(prevOrders);
            } else {
              return prevOrders;
            }
          });
        } catch (error) {
          Swal.fire({
            title: error,
            icon: "error",
            confirmButtonText: "Aceptar",
            confirmButtonColor: "#f27474",
          });
        }
      }
    });
  };
  //#endregion

  //#region Función para borrar el estado de pendiente
  const ClearPending = () => {
    if (pending === true) {
      setPending(false);
    }
  };
  //#endregion

  //#region Función para borrar cualquier filtro
  const ClearFilter = () => {
    setOrders(originalOrdersList); // trae la lista de pedidos original, sin ningun filtro
    setTipo("");
    setQuery("");
    setFilterName("");
    setFilterType("");
    setTitle("Detalles de Pedidos");
    document.getElementById("clear-filter").style.display = "none";
    document.getElementById("clear-filter2").style.display = "none"; // esconde del DOM el boton de limpiar filtros
    setCurrentPage(1);
    ClearPending();
    window.scrollTo(0, 0);
  };
  //#endregion

  //#region Función para filtrar pedidos por tipo de pedido
  const filterResultType = (type) => {
    // Hacer una copia de la lista original
    const originalOrdersCopy = [...originalOrdersList];

    // Aplicar el filtro sobre la copia
    const result = originalOrdersCopy.filter((order) => order.tipo === type);

    setTitle(`Detalles de Pedidos ${type}`);
    setOrders(result);
    setQuery("");
    document.getElementById("clear-filter").style.display = "flex";
    document.getElementById("clear-filter2").style.display = "flex";
    setFilterName(type);
    setFilterType("type");
    setCurrentPage(1);
    ClearPending();
    window.scrollTo(0, 0);
  };
  //#endregion

  //#region Función para filtrar pedidos por estado pendiente
  const filterResultPending = (pending) => {
    if (pending === false) {
      // Hacer una copia de la lista original
      const originalOrdersCopy = [...originalOrdersList];

      // Aplicar el filtro sobre la copia
      const result = originalOrdersCopy.filter((order) => !order.verificado);

      setTitle("Detalles de Pedidos pendientes");
      setOrders(result);
      setQuery("");
      document.getElementById("clear-filter").style.display = "flex";
      document.getElementById("clear-filter2").style.display = "flex";
      setFilterName("Pendiente");
      setFilterType("pending");
      setCurrentPage(1);
      window.scrollTo(0, 0);
      setTipo("");
    } else {
      ClearFilter();
    }
  };
  //#endregion

  //#region Función para filtrar pedido mediante una consulta personalizada por su ID
  const search = () => {
    setOrders(originalOrdersList);
    const result = orders.filter((order) =>
      order.idPedido.toString().toLowerCase().includes(query.toLowerCase())
    );
    setOrders(result);
    document.getElementById("clear-filter").style.display = "flex";
    document.getElementById("clear-filter2").style.display = "flex";
    setTitle(`Detalles de Pedido con ID: "${query}"`);
    setFilterName(query);
    setFilterType("search");
    ClearPending();
    setCurrentPage(1);
    window.scrollTo(0, 0);
    if (query === "") {
      document.getElementById("clear-filter").style.display = "none";
      document.getElementById("clear-filter2").style.display = "none";
      setFilterType("");
      setTitle("Detalles de Pedidos");
      window.scrollTo(0, 0);
    }
  };
  //#endregion

  //#region Funciónes para la paginacion
  function prePage() {
    if (currentPage !== 1) {
      setCurrentPage(currentPage - 1);
    }
  }

  function nextPage() {
    if (currentPage !== npage) {
      setCurrentPage(currentPage + 1);
    }
  }

  function changeCPage(id) {
    setCurrentPage(id);
  }
  //#endregion

  //#region Función para limpiar todos los valores de los inputs del formulario
  function ClearOrderInputs() {
    setIdPedido("");

    setEntrega("");
    setVendedor("");
    setCostoEnvio("");
    setAbono("");
  }
  //#endregion

  //#region Función para obtener los valores almacenados de un pedido y cargar cada uno de ellos en su input correspondiente
  function RetrieveOrderInputs(order) {
    setIdPedido(order.idPedido);

    if (order.entrega === "Lo retiro por el local") {
      setEntrega(1);
    } else if (order.entrega === "Envío a domicilio") {
      setEntrega(2);
    }

    setVendedor(getIdVendedor(order.vendedor));
    setCostoEnvio(order.costoEnvio);

    if (order.abono === "Efectivo") {
      setAbono(1);
    } else if (order.abono === "Transferencia") {
      setAbono(2);
    } else if (order.abono === "Tarjeta de débito") {
      setAbono(3);
    } else if (order.abono === "Tarjeta de crédito") {
      setAbono(4);
    } else if (order.abono === "Mercado Pago") {
      setAbono(5);
    }

    if (order.entrega === "Lo retiro por el local") {
      setPrevEntrega(1);
    } else if (order.entrega === "Envío a domicilio") {
      setPrevEntrega(2);
    }
    setPrevVendedor(getIdVendedor(order.vendedor));
    setPrevCostoEnvio(order.costoEnvio);

    if (order.abono === "Efectivo") {
      setPrevAbono(1);
    } else if (order.abono === "Transferencia") {
      setPrevAbono(2);
    } else if (order.abono === "Tarjeta de débito") {
      setPrevAbono(3);
    } else if (order.abono === "Tarjeta de crédito") {
      setPrevAbono(4);
    } else if (order.abono === "Mercado Pago") {
      setPrevAbono(5);
    }
  }
  //#endregion

  //#region Función para volver el formulario a su estado inicial, borrando los valores de los inputs, cargando los selects y refrezcando la lista de pedidos
  function InitialState() {
    ClearOrderInputs();
    GetOrders(setOrders);
    GetOrders(setOriginalOrdersList);
  }
  //#endregion

  //#region Función para cerrar el modal manualmente mediante el codigo
  function CloseModal() {
    $(document).ready(function () {
      $("#btn-close-modal").click();
    });
  }
  //#endregion

  //#region Función para cerrar el modal de filtros manualmente mediante el codigo
  function CloseFilterModal() {
    $(document).ready(function () {
      $("#btn-close-modal-filters").click();
    });
  }
  //#endregion

  //#region Función para verificar si los valores ingresados a traves de los inputs son correctos
  function IsValid() {
    if (entrega === "") {
      Swal.fire({
        icon: "error",
        title: "El tipo de entrega no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#entrega").focus();
        }, 500);
      });
      return false;
    } else if (vendedor === 0) {
      Swal.fire({
        icon: "error",
        title: "El nombre completo del vendedor no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#vendedor").focus();
        }, 500);
      });
      return false;
    } else if (costoEnvio === "") {
      Swal.fire({
        icon: "error",
        title:
          "El costo del envío puede estar vacío, si no tiene costo ingrese 0",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#costoEnvio").focus();
        }, 500);
      });
      return false;
    } else if (abono === "") {
      Swal.fire({
        icon: "error",
        title: "El abono no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#abono").focus();
        }, 500);
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si se actualizo el valor de al menos un input
  function IsUpdated() {
    if (
      prevEntrega !== entrega ||
      prevVendedor !== vendedor ||
      prevCostoEnvio !== costoEnvio ||
      prevAbono !== abono
    ) {
      return true;
    }
    return false;
  }
  //#endregion

  //#region Función para actualizar un pedido ya existente
  async function UpdateOrder(event) {
    event.preventDefault();

    if (IsUpdated() === false) {
      Swal.fire({
        icon: "error",
        title: "No puede actualizar el pedido sin modificar ningun campo",
        text: "Modifique al menos un campo para poder actualizarlo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#F27474",
      });
    } else if (IsValid() === true && IsUpdated() === true) {
      try {
        await UpdateOrders(
          orders.find((u) => u.idPedido === idPedido).idPedido || idPedido,
          {
            idPedido: idPedido,
            costoEnvio: costoEnvio,
            idVendedor: vendedor,
            idMetodoPago: abono,
            IdMetodoEntrega: entrega,
          },
          headers
        );
        Swal.fire({
          icon: "success",
          title: "Pedido actualizado exitosamente!",
          showConfirmButton: false,
          timer: 2000,
        });
        CloseModal();

        // InitialState();
        ClearOrderInputs();
        await GetOrders(setOrders);

        setOrders((prevOrders) => {
          setOriginalOrdersList(prevOrders);

          if (filterType === "type") {
            const result = prevOrders.filter((order) => {
              return order.tipo === filterName;
            });

            setTitle(`Detalles de Pedidos de ${filterName}`);
            setOrders(result);
            setQuery("");
            document.getElementById("clear-filter").style.display = "flex";
            document.getElementById("clear-filter2").style.display = "flex";
            setFilterName(filterName);
            setFilterType("type");
            ClearPending();
            setCurrentPage(1);
          }

          if (filterType === "pending") {
            const result = prevOrders.filter((order) => {
              return order.verificado === false;
            });
            setTitle("Detalles de Pedidos pendientes");
            setOrders(result);
            setQuery("");
            document.getElementById("clear-filter").style.display = "flex";
            document.getElementById("clear-filter2").style.display = "flex";
            setFilterName("Pendiente");
            setFilterType("pending");
            setCurrentPage(1);
          }

          if (filterType === "search") {
            const result = prevOrders.filter((order) => {
              return order.idPedido
                .toLowerCase()
                .includes(filterName.toLowerCase());
            });
            setOrders(result);
            document.getElementById("clear-filter").style.display = "flex";
            document.getElementById("clear-filter2").style.display = "flex";
            setTitle(`Detalles de Pedidos con ID: "${filterName}"`);
            setFilterName(filterName);
            setFilterType("search");
            ClearPending();
            setCurrentPage(1);
            if (filterName === "") {
              document.getElementById("clear-filter").style.display = "none";
              document.getElementById("clear-filter2").style.display = "none";
              setFilterType("");
              setTitle("Detalles de Pedidos");
            }
          }
          if (filterType === "other") {
            setOrders(prevOrders);
          } else {
            return prevOrders;
          }
        });
      } catch (err) {
        Swal.fire({
          title: err,
          icon: "error",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#f27474",
        });
      }
    }
  }
  //#endregion

  //#region Función para eliminar un pedido existente
  async function DeleteOrder(id) {
    await DeleteOrders(id, headers);
    Swal.fire({
      icon: "success",
      title: "Pedido eliminado exitosamente!",
      showConfirmButton: false,
      timer: 2000,
    });
    InitialState();
    ClearFilter();
  }
  //#endregion

  //#region Función para obtener el id del vendedor de un pedido
  const getIdVendedor = (nombre) => {
    const vendedor = listaNombresVendedores.find((v) => v.nombre === nombre);
    return vendedor ? vendedor.idUsuario : "-1";
  };
  //#endregion

  //#region Return
  return (
    <div>
      <Helmet>
        <title>Catapp | Administrar Pedidos</title>
      </Helmet>

      <section className="general-container">
        <div className="general-content">
          <div className="general-title">
            <div className="title-header">
              <Link
                to="/panel-de-administrador"
                className="btn btn-info btn-back"
              >
                <div className="btn-back-content">
                  <Back className="back" />
                  <p className="p-back">Regresar</p>
                </div>
              </Link>

              <h2 className="title title-general">{title}</h2>

              {cantidadPedidosPendientes > 0 && !pending && tipo === "" && (
                <button
                  type="button"
                  className="btn btn-warning btn-orders"
                  title="Ver pedidos pendientes"
                  onClick={() => {
                    Swal.fire({
                      title: `Hay ${cantidadPedidosPendientes} ${
                        cantidadPedidosPendientes === 1 ? "pedido" : "pedidos"
                      } pendientes`,
                      text: "¿Desea visualizarlos?",
                      icon: "question",
                      showCancelButton: true,
                      confirmButtonColor: "#87adbd",
                      cancelButtonColor: "#6c757d",
                      confirmButtonText: "Aceptar",
                      cancelButtonText: "Cancelar",
                      focusCancel: true,
                    }).then((result) => {
                      if (result.isConfirmed) {
                        filterResultPending(false);
                        setPending(true);
                      }
                    });
                  }}
                >
                  {cantidadPedidosPendientes}
                </button>
              )}
            </div>

            {orders.length > 1 || orders.length === 0 ? (
              <p className="total">Hay {orders.length} pedidos.</p>
            ) : (
              <p className="total">Hay {orders.length} pedido.</p>
            )}
          </div>

          {/* modal con el formulario para actualizar un pedido */}
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
                    Actualizar Pedido
                  </h1>
                </div>
                <div className="modal-body">
                  <div className="container mt-4">
                    <form>
                      <div className="form-group">
                        <input
                          type="text"
                          className="input"
                          id="idPedido"
                          hidden
                          value={idPedido}
                          onChange={(event) => {
                            setIdPedido(event.target.value);
                          }}
                        />

                        <label className="label selects" htmlFor="envio">
                          Entrega:
                        </label>
                        <div className="form-group-input nombre-input">
                          <span className="input-group-text">
                            <EntregaInput className="input-group-svg" />
                          </span>
                          <select
                            className="input"
                            style={{ cursor: "pointer" }}
                            name="envio"
                            id="envio"
                            value={entrega}
                            onChange={(e) => {
                              setEntrega(e.target.value);
                              ajustarCostoYTotal(e.target.value);
                            }}
                          >
                            <option hidden key={0} value="0">
                              Seleccione una opción
                            </option>
                            <option className="btn-option" value="1">
                              Lo retiro por el local ($0)
                            </option>
                            <option className="btn-option" value="2">
                              Envío a domicilio (${costoEnvioDomicilio})
                            </option>
                          </select>
                        </div>

                        <label className="label selects" htmlFor="vendedor">
                          Vendedor:
                        </label>
                        <div className="form-group-input nombre-input">
                          <span className="input-group-text">
                            <SellerInput className="input-group-svg" />
                          </span>
                          <select
                            className="input"
                            style={{ cursor: "pointer" }}
                            name="vendedor"
                            id="vendedor"
                            value={vendedor}
                            onChange={(e) => setVendedor(e.target.value)}
                          >
                            <option hidden key={0} value="0">
                              Seleccione un vendedor
                            </option>
                            {listaNombresVendedores &&
                              Array.from(listaNombresVendedores).map(
                                (opts, i) => (
                                  <option
                                    className="btn-option"
                                    key={i}
                                    value={opts.idUsuario}
                                  >
                                    {opts.nombre}
                                  </option>
                                )
                              )}
                            <option className="no-vendedor" value="-1">
                              No recibió atención
                            </option>
                          </select>
                        </div>

                        <label className="label">Costo de envío:</label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <CostoEnvioInput className="input-group-svg" />
                          </span>
                          <input
                            style={{
                              backgroundColor: "#d3d3d3",
                              cursor: "default",
                            }}
                            type="number"
                            step="1"
                            min={0}
                            className="input"
                            id="costoEnvio"
                            value={costoEnvio}
                            onChange={(event) => {
                              setCostoEnvio(event.target.value);
                            }}
                            readOnly
                          />
                        </div>

                        <label className="label selects" htmlFor="abono">
                          Abono:
                        </label>
                        <div className="form-group-input nombre-input">
                          <span className="input-group-text">
                            <PaymentInput className="input-group-svg" />
                          </span>
                          <select
                            className="input"
                            style={{ cursor: "pointer" }}
                            name="abono"
                            id="abono"
                            value={abono}
                            onChange={(e) => setAbono(e.target.value)}
                          >
                            <option hidden key={0} value="0">
                              Seleccione una opción
                            </option>
                            <option className="btn-option" value="1">
                              Efectivo
                            </option>
                            <option className="btn-option" value="2">
                              Transferencia
                            </option>
                            <option className="btn-option" value="3">
                              Tarjeta de débito
                            </option>
                            <option className="btn-option" value="4">
                              Tarjeta de crédito
                            </option>
                            <option className="btn-option" value="5">
                              Mercado Pago
                            </option>
                          </select>
                        </div>
                      </div>

                      <div>
                        <div id="div-btn-update">
                          <button
                            className="btn btn-warning btn-edit-color"
                            id="btn-update"
                            onClick={UpdateOrder}
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
                        ClearOrderInputs();
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
                            ClearOrderInputs();
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

          {/* modal con filtros */}
          <div
            className="modal fade"
            id="modal-filters"
            data-bs-backdrop="static"
            data-bs-keyboard="false"
            tabIndex="-1"
            aria-labelledby="staticBackdropLabel"
            aria-hidden="true"
          >
            <div className="modal-dialog modal-dialog2">
              <div className="modal-content">
                <div className="modal-header2">
                  <h1 className="modal-title2" id="exampleModalLabel">
                    Filtros
                  </h1>
                  <button
                    id="clear-filter2"
                    className="clear-filter2"
                    onClick={ClearFilter}
                  >
                    <Close className="close-svg2" />
                    <p className="clear-filter-p">{filterName}</p>
                  </button>
                </div>
                <div className="modal-body">
                  <div className="container">
                    <p className="filter-separator separator-margin"></p>

                    <div
                      className="filter-btn-container"
                      data-bs-toggle="collapse"
                      href="#collapseCategory"
                      role="button"
                      aria-expanded="false"
                      aria-controls="collapseCategory"
                    >
                      <p className="filter-btn-name">TIPOS</p>

                      <div className="form-group-input">
                        <select
                          className="input2"
                          style={{ cursor: "pointer" }}
                          name="tipo"
                          id="tipo"
                          value={tipo}
                          onChange={(e) => {
                            setTipo(e.target.value);
                            filterResultType(e.target.value);
                          }}
                        >
                          <option hidden key={0} value="0">
                            Seleccione una opción
                          </option>
                          <option className="btn-option" value="Minorista">
                            Minorista
                          </option>
                          <option className="btn-option" value="Mayorista">
                            Mayorista
                          </option>
                        </select>
                      </div>
                    </div>

                    <p className="filter-separator"></p>

                    <div className="filter-btn-container">
                      <p className="filter-btn-name">PENDIENTE</p>
                      <p className="filter-btn">
                        <input
                          type="checkbox"
                          className="form-check-input tick"
                          id="pending"
                          checked={pending}
                          onChange={() => {
                            setPending(!pending);
                            filterResultPending(pending);
                          }}
                        />
                        <label htmlFor="pending" className="lbl-switch"></label>
                      </p>
                    </div>
                  </div>
                </div>
                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={CloseFilterModal}
                  >
                    <div className="btn-save-update-close">
                      <Close className="close-btn" />
                      <p className="p-save-update-close">Cerrar</p>
                    </div>
                  </button>

                  <button
                    type="button"
                    className="btn-close-modal"
                    id="btn-close-modal-filters"
                    data-bs-dismiss="modal"
                  ></button>
                </div>
              </div>
            </div>
          </div>

          <div className="filters-left2">
            <div className="pagination-count3">
              <div className="search-container">
                <div className="form-group-input-search2">
                  <span className="input-group-text3">
                    <Lupa className="input-group-svg" />
                  </span>
                  <input
                    className="search-input2"
                    type="text"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    onKeyUp={search}
                    placeholder="Buscar..."
                  />
                </div>
              </div>
            </div>

            <div className="pagination-count-filter">
              <button
                className="btn btn-secondary btn-filters"
                data-bs-toggle="modal"
                data-bs-target="#modal-filters"
              >
                <div
                  className="filter-btn-title-container-2"
                  id="filter-btn-title-container"
                >
                  <p className="filter-btn">
                    <Filter className="filter-svg2" />
                  </p>
                  <p className="filter-title2">Filtros</p>
                </div>
              </button>

              <button
                id="clear-filter"
                className="clear-filter2"
                onClick={ClearFilter}
              >
                <Close className="close-svg2" />
                <p className="clear-filter-p">{filterName}</p>
              </button>
            </div>

            <div className="header-excel">
              <button
                onClick={onDownload}
                type="button"
                className="btn btn-success btn-excel"
              >
                <div className="btn-add-content">
                  <Excel className="excel" />
                  <p className="p-add">Descargar</p>
                </div>
              </button>
            </div>
          </div>

          {/* tabla de pedidos */}
          <table
            className="table table-dark table-bordered table-hover table-list"
            align="center"
          >
            <thead>
              <tr className="table-header">
                <th className="table-title" scope="col">
                  #
                </th>
                <th className="table-title" scope="col">
                  ID
                </th>
                <th className="table-title" scope="col">
                  Tipo
                </th>
                <th className="table-title" scope="col">
                  Cliente
                </th>
                <th className="table-title" scope="col">
                  Entrega
                </th>
                <th className="table-title" scope="col">
                  Vendedor
                </th>
                <th className="table-title" scope="col">
                  Cantidad de productos
                </th>
                <th className="table-title" scope="col">
                  Subtotal
                </th>
                <th className="table-title" scope="col">
                  Costo de envío
                </th>
                <th className="table-title" scope="col">
                  Total
                </th>
                <th className="table-title" scope="col">
                  Abono
                </th>
                <th className="table-title" scope="col">
                  Detalle
                </th>
                <th className="table-title" scope="col">
                  Fecha
                </th>
                <th className="table-title" scope="col">
                  Status
                </th>
                {(rolUsuario === "Supervisor" ||
                  rolUsuario === "SuperAdmin") && (
                  <th className="table-title" scope="col">
                    Acciones
                  </th>
                )}
              </tr>
            </thead>

            {orders.length > 0 ? (
              ordersTable.map(function fn(order, index) {
                return (
                  <tbody key={1 + order.idPedido}>
                    <tr>
                      <th scope="row" className="table-name">
                        {index + 1}
                      </th>
                      <td className="table-name">{order.idPedido}</td>
                      <td className="table-name">{order.tipo}</td>
                      <td className="table-name">{order.cliente}</td>
                      <td
                        className={`table-name ${
                          order.entrega.includes("domicilio")
                            ? "domicilio"
                            : order.entrega.includes("retiro por el local")
                            ? "retiro-local"
                            : "domicilio"
                        }`}
                      >
                        {order.entrega}
                      </td>
                      <td
                        className={`table-name ${
                          order.vendedor === null ? "predeterminado" : ""
                        }`}
                      >
                        {order.vendedor === null ? "-" : order.vendedor}
                      </td>
                      <td className="table-name">{order.cantidadProductos}</td>
                      <td className="table-name">
                        ${order.subtotal.toLocaleString()}
                      </td>
                      <td
                        className={`table-name ${
                          order.costoEnvio > 0
                            ? "domicilio"
                            : order.entrega.includes("retiro por el local")
                            ? "retiro-local"
                            : "domicilio"
                        }`}
                      >
                        ${order.costoEnvio.toLocaleString()}
                      </td>
                      <td className="table-name">
                        ${order.total.toLocaleString()}
                      </td>
                      <td
                        className={`table-name ${
                          order.abono === "Mercado Pago" ? "mercado-pago" : "table-name"
                        }`}
                      >
                        {order.abono}
                      </td>
                      <td className="table-name table-overflow">
                        <pre>{order.detalle.split("|").join("\n")}</pre>
                      </td>
                      <td className="table-name">{formatDate(order.fecha)}</td>

                      {order.verificado ? (
                        <td className="table-name">
                          <div className="status-btns">
                            <div className="circulo-verificado"></div>
                            <p className="status-name">Verificado</p>
                            {(rolUsuario === "Supervisor" ||
                              rolUsuario === "SuperAdmin") && (
                              <button
                                type="button"
                                className="btn btn-light btn-delete4"
                                onClick={() => {
                                  Pending(order);
                                }}
                              >
                                <Pendiente className="edit3" />
                              </button>
                            )}
                          </div>
                        </td>
                      ) : (
                        <td className="table-name">
                          <div className="status-btns">
                            <div className="circulo-pendiente"></div>
                            <p className="status-name">Pendiente</p>
                            {(rolUsuario === "Supervisor" ||
                              rolUsuario === "SuperAdmin") && (
                              <button
                                type="button"
                                className="btn btn-light btn-delete4"
                                onClick={() => {
                                  Verify(order);
                                }}
                              >
                                <Verificar className="edit3" />
                              </button>
                            )}
                          </div>
                        </td>
                      )}

                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td className="table-name">
                          <button
                            type="button"
                            className="btn btn-warning btn-edit"
                            data-bs-toggle="modal"
                            data-bs-target="#modal"
                            onClick={() => {
                              RetrieveOrderInputs(order);
                            }}
                          >
                            <Edit className="edit" />
                          </button>

                          <button
                            type="button"
                            className="btn btn-danger btn-delete"
                            onClick={() =>
                              Swal.fire({
                                title:
                                  "Esta seguro de que desea eliminar el siguiente pedido: " +
                                  order.idPedido +
                                  "?",
                                text: "Una vez eliminado, no se podra recuperar",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#F8BB86",
                                cancelButtonColor: "#6c757d",
                                confirmButtonText: "Aceptar",
                                cancelButtonText: "Cancelar",
                                focusCancel: true,
                              }).then((result) => {
                                if (result.isConfirmed) {
                                  DeleteOrder(order.idPedido);
                                }
                              })
                            }
                          >
                            <Delete className="delete" />
                          </button>
                        </td>
                      )}
                    </tr>
                  </tbody>
                );
              })
            ) : (
              <tbody>
                <tr className="tr-name1">
                  <td className="table-name table-name1" colSpan={15}>
                    Sin registros
                  </td>
                </tr>
              </tbody>
            )}
          </table>

          {/* tabla de pedidos */}
          <table
            ref={tableRef}
            className="table table-dark table-list-none"
            align="center"
          >
            <thead>
              <tr className="table-header">
                <th className="table-title" scope="col">
                  ID
                </th>
                <th className="table-title" scope="col">
                  Tipo
                </th>
                <th className="table-title" scope="col">
                  Cliente
                </th>
                <th className="table-title" scope="col">
                  Entrega
                </th>
                <th className="table-title" scope="col">
                  Vendedor
                </th>
                <th className="table-title" scope="col">
                  Cantidad de productos
                </th>
                <th className="table-title" scope="col">
                  Subtotal
                </th>
                <th className="table-title" scope="col">
                  Costo de envío
                </th>
                <th className="table-title" scope="col">
                  Total
                </th>
                <th className="table-title" scope="col">
                  Abono
                </th>
                <th className="table-title" scope="col">
                  Detalle
                </th>
                <th className="table-title" scope="col">
                  Fecha
                </th>
                <th className="table-title" scope="col">
                  Status
                </th>
              </tr>
            </thead>

            {orders.length > 0 ? (
              orders.map(function fn(order) {
                return (
                  <tbody key={1 + order.idPedido}>
                    <tr>
                      <td className="table-name">{order.idPedido}</td>
                      <td className="table-name">{order.tipo}</td>
                      <td className="table-name">{order.cliente}</td>
                      <td className="table-name">{order.entrega}</td>
                      <td
                        className={`table-name ${
                          order.vendedor === null ? "predeterminado" : ""
                        }`}
                      >
                        {order.vendedor === null ? "-" : order.vendedor}
                      </td>
                      <td className="table-name">{order.cantidadProductos}</td>
                      <td className="table-name">
                        {order.subtotal.toLocaleString()}
                      </td>
                      <td className="table-name">
                        {order.costoEnvio.toLocaleString()}
                      </td>
                      <td className="table-name">
                        {order.total.toLocaleString()}
                      </td>
                      <td className="table-name">{order.abono}</td>
                      <td className="table-name">{order.detalle}</td>
                      <td className="table-name">{formatDate(order.fecha)}</td>
                      {order.verificado ? (
                        <td className="table-name">Verificado</td>
                      ) : (
                        <td className="table-name">Pendiente</td>
                      )}
                    </tr>
                  </tbody>
                );
              })
            ) : (
              <tbody>
                <tr>
                  <td className="table-name" colSpan={11}>
                    Sin registros
                  </td>
                </tr>
              </tbody>
            )}
          </table>

          <div className="pagination-count-container2">
            <div className="pagination-count">
              {orders.length > 0 ? (
                orders.length === 1 ? (
                  <p className="total">
                    Pedido {firstIndex + 1} de {orders.length}
                  </p>
                ) : (
                  <p className="total">
                    Pedidos {firstIndex + 1} a {ordersTable.length + firstIndex}{" "}
                    de {orders.length}
                  </p>
                )
              ) : (
                <></>
              )}
            </div>

            {orders.length > 0 ? (
              <ul className="pagination-manager">
                <div className="page-item">
                  <div className="page-link" onClick={prePage}>
                    {"<"}
                  </div>
                </div>

                <div className="numbers">
                  {numbers.map((n, i) => {
                    if (n === currentPage) {
                      return (
                        <ul className="page-item-container">
                          <li className="page-item active" key={i}>
                            <div className="page-link">{n}</div>
                          </li>
                        </ul>
                      );
                    } else if (
                      n === 1 ||
                      n === npage ||
                      (n >= currentPage - maxPageNumbersToShow &&
                        n <= currentPage + maxPageNumbersToShow)
                    ) {
                      return (
                        <li className="page-item" key={i}>
                          <div
                            className="page-link"
                            onClick={() => changeCPage(n)}
                          >
                            {n}
                          </div>
                        </li>
                      );
                    } else if (
                      (n === currentPage - maxPageNumbersToShow - 1 &&
                        currentPage - maxPageNumbersToShow >
                          minPageNumbersToShow) ||
                      (n === currentPage + maxPageNumbersToShow + 1 &&
                        currentPage + maxPageNumbersToShow <
                          npage - minPageNumbersToShow)
                    ) {
                      return (
                        <li className="page-item" key={i}>
                          <div className="page-link">...</div>
                        </li>
                      );
                    } else {
                      return null;
                    }
                  })}
                </div>

                <div className="page-item">
                  <div className="page-link" onClick={nextPage}>
                    {">"}
                  </div>
                </div>
              </ul>
            ) : (
              <></>
            )}

            <div className="pagination-count"></div>
          </div>
        </div>
      </section>
    </div>
  );
  //#endregion
}

export default OrderManager;
