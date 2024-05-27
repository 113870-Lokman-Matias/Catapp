import React, { useState, useEffect } from "react";
import { Helmet } from "react-helmet";
import $ from "jquery";
import Swal from "sweetalert2";

import * as signalR from "@microsoft/signalr";

import {
  GetCategoriesMinorista,
  GetCategoriesMayorista,
} from "../../../services/CategoryService";
import { GetCotizacionDolarUnicamente } from "../../../services/DollarService";
import { GetInfoEnvio } from "../../../services/ShipmentService";
import { GetUsersSellers } from "../../../services/UserService";
import { SaveOrders } from "../../../services/OrderService";
import {
  GetProductsByCategory,
  GetProductsByQuery,
} from "../../../services/ProductService";
import {
  PayWithMercadoPago,
  GetPaymentInfo,
} from "../../../services/PaymentService";

import { initMercadoPago, Wallet } from "@mercadopago/sdk-react";

//#region Imports de los SVG'S
import { ReactComponent as Zoom } from "../../../assets/svgs/zoom.svg";
import { ReactComponent as Cart } from "../../../assets/svgs/cart.svg";
import { ReactComponent as Back } from "../../../assets/svgs/cartBack.svg";
import { ReactComponent as Delete } from "../../../assets/svgs/delete.svg";
import { ReactComponent as Whatsapplogo } from "../../../assets/svgs/whatsapp.svg";
import { ReactComponent as Close } from "../../../assets/svgs/closebtn.svg";
import { ReactComponent as Save } from "../../../assets/svgs/save.svg";
import { ReactComponent as Lupa } from "../../../assets/svgs/lupa.svg";
import { ReactComponent as Location } from "../../../assets/svgs/location.svg";
import { ReactComponent as Mercadopagologo } from "../../../assets/svgs/mercadopago.svg";
import Loader from "../../../components/Loaders/LoaderCircle";
//#endregion

import "../CatalogueCart/CatalogueCart.css";

const CatalogueCart = () => {
  //#region Constantes
  const [pedidoAprobado, setPedidoAprobado] = useState(false);

  const [showWallet, setShowWallet] = useState(false);
  const [preferenceId, setPreferenceId] = useState(null);

  const pathname = window.location.pathname.toLowerCase();
  const [clientType, setClientType] = useState("");

  const [click, setClick] = useState(false);
  const handleClick = () => setClick(!click);
  const [isVisible, setIsVisible] = useState(true);
  const [color, setColor] = useState(false);
  const changeColor = () => {
    if (window.scrollY >= 100) {
      setColor(true);
    } else {
      setColor(false);
    }
  };
  window.addEventListener("scroll", changeColor);

  const [categories, setCategories] = useState([]);
  const [categorySign, setCategorySign] = useState({});
  const [categoryProducts, setCategoryProducts] = useState({});
  const [productQuantities, setProductQuantities] = useState({});
  const [productNotes, setProductNotes] = useState({});
  const [cart, setCart] = useState({});
  const [totalQuantity, setTotalQuantity] = useState(0);
  const [total, setTotal] = useState(0);
  const [modalAbierto, setModalAbierto] = useState(false);
  const [showButton, setShowButton] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingProductByCategory, setIsLoadingProductByCategory] =
    useState(true);
  const [isLoadingQuery, setIsLoadingQuery] = useState(false);
  const [listaNombresVendedores, setListaNombresVendedores] = useState(true);

  //#region Constantes para el formulario del cliente
  const [nombre, setNombre] = useState("");
  const [dni, setDni] = useState("");
  const [direccion, setDireccion] = useState("");

  const [direccionAuto] = useState("Maestro M. Lopez (Catapp)");
  const [horariosAtencion] = useState(
    "Lunes a Viernes de 09:30 a 18:00 hs y Sábados de 9:00 a 13:30 hs"
  );
  const [telefonoEmpresa] = useState("3517476389");
  const [cbu] = useState("45300005445599555");
  const [costoEnvioDomicilio, setCostoEnvioDomicilio] = useState("");
  const [habilitadoEnvioDomicilio, setHabilitadoEnvioDomicilio] = useState("");

  const [valorDolar, setvalorDolar] = useState(0);

  const [calles, setCalles] = useState("");
  const [abono, setAbono] = useState("");

  const [telefono, setTelefono] = useState("");

  const [vendedor, setVendedor] = useState("");
  const [envio, setEnvio] = useState("");
  //#endregion

  //#region Constantes necesarias para el filtro por busqueda
  const [products, setProducts] = useState([]);
  const [query, setQuery] = useState("");
  const [searchValue, setSearchValue] = useState("");
  //#endregion
  //#endregion

  //#region UseEffect
  useEffect(() => {
    initMercadoPago("APP_USR-13e9d0f8-62f0-47f4-9e93-74dbc93351e3", {
      locale: "es-AR",
    });
  }, []);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7207/generalHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection
      .start()
      .then(() => {
        // console.log("Conexión establecida con el servidor SignalR");
      })
      .catch((err) => console.error(err.toString()));

    connection.on("MensajeCrudCategoria", async () => {
      try {
        if (pathname.includes("mayorista")) {
          GetCategoriesMayorista(setCategories);
        } else if (pathname.includes("minorista")) {
          GetCategoriesMinorista(setCategories);
        }
      } catch (error) {
        console.error("Error al obtener las categorias: " + error);
      }
    });

    // connection.on("MensajeCrudProducto", async () => {
    //   try {
    //       GetProducts(setOriginalProductsList);
    //     if (pathname.includes("mayorista")) {
    //       GetCategoriesMayorista(setCategories);
    //     } else if (pathname.includes("minorista")) {
    //       GetCategoriesMinorista(setCategories);
    //     }
    //   } catch (error) {
    //     console.error("Error al obtener los productos: " + error);
    //   }
    // });

    connection.on("MensajeUpdateCotizacion", async () => {
      try {
        await GetCotizacionDolarUnicamente(setvalorDolar);
      } catch (error) {
        console.error("Error al obtener la cotización: " + error);
      }
    });

    connection.on("MensajeUpdateEnvio", async () => {
      try {
        const response = await GetInfoEnvio();
        setCostoEnvioDomicilio(response.precio);
        setHabilitadoEnvioDomicilio(response.habilitado);

        if (response.habilitado === false) {
          setEnvio("");
        }
      } catch (error) {
        console.error("Error al obtener el costo de envío: " + error);
      }
    });

    connection.on("MensajeCrudVendedor", async () => {
      try {
        await GetUsersSellers(setListaNombresVendedores);
      } catch (error) {
        console.error("Error al obtener los vendedores: " + error);
      }
    });

    return () => {
      connection.stop();
    };
  }, []);

  useEffect(() => {
    // Funciónes asincronas
    (async () => {
      try {
        const response = await GetInfoEnvio();
        setCostoEnvioDomicilio(response.precio);
        setHabilitadoEnvioDomicilio(response.habilitado);

        await GetCotizacionDolarUnicamente(setvalorDolar);
        await GetUsersSellers(setListaNombresVendedores);
      } catch (error) {
        console.log(error);
      } finally {
        setIsLoading(false);
      }
    })();
  }, []);

  useEffect(() => {
    calculateTotal();
  }, [
    totalQuantity,
    envio,
    modalAbierto,
    costoEnvioDomicilio,
    habilitadoEnvioDomicilio,
  ]);

  useEffect(() => {
    // Mostrar u ocultar el botón de WhatsApp en función de totalQuantity (Si es mayor o igual a 1 se mostrara, por lo contrario se escondera)
    if (totalQuantity >= 1 && modalAbierto === false) {
      setShowButton(true);
    } else {
      setShowButton(false);
    }
  }, [totalQuantity, modalAbierto]);

  useEffect(() => {
    const fetchData = async () => {
      const isMayorista = pathname.includes("mayorista");
      const isMinorista = pathname.includes("minorista");

      if (isMayorista) {
        setClientType("Mayorista");
        await GetCategoriesMayorista(setCategories);
      } else if (isMinorista) {
        setClientType("Minorista");
        await GetCategoriesMinorista(setCategories);
      }

      const storedCartKey = isMayorista
        ? "shoppingCartMayorista"
        : "shoppingCartMinorista";
      const storedCart = localStorage.getItem(storedCartKey);

      const urlParams = new URLSearchParams(window.location.search);
      const paymentId = urlParams.get("payment_id");
      const collectionStatus = urlParams.get("collection_status");

      if (!paymentId) {
        const storedFormData = localStorage.getItem("userData");

        if (storedFormData && storedFormData !== "{}") {
          const parsedFormData = JSON.parse(storedFormData);

          setNombre(parsedFormData.nombre || "");
          setDni(parsedFormData.dni || "");
          setDireccion(parsedFormData.direccion || "");
          setCalles(parsedFormData.calles || "");
          setTelefono(parsedFormData.telefono || "");
          setAbono(parsedFormData.abono || "");
          setVendedor(parsedFormData.vendedor || "");
          setEnvio(parsedFormData.envio || "");
        }

        if (storedCart && storedCart !== "{}") {
          const parsedCart = JSON.parse(storedCart);
          setCart(parsedCart);

          const productQuantities = Object.values(parsedCart).reduce(
            (quantities, product) => {
              quantities[product.idProducto] = product.cantidad;
              return quantities;
            },
            {}
          );

          setProductQuantities(productQuantities);

          const totalQuantitySum = Object.values(productQuantities).reduce(
            (sum, quantity) => sum + quantity,
            0
          );
          setTotalQuantity(totalQuantitySum);

          setModalAbierto(true);

          Swal.fire({
            title: "Carrito encontrado!",
            text: "¿Quieres continuar comprando o vaciar el carrito?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Continuar Comprando",
            cancelButtonText: "Vaciar Carrito",
            allowOutsideClick: false,
            confirmButtonColor: "#87adbd",
            cancelButtonColor: "#dc3545",
          }).then((result) => {
            if (result.isConfirmed) {
              setModalAbierto(false);
              // No hacer nada, continuar comprando
            } else if (result.isDismissed) {
              Swal.fire({
                title: "¿Estás seguro de vaciar el carrito?",
                text: "Al hacer esto, no podrás recuperar los productos de este carrito.",
                icon: "warning",
                showCancelButton: true,
                allowOutsideClick: false,
                confirmButtonText: "Sí, vaciar carrito",
                cancelButtonText: "Cancelar",
                confirmButtonColor: "#f8bb86",
              }).then((confirmationResult) => {
                if (confirmationResult.isConfirmed) {
                  clearCart();
                  setModalAbierto(false);
                }
              });
            }
          });
        }
      } else if (paymentId === "null" || collectionStatus === "rejected") {
        setModalAbierto(true);
        Swal.fire({
          title: "Pago rechazado",
          text: "inténtelo de nuevo o cambie el método de pago.",
          icon: "error",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#dc3545",
          allowOutsideClick: false,
        }).then((confirmationResult) => {
          if (confirmationResult.isConfirmed) {
            setModalAbierto(false);
          }
        });

        const storedFormData = localStorage.getItem("userData");

        if (storedFormData) {
          const parsedFormData = JSON.parse(storedFormData);

          setNombre(parsedFormData.nombre || "");
          setDni(parsedFormData.dni || "");
          setDireccion(parsedFormData.direccion || "");
          setCalles(parsedFormData.calles || "");
          setTelefono(parsedFormData.telefono || "");
          setAbono(parsedFormData.abono || "");
          setVendedor(parsedFormData.vendedor || "");
          setEnvio(parsedFormData.envio || "");
        }

        if (storedCart && storedCart !== "{}") {
          const parsedCart = JSON.parse(storedCart);
          setCart(parsedCart);

          const productQuantities = Object.values(parsedCart).reduce(
            (quantities, product) => {
              quantities[product.idProducto] = product.cantidad;
              return quantities;
            },
            {}
          );

          setProductQuantities(productQuantities);

          const totalQuantitySum = Object.values(productQuantities).reduce(
            (sum, quantity) => sum + quantity,
            0
          );
          setTotalQuantity(totalQuantitySum);
        }
      } else if (paymentId) {
        await GetUsersSellers(setListaNombresVendedores);

        const storedFormData = localStorage.getItem("userData");

        if (storedFormData) {
          const parsedFormData = JSON.parse(storedFormData);

          setNombre(parsedFormData.nombre || "");
          setDni(parsedFormData.dni || "");
          setDireccion(parsedFormData.direccion || "");
          setCalles(parsedFormData.calles || "");
          setTelefono(parsedFormData.telefono || "");
          setAbono(parsedFormData.abono || "");
          setVendedor(parsedFormData.vendedor || "");
          setEnvio(parsedFormData.envio || "");
        }

        if (storedCart && storedCart !== "{}") {
          const parsedCart = JSON.parse(storedCart);
          setCart(parsedCart);

          const productQuantities = Object.values(parsedCart).reduce(
            (quantities, product) => {
              quantities[product.idProducto] = product.cantidad;
              return quantities;
            },
            {}
          );

          setProductQuantities(productQuantities);
        }

        GetPaymentInfo(paymentId)
          .then((PaymentResponse) => {
            if (PaymentResponse.status === "approved") {
              setPedidoAprobado(true);
            }
          })
          .catch((error) => {
            console.error("Error al obtener la información de pago:", error);
          });
      }
    };

    fetchData();
    // The dependency array is kept minimal to avoid multiple triggers
    // It's assumed that pathname will change when clientType changes, thus avoiding direct dependency on clientType
  }, [pathname, pedidoAprobado]);

  useEffect(() => {
    if (pedidoAprobado) {
      Swal.fire({
        title: "Pago aprobado",
        text: "Su pago ha sido procesado correctamente.",
        icon: "success",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#a5dc86",
        allowOutsideClick: false,
      });
      ClickBtnHandlePedidoAprobado();
    }
  }, [pedidoAprobado]);
  //#endregion

  //#region Función para filtrar los productos por query
  const search = async () => {
    if (searchValue === "") {
      Swal.fire({
        icon: "warning",
        title: "Consulta vacía",
        text: "Ingrese un término de búsqueda.",
        confirmButtonText: "Aceptar",
        showCancelButton: false,
        confirmButtonColor: "#f8bb86",
      });
    } else if (searchValue === query) {
      Swal.fire({
        icon: "warning",
        title: "Término de búsqueda no cambiado",
        text: "El término de búsqueda es el mismo que la última consulta. Intente con un término diferente.",
        confirmButtonText: "Aceptar",
        showCancelButton: false,
        confirmButtonColor: "#f8bb86",
      });
    } else {
      setQuery(searchValue);
      try {
        setIsLoadingQuery(true);
        const products = await GetProductsByQuery(searchValue);

        setProducts(products);
        window.scrollTo(0, 0);
      } catch (error) {
        console.log(error);
      } finally {
        setIsLoadingQuery(false);
      }

      if (searchValue === "") {
        window.scrollTo(0, 0);
      }
    }
  };
  //#endregion

  //#region Función para abrir el "+" o "-" de cada categoria
  const handleCategoryClick = async (index) => {
    setCategorySign((prevSigns) => ({
      ...prevSigns,
      [index]: prevSigns[index] === "-" ? "+" : "-",
    }));

    if (categorySign[index] === "-") {
      setIsLoadingProductByCategory(false);
      return; // No hacer la petición de productos
    }

    const category = categories[index].nombre;
    let products;

    try {
      products = await GetProductsByCategory(category);
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoadingProductByCategory(false);
    }

    setCategoryProducts((prevProducts) => ({
      ...prevProducts,
      [category]: products,
    }));
  };
  //#endregion

  //#region Función para agregar productos al carrito
  const handleAdd = (product) => {
    const availableStock = product.stockTransitorio;
    const currentQuantity = productQuantities[product.idProducto] || 0;

    if (currentQuantity < availableStock) {
      setProductQuantities((prevQuantities) => ({
        ...prevQuantities,
        [product.idProducto]: (prevQuantities[product.idProducto] || 0) + 1,
      }));

      setCart((prevCart) => {
        const updatedCart = { ...prevCart };
        const quantity = updatedCart[product.idProducto]?.cantidad || 0;
        const aclaraciones =
          updatedCart[product.idProducto]?.aclaraciones || "";
        updatedCart[product.idProducto] = {
          ...product,
          cantidad: quantity + 1,
          aclaraciones,
        };

        // Actualizar el localStorage con el nuevo carrito
        updateLocalStorage(updatedCart);

        return updatedCart;
      });

      setProductNotes((prevNotes) => ({
        ...prevNotes,
        [product.idProducto]: prevNotes[product.idProducto] || "",
      }));

      setTotalQuantity((prevQuantity) => prevQuantity + 1);
    } else {
      Swal.fire({
        icon: "warning",
        title: "No hay más unidades de stock para agregar",
        text: `Lo sentimos, solo hay ${availableStock} unidades disponibles en este momento.`,
        confirmButtonText: "Aceptar",
        showCancelButton: false,
        confirmButtonColor: "#f8bb86",
      });
    }
  };
  //#endregion

  //#region Función para actualizar la cantidad total de productos
  const updateTotalQuantity = () => {
    let total = 0;
    for (const productId in productQuantities) {
      total += productQuantities[productId];
    }
    setTotalQuantity(total);
  };
  //#endregion

  //#region Función para quitar productos del carrito
  const handleSubtract = (product) => {
    const quantity = productQuantities[product.idProducto] || 0;

    if (quantity > 0) {
      setProductQuantities((prevQuantities) => ({
        ...prevQuantities,
        [product.idProducto]: Math.max(0, quantity - 1),
      }));

      setCart((prevCart) => {
        const updatedCart = { ...prevCart };
        const updatedQuantity = updatedCart[product.idProducto]?.cantidad || 0;

        if (updatedQuantity > 1) {
          updatedCart[product.idProducto] = {
            ...product,
            cantidad: updatedQuantity - 1,
            aclaraciones: prevCart[product.idProducto]?.aclaraciones || "",
          };
        } else {
          delete updatedCart[product.idProducto];
          delete productNotes[product.idProducto];
        }

        // Actualizar el localStorage con el nuevo carrito
        updateLocalStorage(updatedCart);

        return updatedCart;
      });

      setTotalQuantity((prevQuantity) => prevQuantity - 1);
    }
  };
  //#endregion

  //#region Función para manejar la cantidad de productos ingresados por input
  const handleQuantityChange = (event, product) => {
    const value = parseInt(event.target.value);
    const maxStock = product.stockTransitorio;

    // Verificar que la cantidad no supere el stock transitorio
    if (value > maxStock) {
      Swal.fire({
        icon: "warning",
        title: "La cantidad de unidades ingresada no está disponible",
        text: `Lo sentimos, solo hay ${maxStock} unidades disponibles en este momento.`,
        confirmButtonText: "Aceptar",
        showCancelButton: false,
        confirmButtonColor: "#f8bb86",
      });
      // Restaurar la cantidad a la máxima permitida
      event.target.value = maxStock;
      return; // Salir de la función para evitar continuar con la actualización
    }

    setProductQuantities((prevQuantities) => ({
      ...prevQuantities,
      [product.idProducto]: isNaN(value) ? 0 : value,
    }));

    setCart((prevCart) => {
      const updatedCart = { ...prevCart };
      const updatedQuantity = isNaN(value) ? 0 : value;

      if (updatedQuantity > 0) {
        updatedCart[product.idProducto] = {
          ...product,
          cantidad: updatedQuantity,
          aclaraciones: prevCart[product.idProducto]?.aclaraciones || "",
        };
      } else {
        delete updatedCart[product.idProducto];
        delete productNotes[product.idProducto];
      }

      // Calcula la cantidad total sumando las cantidades de todos los productos en el carrito
      let total = 0;
      for (const productId in updatedCart) {
        total += updatedCart[productId].cantidad;
      }
      setTotalQuantity(total);

      // Actualizar el localStorage con el nuevo carrito
      updateLocalStorage(updatedCart);

      return updatedCart;
    });

    setProductNotes((prevNotes) => ({
      ...prevNotes,
      [product.idProducto]: prevNotes[product.idProducto] || "",
    }));
  };
  //#endregion

  //#region Función para manejar las aclaraciones de cada producto
  const handleAclaracionesChange = (event, product) => {
    const value = event.target.value;

    setCart((prevCart) => {
      const updatedCart = { ...prevCart };
      updatedCart[product.idProducto] = {
        ...product,
        cantidad: prevCart[product.idProducto]?.cantidad || 0,
        aclaraciones: value,
      };

      // Actualizar el localStorage con el nuevo carrito
      updateLocalStorage(updatedCart);

      return updatedCart;
    });

    setProductNotes((prevNotes) => ({
      ...prevNotes,
      [product.idProducto]: value,
    }));
  };
  //#endregion

  //#region Función para calcular los subtotales de cada producto
  const calculateSubtotal = (product) => {
    const quantity = productQuantities[product.idProducto] || 0;

    if (
      (clientType === "Mayorista"
        ? product.precioMayorista
        : product.precioMinorista) > 0
    ) {
      // Si se estableció un precio manual, usar ese precio directamente sin aplicar porcentaje ni valor del dólar
      return Math.round(
        (clientType === "Mayorista"
          ? product.precioMayorista
          : product.precioMinorista) * quantity
      );
    } else if (product.divisa === "Dólar") {
      // Si la divisa es Dólar, realiza el cálculo multiplicando por el valor del dólar
      return (
        Math.ceil(
          Math.round(
            product.precio *
              valorDolar *
              (1 +
                (clientType === "Mayorista"
                  ? product.porcentajeMayorista
                  : product.porcentajeMinorista) /
                  100) *
              quantity
          ) / 50
        ) * 50
      );
    } else if (product.divisa === "Peso") {
      // Si la divisa es Peso, realiza el cálculo sin multiplicar por el valor del dólar
      return (
        Math.ceil(
          Math.round(
            product.precio *
              (1 +
                (clientType === "Mayorista"
                  ? product.porcentajeMayorista
                  : product.porcentajeMinorista) /
                  100) *
              quantity
          ) / 50
        ) * 50
      );
    } else {
      return 0;
    }
  };
  //#endregion

  //#region Función para calcular el monto total del pedido
  const calculateTotal = () => {
    let total = 0;
    for (const productId in cart) {
      const product = cart[productId];
      total += calculateSubtotal(product);
    }

    // Agregar el costo de envío si corresponde
    if (envio == 2 && costoEnvioDomicilio > 0) {
      total += costoEnvioDomicilio;
    }

    setTotal(total);
  };
  //#endregion

  //#region Función para los datos del cliente del formulario
  const ClearFormData = () => {
    localStorage.removeItem("userData");
  };
  //#endregion

  //#region Función para borrar el carrito entero
  const clearCart = () => {
    setCart({});
    // Actualizar el localStorage con el nuevo carrito
    // updateLocalStorage({});
    localStorage.removeItem(
      clientType === "Mayorista"
        ? "shoppingCartMayorista"
        : "shoppingCartMinorista"
    );
    setProductQuantities({});
    setProductNotes({});
    setTotalQuantity(0);
  };
  //#endregion

  //#region Función para eliminar productos del carrito
  const handleDelete = (product) => {
    setTotalQuantity((prevQuantity) => prevQuantity - product.cantidad);

    setCart((prevCart) => {
      const updatedCart = { ...prevCart };
      delete updatedCart[product.idProducto];
      delete productQuantities[product.idProducto];
      delete productNotes[product.idProducto];

      // Actualizar el localStorage con el nuevo carrito
      updateLocalStorage(updatedCart);

      return updatedCart;
    });
  };
  //#endregion

  //#region Función para guardar carrito en el local storage
  const updateLocalStorage = (cart) => {
    localStorage.setItem(
      clientType === "Mayorista"
        ? "shoppingCartMayorista"
        : "shoppingCartMinorista",
      JSON.stringify(cart)
    );
  };
  //#endregion

  //#region Función para crear el pedido y luego enviarlo por Mercado Pago
  const handleSubmitPedidoMercadoPago = async (e) => {
    e.preventDefault();

    if (IsValid() === true) {
      try {
        e.preventDefault();

        const response = await PayWithMercadoPago({
          title: "Productos",
          quantity: 1,
          unitPrice: total,
          url: `http://localhost:3000/catalogo-${
            clientType === "Mayorista" ? "mayorista" : "minorista"
          }`,
        });

        if (response.preferenceId !== null) {
          setPreferenceId(response.preferenceId);
          setShowWallet(true);

          // Create an object with the values you want to store
          const userData = {
            nombre,
            dni,
            direccion,
            calles,
            telefono,
            abono,
            vendedor,
            envio,
          };

          // Convert the object to a JSON string and store it in localStorage
          localStorage.setItem("userData", JSON.stringify(userData));
        }
      } catch (error) {
        console.error("Error durante PayWithMercadoPago:", error);
        // Manejar el error apropiadamente (e.g., mostrar un mensaje al usuario)
      }
    }
  };
  //#endregion

  function ClickBtnHandlePedidoAprobado() {
    $(document).ready(function () {
      $("#btn-handlepedido").click();
    });
  }

  const handleSubmitPedidoAprobado = async () => {
    const detalles = Object.values(cart).map((producto) => {
      return {
        idProducto: producto.idProducto,
        cantidad: productQuantities[producto.idProducto],
        aclaracion: producto.aclaraciones,
        precioUnitario:
          clientType === "Mayorista" && producto.precioMayorista > 0
            ? `${Math.ceil(producto.precioMayorista)}`
            : clientType === "Minorista" && producto.precioMinorista > 0
            ? `${Math.ceil(producto.precioMinorista)}`
            : // producto.precioMayorista > 0
              // ? `${Math.ceil(producto.precioMayorista)}`
              `${Math.ceil(
                Math.round(
                  ((producto.divisa === "Dólar"
                    ? producto.precio * valorDolar
                    : producto.precio) *
                    (1 +
                      (clientType === "Mayorista"
                        ? producto.porcentajeMayorista
                        : producto.porcentajeMinorista) /
                        100)) /
                    50
                ) * 50
              )}`,
      };
    });

    const response = await SaveOrders({
      nombreCompleto: `${nombre.charAt(0).toUpperCase() + nombre.slice(1)}`,
      dni: dni,
      telefono: telefono,
      direccion: `${direccion.charAt(0).toUpperCase() + direccion.slice(1)}`,
      entreCalles: `${calles.charAt(0).toUpperCase() + calles.slice(1)}`,
      costoEnvio:
        envio == 2 && costoEnvioDomicilio > 0 ? costoEnvioDomicilio : 0,
      idTipoPedido: clientType === "Minorista" ? 1 : 2,
      idVendedor: vendedor,
      idMetodoPago: abono,
      idMetodoEntrega: envio,
      detalles: detalles, // Aquí se incluyen los detalles de los productos
    });

    if (response.data.statusCode == 200) {
      // Crear el mensaje con la información del pedido para Whatsapp
      let mensaje = "```Datos del cliente:```\n\n";

      mensaje += `*Nombre completo*:\n_${nombre}_\n\n`;

      mensaje += `*DNI*:\n_${dni}_\n\n`;

      mensaje += `*Entrega*:\n_${getTipoEnvio(envio)}_\n\n`;

      if (envio != 1) {
        mensaje += `*Dirección*:\n_${direccion}_\n\n`;
        mensaje += `*Entre calles*:\n_${calles}_\n\n`;
      }

      mensaje += `*Número de teléfono*:\n_${telefono}_\n\n`;

      mensaje += `*Abona con*:\n_${getTipoAbono(abono)}_\n\n`;

      mensaje += `*----------------------------------*\n\n`;
      mensaje += "```Datos de la empresa:```\n\n";

      if (envio == 1) {
        mensaje += `*Dirección*:\n_${direccionAuto}_\n\n`;
        mensaje += `*Horarios de atención:*\n_${horariosAtencion}_\n\n`;
      }

      mensaje += `*Número de teléfono*:\n_${telefonoEmpresa}_\n\n`;

      if (abono == 2) {
        mensaje += `*CBU*:\n_${cbu}_\n\n`;
      }

      mensaje += `*Vendedor*:\n_${getNombreVendedor(vendedor)}_\n\n`;

      mensaje += `*----------------------------------*\n\n`;

      mensaje +=
        clientType === "Mayorista"
          ? "```Pedido Mayorista:```\n\n"
          : "```Pedido Minorista:```\n\n";

      mensaje += `*Número de pedido*: ${response.data.idPedido}\n\n`;
      for (const productId in cart) {
        const product = cart[productId];
        const quantity = productQuantities[product.idProducto];
        mensaje += `*${quantity}* x *${product.nombre}*\n`;

        if (productNotes[product.idProducto]) {
          mensaje += `*Aclaración: ${productNotes[product.idProducto]}*\n`;
        }

        mensaje += `_Subtotal = $${calculateSubtotal(product)
          .toLocaleString("es-ES", {
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
          })
          .replace(",", ".")
          .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}_\n\n`;
      }

      if (envio == 2 && costoEnvioDomicilio > 0) {
        mensaje += `*SUBTOTAL: $${(total - costoEnvioDomicilio)
          .toLocaleString("es-ES", {
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
          })
          .replace(",", ".")
          .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}*\n`;
        mensaje += `*Costo de envío: $${costoEnvioDomicilio}*\n`;
      }
      mensaje += `*TOTAL: $${total
        .toLocaleString("es-ES", {
          minimumFractionDigits: 0,
          maximumFractionDigits: 2,
        })
        .replace(",", ".")
        .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}*`;

      // El POST se realizó exitosamente, ahora redirigir a WhatsApp

      // Crear el enlace para abrir WhatsApp con el mensaje
      const encodedMensaje = encodeURIComponent(mensaje);
      const whatsappURL = `https://api.whatsapp.com/send?phone=543517476389&text=${encodedMensaje}`;

      // Redirigir directamente a la URL de WhatsApp
      window.location.href = whatsappURL;

      // Restablecer el formulario y ocultarlo
      ClearClientInputs();
      CloseModal();

      clearCart();
      ClearFormData();
    }
  };

  //#region Función para crear el pedido y luego enviarlo por Whatsapp
  const handleSubmitPedido = async (e) => {
    e.preventDefault();

    if (IsValid() === true) {
      // Iterar sobre los elementos del carrito y crear los detalles
      const detalles = Object.values(cart).map((producto) => {
        return {
          idProducto: producto.idProducto,
          cantidad: productQuantities[producto.idProducto],
          aclaracion: producto.aclaraciones,
          precioUnitario:
            clientType === "Mayorista" && producto.precioMayorista > 0
              ? `${Math.ceil(producto.precioMayorista)}`
              : clientType === "Minorista" && producto.precioMinorista > 0
              ? `${Math.ceil(producto.precioMinorista)}`
              : // producto.precioMayorista > 0
                // ? `${Math.ceil(producto.precioMayorista)}`
                `${Math.ceil(
                  Math.round(
                    ((producto.divisa === "Dólar"
                      ? producto.precio * valorDolar
                      : producto.precio) *
                      (1 +
                        (clientType === "Mayorista"
                          ? producto.porcentajeMayorista
                          : producto.porcentajeMinorista) /
                          100)) /
                      50
                  ) * 50
                )}`,
        };
      });

      await SaveOrders({
        nombreCompleto: `${nombre.charAt(0).toUpperCase() + nombre.slice(1)}`,
        dni: dni,
        telefono: telefono,
        direccion: `${direccion.charAt(0).toUpperCase() + direccion.slice(1)}`,
        entreCalles: `${calles.charAt(0).toUpperCase() + calles.slice(1)}`,
        costoEnvio:
          envio == 2 && costoEnvioDomicilio > 0 ? costoEnvioDomicilio : 0,
        idTipoPedido: clientType === "Minorista" ? 1 : 2,
        idVendedor: vendedor,
        idMetodoPago: abono,
        idMetodoEntrega: envio,
        detalles: detalles, // Aquí se incluyen los detalles de los productos
      })
        .then((response) => {
          // Crear el mensaje con la información del pedido para Whatsapp
          let mensaje = "```Datos del cliente:```\n\n";

          mensaje += `*Nombre completo*:\n_${nombre}_\n\n`;

          mensaje += `*DNI*:\n_${dni}_\n\n`;

          mensaje += `*Entrega*:\n_${getTipoEnvio(envio)}_\n\n`;

          if (envio != 1) {
            mensaje += `*Dirección*:\n_${direccion}_\n\n`;
            mensaje += `*Entre calles*:\n_${calles}_\n\n`;
          }

          mensaje += `*Número de teléfono*:\n_${telefono}_\n\n`;

          mensaje += `*Abona con*:\n_${getTipoAbono(abono)}_\n\n`;

          mensaje += `*----------------------------------*\n\n`;
          mensaje += "```Datos de la empresa:```\n\n";

          if (envio == 1) {
            mensaje += `*Dirección*:\n_${direccionAuto}_\n\n`;
            mensaje += `*Horarios de atención:*\n_${horariosAtencion}_\n\n`;
          }

          mensaje += `*Número de teléfono*:\n_${telefonoEmpresa}_\n\n`;

          if (abono == 2) {
            mensaje += `*CBU*:\n_${cbu}_\n\n`;
          }

          mensaje += `*Vendedor*:\n_${getNombreVendedor(vendedor)}_\n\n`;

          mensaje += `*----------------------------------*\n\n`;

          mensaje +=
            clientType === "Mayorista"
              ? "```Pedido Mayorista:```\n\n"
              : "```Pedido Minorista:```\n\n";

          mensaje += `*Número de pedido*: ${response.data.idPedido}\n\n`;
          for (const productId in cart) {
            const product = cart[productId];
            const quantity = productQuantities[product.idProducto];
            mensaje += `*${quantity}* x *${product.nombre}*\n`;

            if (productNotes[product.idProducto]) {
              mensaje += `*Aclaración: ${productNotes[product.idProducto]}*\n`;
            }

            mensaje += `_Subtotal = $${calculateSubtotal(product)
              .toLocaleString("es-ES", {
                minimumFractionDigits: 0,
                maximumFractionDigits: 2,
              })
              .replace(",", ".")
              .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}_\n\n`;
          }

          if (envio == 2 && costoEnvioDomicilio > 0) {
            mensaje += `*SUBTOTAL: $${(total - costoEnvioDomicilio)
              .toLocaleString("es-ES", {
                minimumFractionDigits: 0,
                maximumFractionDigits: 2,
              })
              .replace(",", ".")
              .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}*\n`;
            mensaje += `*Costo de envío: $${costoEnvioDomicilio}*\n`;
          }
          mensaje += `*TOTAL: $${total
            .toLocaleString("es-ES", {
              minimumFractionDigits: 0,
              maximumFractionDigits: 2,
            })
            .replace(",", ".")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}*`;

          // El POST se realizó exitosamente, ahora redirigir a WhatsApp

          // Crear el enlace para abrir WhatsApp con el mensaje
          const encodedMensaje = encodeURIComponent(mensaje);
          const whatsappURL = `https://api.whatsapp.com/send?phone=543517476389&text=${encodedMensaje}`;

          // Redirigir directamente a la URL de WhatsApp
          window.location.href = whatsappURL;

          // Restablecer el formulario y ocultarlo
          ClearClientInputs();
          CloseModal();

          clearCart();
          ClearFormData();
        })
        .catch((error) => {
          // Manejar el error si algo sale mal en el POST
          console.error("Error al guardar el pedido:", error);
        });
    }
  };
  //#endregion

  //#region Función para limpiar todos los valores de los inputs del formulario
  function ClearClientInputs() {
    setNombre("");
    setDni("");
    setDireccion("");
    setCalles("");
    setTelefono("");
    setAbono("");
    setVendedor("");
    setEnvio("");
  }
  //#endregion

  //#region Función para verificar si el valor de algun input del cliente esta vacio
  function IsEmpty() {
    if (nombre !== "") {
      return false;
    } else if (dni !== "") {
      return false;
    } else if (telefono !== "") {
      return false;
    } else if (direccion !== "") {
      return false;
    } else if (calles !== "") {
      return false;
    } else if (abono !== "") {
      return false;
    } else if (vendedor !== "") {
      return false;
    } else if (envio !== "") {
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si los valores ingresados a traves de los input son correctos
  function IsValid() {
    if (nombre === "") {
      Swal.fire({
        icon: "error",
        title: "Debe ingresar su nombre completo",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#nombre").focus();
        }, 500);
      });
      return false;
    } else if (dni === "") {
      Swal.fire({
        icon: "error",
        title: "Debe ingresar su número de documento",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#dni").focus();
        }, 500);
      });
      return false;
    } else if (telefono === "") {
      Swal.fire({
        icon: "error",
        title: "Debe indicar su número de teléfono",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#telefono").focus();
        }, 500);
      });
      return false;
    } else if (envio === "") {
      Swal.fire({
        icon: "error",
        title:
          "Debe indicar si quiere que se lo enviemos o si lo retira por el local",
        text: "Seleccione una opción",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      return false;
    } else if (direccion === "" && envio != 1) {
      Swal.fire({
        icon: "error",
        title: "Debe indicar su dirección",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#direccion").focus();
        }, 500);
      });
      return false;
    } else if (calles === "" && envio != 1) {
      Swal.fire({
        icon: "error",
        title: "Debe indicar entre que calles se encuentra la dirección",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#calles").focus();
        }, 500);
      });
      return false;
    } else if (abono === "") {
      Swal.fire({
        icon: "error",
        title: "Debe indicar de que forma abonara el pedido",
        text: "Seleccione una opción",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      return false;
    } else if (vendedor === "") {
      Swal.fire({
        icon: "error",
        title: "Debe indicar el vendedor",
        text: "Seleccione un vendedor",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para cerrar el modal manualmente mediante el codigo
  function CloseModal() {
    $(document).ready(function () {
      $("#btn-close-modal").click();
    });
  }
  //#endregion

  //#region Función para borrar la busqueda por query
  const handleClearSearch = () => {
    // Función para limpiar la búsqueda (setear query a vacío)
    setQuery("");
    setSearchValue("");
  };
  //#endregion

  //#region Función para borrar que se eliga primero el tipo de entrega antes del abono
  const handleAbonoClick = () => {
    if (envio === "" && costoEnvioDomicilio > 0) {
      Swal.fire({
        icon: "warning",
        title:
          "Primero debe indicar si quiere que se lo enviemos o si lo retira por el local",
        text: "Seleccione una opción",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f8bb86",
      });
    }
  };
  //#endregion

  //#region Función para obtener el tipo de envío
  const getTipoEnvio = (id) => {
    switch (id) {
      case "1":
        return "Lo retiro por el local ($0)";
      case "2":
        return `Envío a domicilio (${costoEnvioDomicilio})`;
      default:
        return "Tipo de envio no especificado";
    }
  };
  //#endregion

  //#region Función para obtener el tipo de abono
  const getTipoAbono = (id) => {
    switch (id) {
      case "1":
        return "Efectivo";
      case "2":
        return "Transferencia";
      case "3":
        return "Tarjeta de débito";
      case "4":
        return "Tarjeta de crédito";
      case "5":
        return "Mercado Pago";
      default:
        return "Tipo de abono no especificado";
    }
  };
  //#endregion

  //#region Función para obtener el nombre del vendedor
  const getNombreVendedor = (idVendedor) => {
    const vendedor = listaNombresVendedores.find(
      (v) => v.idUsuario == idVendedor
    );
    return vendedor ? vendedor.nombre : "Nombre de vendedor no encontrado";
  };
  //#endregion

  //#region Return
  return (
    <div>
      <Helmet>
        <title>
          Catapp | Catálogo{" "}
          {clientType === "Mayorista" ? "Mayorista" : "Minorista"}
        </title>
      </Helmet>

      <div className={color ? "header-nav2 header-nav-bg2" : "header-nav2"}>
        <div className={click ? "header-menu2 active" : "header-menu2"}>
          <ul
            className={
              color
                ? "header-menu2-container header-menu2-container-bg"
                : "header-menu2-container"
            }
          >
            <div className="cart-tit-del">
              <h1 className="title carrito-title">Carrito</h1>
              {Object.values(cart).some((product) => product.cantidad > 0) && (
                <button
                  type="button"
                  className="btn btn-danger btn-delete btn-carrito"
                  onClick={() =>
                    Swal.fire({
                      title: "¿Está seguro de que desea vaciar el carrito?",
                      text: "Una vez vaciado, no se podrá recuperar",
                      icon: "warning",
                      showCancelButton: true,
                      confirmButtonColor: "#F8BB86",
                      cancelButtonColor: "#6c757d",
                      confirmButtonText: "Aceptar",
                      cancelButtonText: "Cancelar",
                      focusCancel: true,
                    }).then((result) => {
                      if (result.isConfirmed) {
                        clearCart();
                      }
                    })
                  }
                >
                  <Delete className="delete" />
                </button>
              )}
            </div>

            <button
              type="button"
              className="btn-close-modal"
              id="btn-handlepedido"
              onClick={handleSubmitPedidoAprobado}
            ></button>

            {/* Renderizar los productos agregados al carrito */}
            {Object.values(cart).map((product) => {
              if (product.cantidad > 0) {
                return (
                  <div className="home-5" key={product.idProducto}>
                    <div
                      className="contenedor-producto"
                      key={product.idProducto}
                    >
                      <div className="product">
                        <div className="product-1-col">
                          <figure className="figure">
                            <div className="zoom-container">
                              <Zoom
                                className="zoom"
                                onClick={() =>
                                  Swal.fire({
                                    title: product.nombre,
                                    imageUrl: `${product.urlImagen}`,
                                    imageWidth: 400,
                                    imageHeight: 400,
                                    imageAlt: "Vista Producto",
                                    confirmButtonColor: "#6c757d",
                                    confirmButtonText: "Cerrar",
                                    focusConfirm: true,
                                  })
                                }
                              ></Zoom>
                            </div>
                            <img
                              className="product-img"
                              src={product.urlImagen}
                              alt="Producto"
                            />
                          </figure>
                        </div>

                        <div className="product-2-col">
                          <h3 className="product-title">{product.nombre}</h3>
                          <h3 className="product-desc">
                            <pre className="pre">{product.descripcion}</pre>
                          </h3>
                        </div>

                        <div className="product-3-col2">
                          <p className="product-price">
                            {(clientType === "Minorista"
                              ? product.precioMinorista
                              : product.precioMayorista) > 0
                              ? `$${Math.ceil(
                                  clientType === "Minorista"
                                    ? product.precioMinorista
                                    : product.precioMayorista
                                )
                                  .toLocaleString("es-ES", {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 2,
                                  })
                                  .replace(",", ".")
                                  .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}`
                              : `$${Math.ceil(
                                  Math.round(
                                    ((product.divisa === "Dólar"
                                      ? product.precio * valorDolar
                                      : product.precio) *
                                      (1 +
                                        (clientType === "Minorista"
                                          ? product.porcentajeMinorista
                                          : product.porcentajeMayorista) /
                                          100)) /
                                      50
                                  ) * 50
                                )
                                  .toLocaleString("es-ES", {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 2,
                                  })
                                  .replace(",", ".")
                                  .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}`}
                          </p>
                          <button
                            type="button"
                            className="btn btn-danger btn-delete2 btn-carrito"
                            onClick={() =>
                              Swal.fire({
                                title: `¿Está seguro de que quitar todas las unidades de ${product.nombre}?`,
                                text: "Una vez quitado, no se podrá recuperar",
                                icon: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#F8BB86",
                                cancelButtonColor: "#6c757d",
                                confirmButtonText: "Aceptar",
                                cancelButtonText: "Cancelar",
                                focusCancel: true,
                              }).then((result) => {
                                if (result.isConfirmed) {
                                  handleDelete(product);
                                }
                              })
                            }
                          >
                            <Delete className="delete2" />
                          </button>
                        </div>
                      </div>

                      <div className="product-2">
                        <div className="product-subtotal2">
                          <p className="product-desc">
                            Cantidad:{" "}
                            <b className="product-price">
                              {productQuantities[product.idProducto]}
                            </b>
                          </p>
                          <p className="product-desc">
                            Subtotal:{" "}
                            <b className="product-price">
                              $
                              {calculateSubtotal(product)
                                .toLocaleString("es-ES", {
                                  minimumFractionDigits: 0,
                                  maximumFractionDigits: 2,
                                })
                                .replace(",", ".")
                                .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                            </b>
                          </p>
                        </div>

                        <div className="product-quantity">
                          <button
                            className="quantity-btn btnminus"
                            onClick={() => handleSubtract(product)}
                          >
                            -
                          </button>
                          <input
                            type="number"
                            min="0"
                            value={productQuantities[product.idProducto]}
                            onChange={(event) =>
                              handleQuantityChange(event, product)
                            }
                            onBlur={() => updateTotalQuantity()}
                            className="quantity-input"
                          />
                          <button
                            className="quantity-btn btnplus"
                            onClick={() => handleAdd(product)}
                          >
                            +
                          </button>
                        </div>
                      </div>

                      <div className="product-notes">
                        <textarea
                          className="textarea"
                          placeholder="Agregar aclaraciones..."
                          value={product.aclaraciones}
                          onChange={(event) =>
                            handleAclaracionesChange(event, product)
                          }
                        ></textarea>
                      </div>
                    </div>
                  </div>
                );
              } else {
                return null;
              }
            })}

            {/* Mostrar el total solo si hay productos en el carrito */}
            {Object.values(cart).some((product) => product.cantidad > 0) ? (
              <div className="home-6">
                <div className="totales-wpp-container">
                  <div className="totales">
                    <p className="product-desc cant-total">
                      Cantidad total de productos:{" "}
                      <b className="product-price">{totalQuantity}</b>
                    </p>
                    <p className="product-desc">
                      Total:{" "}
                      <b className="product-price">
                        $
                        {total
                          .toLocaleString("es-ES", {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 2,
                          })
                          .replace(",", ".")
                          .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                      </b>
                    </p>
                  </div>

                  <div className="wpp-btn-container">
                    <button
                      type="button"
                      className="btn-wpp-cart"
                      //  data-bs-toggle="modal"
                      //  data-bs-target="#exampleModal"
                      onClick={() => {
                        // Agregar verificación antes de abrir el modal
                        if (clientType === "Mayorista" && totalQuantity < 5) {
                          const faltanProductos = 5 - totalQuantity;

                          Swal.fire({
                            icon: "warning",
                            title: `Falta${
                              faltanProductos !== 1 ? "n" : ""
                            } ${faltanProductos} producto${
                              faltanProductos !== 1 ? "s" : ""
                            } para que puedas hacer tu pedido mayorista`,
                            text: "Debes agregar al menos 5 productos al carrito para enviar el pedido.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: false,
                            confirmButtonColor: "#f8bb86",
                          });

                          return; // Detener el proceso si no hay suficientes productos
                        }

                        // Lógica adicional después de la verificación (si es necesario)
                        // ClearClientInputs();
                        setTimeout(function () {
                          $("#nombre").focus();
                        }, 500);

                        // Abre el modal solo si hay suficientes productos
                        document.getElementById("hiddenModalButton").click();
                      }}
                    >
                      <Whatsapplogo className="svg-wpp2" />
                      Enviar pedido por WhatsApp
                    </button>

                    <button
                      id="hiddenModalButton"
                      className="hidden"
                      data-bs-toggle="modal"
                      data-bs-target="#exampleModal"
                    >
                      .
                    </button>
                  </div>
                </div>
              </div>
            ) : (
              <div className="home-6">
                <div className="vacio">
                  <p className="product-desc">
                    Aún no hay productos cargados en el carrito.
                  </p>
                </div>
              </div>
            )}
          </ul>
        </div>

        <div
          className="header-burger-menu-container"
          onClick={() => {
            handleClick();
            setIsVisible(!isVisible);
          }}
        >
          {click ? (
            <Back
              className={
                color
                  ? "header-close-menu header-close-menu-bg"
                  : "header-close-menu"
              }
            />
          ) : (
            <div className="cart-icon-container">
              <Cart
                className={
                  color
                    ? "header-burger-menu header-burger-menu-bg"
                    : "header-burger-menu"
                }
              />
              {totalQuantity > 0 && (
                <span
                  className={
                    color
                      ? "cart-item-count cart-item-count-bg"
                      : "cart-item-count"
                  }
                >
                  {totalQuantity}
                </span>
              )}
            </div>
          )}
        </div>
      </div>

      <section id="home" className="home-container">
        <div className="home-content">
          <div className="home-4">
            <div className="title-search">
              <h1 className="title categories-title title-nomarg">
                {clientType === "Mayorista"
                  ? "Catálogo Mayorista"
                  : "Catálogo Minorista"}
              </h1>

              {isVisible && (
                <div
                  id="search-container"
                  className={
                    color
                      ? "pagination-count3 pagination-count3-fixed"
                      : "pagination-count3"
                  }
                >
                  <div className="search-container">
                    <div className="form-group-input-search2">
                      <span className="input-group-text2" onClick={search}>
                        <Lupa className="input-group-svg lupa-catalogo" />
                      </span>
                      <input
                        className="search-input3"
                        type="text"
                        value={searchValue}
                        onChange={(e) => setSearchValue(e.target.value)}
                        placeholder="Buscar..."
                      />

                      {/* Agregamos la cruz (icono de borrar) para limpiar la búsqueda */}
                      {query && (
                        <span className="input-clearsearch">
                          <Close
                            className="input-group-svg"
                            onClick={handleClearSearch}
                            style={{ cursor: "pointer" }}
                          />
                        </span>
                      )}
                    </div>
                  </div>
                </div>
              )}
            </div>

            {/* Renderizar la lista de productos filtrados */}
            {isLoadingQuery === true ? (
              <div className="loading-single-furniture">
                <Loader />
                <p className="bold-loading">
                  Cargando productos con: "{query}"...
                </p>
              </div>
            ) : query !== "" ? (
              <div className="categorias-container">
                <div className="productos-filtrados">
                  <div className="filters-left" key={1}>
                    <div className="filter-container">
                      <div className="filter-btn-container2" role="button">
                        <p className="filter-btn-name2">{`Productos con: "${query}"`}</p>
                      </div>

                      <div className="collapse show" id="collapseQuery">
                        <div className="product-container">
                          {products?.length === 0 ? (
                            <div className="vacio2">
                              <p className="product-desc no-p">
                                No hay productos que contengan:{" "}
                                <b className="category-name">"{query}"</b>.
                              </p>
                            </div>
                          ) : (
                            products?.map((product, index) => {
                              const quantity =
                                productQuantities[product.idProducto] || 0;
                              const subtotal = calculateSubtotal(product);

                              if (
                                (clientType === "Minorista" &&
                                  product.porcentajeMinorista === 0 &&
                                  product.precioMinorista === 0) ||
                                (clientType === "Mayorista" &&
                                  product.porcentajeMayorista === 0 &&
                                  product.precioMayorista === 0)
                              ) {
                                return <></>; // No renderizar el producto
                              }

                              return (
                                <div
                                  className={`contenedor-producto ${
                                    product.stockTransitorio === 0
                                      ? "sin-stock"
                                      : ""
                                  }`}
                                  key={index}
                                >
                                  <div className="product">
                                    <div className="product-1-col">
                                      <figure className="figure">
                                        <Zoom
                                          className="zoom"
                                          onClick={() =>
                                            Swal.fire({
                                              title: product.nombre,
                                              imageUrl: `${product.urlImagen}`,
                                              imageWidth: 400,
                                              imageHeight: 400,
                                              imageAlt: "Vista Producto",
                                              confirmButtonColor: "#6c757d",
                                              confirmButtonText: "Cerrar",
                                              focusConfirm: true,
                                            })
                                          }
                                        ></Zoom>
                                        <img
                                          src={product.urlImagen}
                                          className="product-img"
                                          alt="Producto"
                                        />
                                      </figure>
                                    </div>
                                    <div className="product-2-col">
                                      <h3 className="product-title">
                                        {product.nombre}
                                      </h3>
                                      <h3 className="product-desc">
                                        <pre className="pre">
                                          {product.descripcion}
                                        </pre>
                                      </h3>
                                      {product.stockTransitorio === 0 && (
                                        <h3 className="product-title sin-stock-h">
                                          SIN STOCK
                                        </h3>
                                      )}
                                    </div>
                                    <div className="product-3-col">
                                      <p className="product-price">
                                        {(clientType === "Minorista"
                                          ? product.precioMinorista
                                          : product.precioMayorista) > 0
                                          ? `$${Math.ceil(
                                              clientType === "Minorista"
                                                ? product.precioMinorista
                                                : product.precioMayorista
                                            )
                                              .toLocaleString("es-ES", {
                                                minimumFractionDigits: 0,
                                                maximumFractionDigits: 2,
                                              })
                                              .replace(",", ".")
                                              .replace(
                                                /\B(?=(\d{3})+(?!\d))/g,
                                                "."
                                              )}`
                                          : `$${Math.ceil(
                                              Math.round(
                                                ((product.divisa === "Dólar"
                                                  ? product.precio * valorDolar
                                                  : product.precio) *
                                                  (1 +
                                                    (clientType === "Minorista"
                                                      ? product.porcentajeMinorista
                                                      : product.porcentajeMayorista) /
                                                      100)) /
                                                  50
                                              ) * 50
                                            )
                                              .toLocaleString("es-ES", {
                                                minimumFractionDigits: 0,
                                                maximumFractionDigits: 2,
                                              })
                                              .replace(",", ".")
                                              .replace(
                                                /\B(?=(\d{3})+(?!\d))/g,
                                                "."
                                              )}`}
                                      </p>
                                    </div>
                                  </div>

                                  {product.stockTransitorio !== 0 && (
                                    <div className="product-2">
                                      <div className="product-subtotal2">
                                        <p className="product-desc">
                                          Cantidad:{" "}
                                          <b className="product-price">
                                            {quantity !== null &&
                                            quantity !== undefined
                                              ? quantity
                                              : 0}
                                          </b>
                                        </p>
                                        <p className="product-desc">
                                          Subtotal:{" "}
                                          <b className="product-price">
                                            $
                                            {subtotal
                                              .toLocaleString("es-ES", {
                                                minimumFractionDigits: 0,
                                                maximumFractionDigits: 2,
                                              })
                                              .replace(",", ".")
                                              .replace(
                                                /\B(?=(\d{3})+(?!\d))/g,
                                                "."
                                              )}
                                          </b>
                                        </p>
                                      </div>
                                      <div className="product-quantity">
                                        <button
                                          className="quantity-btn btnminus"
                                          onClick={() =>
                                            handleSubtract(product)
                                          }
                                        >
                                          -
                                        </button>
                                        <input
                                          type="number"
                                          min="0"
                                          value={quantity}
                                          onChange={(event) =>
                                            handleQuantityChange(event, product)
                                          }
                                          onBlur={() => updateTotalQuantity()}
                                          className="quantity-input"
                                        />
                                        <button
                                          className="quantity-btn btnplus"
                                          onClick={() => handleAdd(product)}
                                        >
                                          +
                                        </button>
                                      </div>
                                    </div>
                                  )}

                                  {product.stockTransitorio !== 0 && (
                                    <div className="product-notes">
                                      <textarea
                                        className="textarea"
                                        placeholder="Agregar aclaraciones..."
                                        value={
                                          productNotes[product.idProducto] || ""
                                        }
                                        onChange={(event) =>
                                          handleAclaracionesChange(
                                            event,
                                            product
                                          )
                                        }
                                      ></textarea>
                                    </div>
                                  )}
                                </div>
                              );
                            })
                          )}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ) : (
              <div className="categorias-container">
                {isLoading === true ? (
                  <div className="loading-single-furniture">
                    <Loader />
                    <p className="bold-loading">Cargando categorías...</p>
                  </div>
                ) : categories === null ? (
                  <div className="filter-container">
                    <div className="vacio">
                      <p className="product-desc">
                        No hay categorías disponibles en este momento.
                      </p>
                    </div>
                  </div>
                ) : (
                  categories.map((category, index) => (
                    <div className="filters-left" key={index}>
                      <div className="filter-container">
                        <div
                          className="filter-btn-container2"
                          style={{
                            backgroundImage: `linear-gradient(#ffffffa6,#ffffffa6), url(${category.urlImagen})`,
                          }}
                          onClick={() => handleCategoryClick(index)}
                          data-bs-toggle="collapse"
                          href={`#collapseCategory${index}`}
                          role="button"
                          aria-expanded="false"
                          aria-controls={`collapseCategory${index}`}
                        >
                          <p className="filter-btn-name2">{category.nombre}</p>
                          <p className="filter-btn2">
                            {categorySign[index] || "+"}
                          </p>
                        </div>
                        <div
                          className="collapse"
                          id={`collapseCategory${index}`}
                        >
                          <div className="product-container">
                            {isLoadingProductByCategory === true ? (
                              <div className="loading-single-furniture">
                                <Loader />
                                <p className="bold-loading">
                                  Cargando productos de {category.nombre}...
                                </p>
                              </div>
                            ) : categoryProducts[category.nombre]?.length ===
                              0 ? (
                              <div className="vacio2">
                                <p className="product-desc no-p">
                                  No hay productos correspondientes a{" "}
                                  <b className="category-name">
                                    {category.nombre}
                                  </b>
                                  .
                                </p>
                              </div>
                            ) : (
                              categoryProducts[category.nombre]?.map(
                                (product, index) => {
                                  const quantity =
                                    productQuantities[product.idProducto] || 0;
                                  const subtotal = calculateSubtotal(product);

                                  if (
                                    (clientType === "Minorista" &&
                                      product.porcentajeMinorista === 0 &&
                                      product.precioMinorista === 0) ||
                                    (clientType === "Mayorista" &&
                                      product.porcentajeMayorista === 0 &&
                                      product.precioMayorista === 0)
                                  ) {
                                    return <></>; // No renderizar el producto
                                  }

                                  return (
                                    <div
                                      className={`contenedor-producto ${
                                        product.stockTransitorio === 0
                                          ? "sin-stock"
                                          : ""
                                      }`}
                                      key={index}
                                    >
                                      <div className="product">
                                        <div className="product-1-col">
                                          <figure className="figure">
                                            <Zoom
                                              className="zoom"
                                              onClick={() =>
                                                Swal.fire({
                                                  title: product.nombre,
                                                  imageUrl: `${product.urlImagen}`,
                                                  imageWidth: 400,
                                                  imageHeight: 400,
                                                  imageAlt: "Vista Producto",
                                                  confirmButtonColor: "#6c757d",
                                                  confirmButtonText: "Cerrar",
                                                  focusConfirm: true,
                                                })
                                              }
                                            ></Zoom>
                                            <img
                                              src={product.urlImagen}
                                              className="product-img"
                                              alt="Producto"
                                            />
                                          </figure>
                                        </div>
                                        <div className="product-2-col">
                                          <h3 className="product-title">
                                            {product.nombre}
                                          </h3>
                                          <h3 className="product-desc">
                                            <pre className="pre">
                                              {product.descripcion}
                                            </pre>
                                          </h3>
                                          {product.stockTransitorio === 0 && (
                                            <h3 className="product-title sin-stock-h">
                                              SIN STOCK
                                            </h3>
                                          )}
                                        </div>
                                        <div className="product-3-col">
                                          <p className="product-price">
                                            {(clientType === "Minorista"
                                              ? product.precioMinorista
                                              : product.precioMayorista) > 0
                                              ? `$${Math.ceil(
                                                  clientType === "Minorista"
                                                    ? product.precioMinorista
                                                    : product.precioMayorista
                                                )
                                                  .toLocaleString("es-ES", {
                                                    minimumFractionDigits: 0,
                                                    maximumFractionDigits: 2,
                                                  })
                                                  .replace(",", ".")
                                                  .replace(
                                                    /\B(?=(\d{3})+(?!\d))/g,
                                                    "."
                                                  )}`
                                              : `$${Math.ceil(
                                                  Math.round(
                                                    ((product.divisa === "Dólar"
                                                      ? product.precio *
                                                        valorDolar
                                                      : product.precio) *
                                                      (1 +
                                                        (clientType ===
                                                        "Minorista"
                                                          ? product.porcentajeMinorista
                                                          : product.porcentajeMayorista) /
                                                          100)) /
                                                      50
                                                  ) * 50
                                                )
                                                  .toLocaleString("es-ES", {
                                                    minimumFractionDigits: 0,
                                                    maximumFractionDigits: 2,
                                                  })
                                                  .replace(",", ".")
                                                  .replace(
                                                    /\B(?=(\d{3})+(?!\d))/g,
                                                    "."
                                                  )}`}
                                          </p>
                                        </div>
                                      </div>

                                      {product.stockTransitorio !== 0 && (
                                        <div className="product-2">
                                          <div className="product-subtotal2">
                                            <p className="product-desc">
                                              Cantidad:{" "}
                                              <b className="product-price">
                                                {quantity !== null &&
                                                quantity !== undefined
                                                  ? quantity
                                                  : 0}
                                              </b>
                                            </p>
                                            <p className="product-desc">
                                              Subtotal:{" "}
                                              <b className="product-price">
                                                $
                                                {subtotal
                                                  .toLocaleString("es-ES", {
                                                    minimumFractionDigits: 0,
                                                    maximumFractionDigits: 2,
                                                  })
                                                  .replace(",", ".")
                                                  .replace(
                                                    /\B(?=(\d{3})+(?!\d))/g,
                                                    "."
                                                  )}
                                              </b>
                                            </p>
                                          </div>
                                          <div className="product-quantity">
                                            <button
                                              className="quantity-btn btnminus"
                                              onClick={() =>
                                                handleSubtract(product)
                                              }
                                            >
                                              -
                                            </button>
                                            <input
                                              type="number"
                                              min="0"
                                              value={quantity}
                                              onChange={(event) =>
                                                handleQuantityChange(
                                                  event,
                                                  product
                                                )
                                              }
                                              onBlur={() =>
                                                updateTotalQuantity()
                                              }
                                              className="quantity-input"
                                            />
                                            <button
                                              className="quantity-btn btnplus"
                                              onClick={() => handleAdd(product)}
                                            >
                                              +
                                            </button>
                                          </div>
                                        </div>
                                      )}

                                      {product.stockTransitorio !== 0 && (
                                        <div className="product-notes">
                                          <textarea
                                            className="textarea"
                                            placeholder="Agregar aclaraciones..."
                                            value={
                                              productNotes[
                                                product.idProducto
                                              ] || ""
                                            }
                                            onChange={(event) =>
                                              handleAclaracionesChange(
                                                event,
                                                product
                                              )
                                            }
                                          ></textarea>
                                        </div>
                                      )}
                                    </div>
                                  );
                                }
                              )
                            )}
                          </div>
                        </div>
                        <p className="filter-separator2"></p>
                      </div>
                    </div>
                  ))
                )}
              </div>
            )}

            {/* Botón "Enviar pedido por WhatsApp" con animación "shake" */}
            {showButton && (
              <div className="btn-whatsapp-container">
                <button
                  type="button"
                  className={`btn-whatsapp-shake ${
                    showButton ? "visible" : ""
                  }`}
                  // data-bs-toggle="modal"
                  // data-bs-target="#exampleModal"
                  onClick={() => {
                    // Agregar verificación antes de abrir el modal
                    if (clientType === "Mayorista" && totalQuantity < 5) {
                      const faltanProductos = 5 - totalQuantity;

                      Swal.fire({
                        icon: "warning",
                        title: `Falta${
                          faltanProductos !== 1 ? "n" : ""
                        } ${faltanProductos} producto${
                          faltanProductos !== 1 ? "s" : ""
                        } para que puedas hacer tu pedido mayorista`,
                        text: "Debes agregar al menos 5 productos al carrito para enviar el pedido.",
                        confirmButtonText: "Aceptar",
                        showCancelButton: false,
                        confirmButtonColor: "#f8bb86",
                      });

                      return; // Detener el proceso si no hay suficientes productos
                    }

                    // Lógica adicional después de la verificación (si es necesario)
                    // ClearClientInputs();
                    setTimeout(function () {
                      $("#nombre").focus();
                    }, 500);

                    // Abre el modal solo si hay suficientes productos
                    document.getElementById("hiddenModalButton").click();
                  }}
                >
                  <Whatsapplogo className="svg-wpp2 wpp-shake" />
                  Enviar pedido por WhatsApp{" "}
                  <b>
                    $
                    {total
                      .toLocaleString("es-ES", {
                        minimumFractionDigits: 0,
                        maximumFractionDigits: 2,
                      })
                      .replace(",", ".")
                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                  </b>
                </button>

                <button
                  id="hiddenModalButton"
                  className="hidden"
                  data-bs-toggle="modal"
                  data-bs-target="#exampleModal"
                >
                  .
                </button>
              </div>
            )}
          </div>
        </div>
      </section>

      <div
        className="modal fade"
        id="exampleModal"
        data-bs-backdrop="static"
        data-bs-keyboard="false"
        tabIndex="-1"
        aria-labelledby="staticBackdropLabel"
        aria-hidden="true"
      >
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h1 className="title-form-login" id="exampleModalLabel">
                COMPLETA EL SIGUIENTE FORMULARIO PARA QUE PREPAREMOS TU PEDIDO
              </h1>
            </div>
            <div className="modal-body modal-body-client">
              <div className="container mt-4">
                <form>
                  <div className="form-group">
                    <label className="label">Nombre completo:</label>
                    <div className="form-group-input nombre-input">
                      <input
                        type="text"
                        className="input2"
                        id="nombre"
                        value={nombre}
                        onChange={(event) => {
                          setNombre(event.target.value);
                        }}
                      />
                    </div>

                    <label className="label">DNI:</label>
                    <div className="form-group-input nombre-input">
                      <input
                        type="number"
                        className="input2"
                        id="dni"
                        value={dni}
                        onChange={(event) => {
                          setDni(event.target.value);
                        }}
                      />
                    </div>

                    <label className="label">Número de teléfono:</label>
                    <div className="form-group-input desc-input">
                      <input
                        type="number"
                        className="input2"
                        id="telefono"
                        value={telefono}
                        onChange={(event) => {
                          setTelefono(event.target.value);
                        }}
                      />
                    </div>

                    <label className="label selects" htmlFor="envio">
                      Entrega:
                    </label>
                    <div className="form-group-input nombre-input">
                      <select
                        className="input2"
                        style={{ cursor: "pointer" }}
                        name="envio"
                        id="envio"
                        value={envio}
                        onChange={(e) => setEnvio(e.target.value)}
                      >
                        <option hidden key={0} value="0">
                          Seleccione una opción
                        </option>
                        <option className="btn-option" value="1">
                          Lo retiro por el local ($0)
                        </option>
                        {habilitadoEnvioDomicilio && (
                          <option
                            className="btn-option"
                            hidden={costoEnvioDomicilio === 0}
                            value="2"
                          >
                            Envío a domicilio (${costoEnvioDomicilio})
                          </option>
                        )}
                      </select>
                    </div>

                    {/* Renderizar los campos de dirección y entre calles solo si no es "Lo retiro por el local" */}
                    {envio == 2 && (
                      <>
                        <label className="label">Dirección:</label>
                        <div className="form-group-input desc-input">
                          <input
                            type="text"
                            className="input2"
                            id="direccion"
                            value={direccion}
                            onChange={(event) => {
                              setDireccion(event.target.value);
                            }}
                          />
                        </div>

                        <label className="label">Entre que calles:</label>
                        <div className="form-group-input desc-input">
                          <input
                            type="text"
                            className="input2"
                            id="calles"
                            value={calles}
                            onChange={(event) => {
                              setCalles(event.target.value);
                            }}
                          />
                        </div>
                      </>
                    )}

                    {envio == 1 && (
                      <>
                        <label className="label">Dirección:</label>
                        <div className="form-group-input desc-input">
                          <input
                            type="text"
                            className="input2"
                            id="direccionAuto"
                            value={direccionAuto}
                            style={{
                              backgroundColor: "#d3d3d3",
                              cursor: "default",
                            }}
                            readOnly
                          />
                          <a
                            href="https://www.google.com/maps/place/C%C3%B3rdoba/@-31.3994267,-64.2767842,12z/data=!3m1!4b1!4m6!3m5!1s0x9432985f478f5b69:0xb0a24f9a5366b092!8m2!3d-31.4200833!4d-64.1887761!16zL20vMDFrMDNy?entry=ttu"
                            target="_blank"
                            rel="noopener noreferrer"
                          >
                            <Location className="location-btn" />
                          </a>
                        </div>

                        <label className="label">Horarios de atención:</label>
                        <div className="form-group-input desc-input">
                          <input
                            type="text"
                            className="input2"
                            id="horariosDeAtencion"
                            value={horariosAtencion}
                            style={{
                              backgroundColor: "#d3d3d3",
                              cursor: "default",
                            }}
                            readOnly
                          />
                        </div>
                      </>
                    )}

                    <label className="label selects" htmlFor="abono">
                      Cómo va a abonar:
                    </label>
                    <div className="form-group-input nombre-input">
                      <select
                        className="input2"
                        onClick={handleAbonoClick}
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
                        <option
                          hidden={envio == 2}
                          className="btn-option"
                          value="3"
                        >
                          Tarjeta de débito
                        </option>
                        <option
                          hidden={envio == 2}
                          className="btn-option"
                          value="4"
                        >
                          Tarjeta de crédito
                        </option>
                        <option className="btn-option" value="5">
                          Mercado Pago
                        </option>
                      </select>
                    </div>

                    {abono == 2 && (
                      <>
                        <label className="label">CBU:</label>
                        <div className="form-group-input desc-input">
                          <input
                            type="text"
                            className="input2"
                            id="cbu"
                            value={cbu}
                            style={{
                              backgroundColor: "#d3d3d3",
                              cursor: "default",
                            }}
                            readOnly
                          />
                        </div>
                      </>
                    )}

                    <label className="label selects" htmlFor="vendedor">
                      Vendedor:
                    </label>
                    <div className="form-group-input nombre-input">
                      <select
                        className="input2"
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
                          Array.from(listaNombresVendedores).map((opts, i) => (
                            <option
                              className="btn-option"
                              key={i}
                              value={opts.idUsuario}
                            >
                              {opts.nombre}
                            </option>
                          ))}
                        <option className="no-vendedor" value="-1">
                          No recibí atención
                        </option>
                      </select>
                    </div>

                    <b>Cantidad total de productos: {totalQuantity}</b>

                    {/* Mostrar el costo de envío solo si la opción es "Envío a domicilio" */}
                    {envio == 2 &&
                      costoEnvioDomicilio > 0 &&
                      habilitadoEnvioDomicilio === true && (
                        <>
                          <b>
                            Subtotal: $
                            {(total - costoEnvioDomicilio)
                              .toLocaleString("es-ES", {
                                minimumFractionDigits: 0,
                                maximumFractionDigits: 2,
                              })
                              .replace(",", ".")
                              .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                          </b>

                          <b className="costo-envio">
                            Costo de envío: ${costoEnvioDomicilio}
                          </b>
                        </>
                      )}

                    <b>
                      Total: $
                      {total
                        .toLocaleString("es-ES", {
                          minimumFractionDigits: 0,
                          maximumFractionDigits: 2,
                        })
                        .replace(",", ".")
                        .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                    </b>
                  </div>
                </form>

                {abono == 5 ? (
                  <div>
                    {showWallet ? (
                      <Wallet initialization={{ preferenceId }} />
                    ) : (
                      // Display "Pagar y enviar pedido" button
                      <div id="div-btn-save">
                        <button
                          className="btnmeli"
                          id="btn-save"
                          onClick={handleSubmitPedidoMercadoPago}
                        >
                          <div className="btn-save-update-close">
                            <Mercadopagologo className="meli-btn" />
                            <p className="p-save-update-close">
                              Pagar y enviar pedido
                            </p>
                          </div>
                        </button>
                      </div>
                    )}
                  </div>
                ) : (
                  <div id="div-btn-save">
                    <button
                      className="btnadd2"
                      id="btn-save"
                      onClick={handleSubmitPedido}
                    >
                      <div className="btn-save-update-close">
                        <Whatsapplogo className="save-btn" />
                        <p className="p-save-update-close">Enviar Pedido</p>
                      </div>
                    </button>
                  </div>
                )}
              </div>
            </div>
            <div className="modal-footer">
              <button
                type="button"
                className="btn btn-success"
                onClick={() => {
                  CloseModal();

                  const userData = {
                    nombre,
                    dni,
                    direccion,
                    calles,
                    telefono,
                    abono,
                    vendedor,
                    envio,
                  };

                  // Convert the object to a JSON string and store it in localStorage
                  localStorage.setItem("userData", JSON.stringify(userData));
                }}
              >
                <div className="btn-save-update-close">
                  <Save className="close-btn" />
                  <p className="p-save-update-close">Cerrar y guardar</p>
                </div>
              </button>

              <button
                type="button"
                className="btn btn-secondary"
                onClick={() => {
                  if (IsEmpty() === true) {
                    ClearClientInputs();
                    CloseModal();
                  } else {
                    Swal.fire({
                      icon: "warning",
                      title: "¿Está seguro de que desea cerrar el formulario?",
                      text: "Se perderán todos los datos cargados",
                      confirmButtonText: "Aceptar",
                      showCancelButton: true,
                      cancelButtonText: "Cancelar",
                      confirmButtonColor: "#f8bb86",
                      cancelButtonColor: "#6c757d",
                    }).then((result) => {
                      if (result.isConfirmed) {
                        ClearClientInputs();
                        CloseModal();
                        ClearFormData();
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
    </div>
  );
  //#endregion
};

export default CatalogueCart;
