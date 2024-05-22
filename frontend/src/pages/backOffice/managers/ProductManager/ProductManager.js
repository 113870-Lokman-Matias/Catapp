import Swal from "sweetalert2";
import { ReactComponent as Filter } from "../../../../assets/svgs/filter.svg";
import { ReactComponent as Lupa } from "../../../../assets/svgs/lupa.svg";
import { useDownloadExcel } from "react-export-table-to-excel";
import $ from "jquery";

import * as signalR from "@microsoft/signalr";

import React, { useEffect, useState, useRef } from "react";
import { Link } from "react-router-dom";
import { Helmet } from "react-helmet";

import "./ProductManager.css";

//#region SVG'S Imports
import { ReactComponent as Edit } from "../../../../assets/svgs/edit.svg";
import { ReactComponent as Delete } from "../../../../assets/svgs/delete.svg";
import { ReactComponent as Add } from "../../../../assets/svgs/add.svg";
import { ReactComponent as Save } from "../../../../assets/svgs/save.svg";
import { ReactComponent as Update } from "../../../../assets/svgs/update.svg";
import { ReactComponent as Close } from "../../../../assets/svgs/closebtn.svg";
import { ReactComponent as Back } from "../../../../assets/svgs/back.svg";
import { ReactComponent as Agregar } from "../../../../assets/svgs/agregar.svg";
import { ReactComponent as Quitar } from "../../../../assets/svgs/quitar.svg";
import { ReactComponent as Page } from "../../../../assets/svgs/page.svg";
import { ReactComponent as Stock } from "../../../../assets/svgs/stockdetails.svg";

import { ReactComponent as Excel } from "../../../../assets/svgs/excel.svg";

import { ReactComponent as ProductInput } from "../../../../assets/svgs/productinput.svg";
import { ReactComponent as DescriptionInput } from "../../../../assets/svgs/descriptioninput.svg";
import { ReactComponent as PriceInput } from "../../../../assets/svgs/priceinput.svg";
import { ReactComponent as TypePriceInput } from "../../../../assets/svgs/typepriceinput.svg";
import { ReactComponent as DolarInput } from "../../../../assets/svgs/dolarinput.svg";
import { ReactComponent as PesoInput } from "../../../../assets/svgs/pesoinput.svg";
import { ReactComponent as StockInput } from "../../../../assets/svgs/stockinput.svg";
import { ReactComponent as PercentageInput } from "../../../../assets/svgs/percentageinput.svg";
import { ReactComponent as ImageInput } from "../../../../assets/svgs/imageinput.svg";
import { ReactComponent as CategoryInput } from "../../../../assets/svgs/category.svg";

import { ReactComponent as MotivoInput } from "../../../../assets/svgs/motivo.svg";
import { ReactComponent as OtroInput } from "../../../../assets/svgs/otro.svg";
//#endregion

import Loader from "../../../../components/Loaders/LoaderCircle";

import { GetCategoriesManage } from "../../../../services/CategoryService";
import { GetCotizacionDolarUnicamente } from "../../../../services/DollarService";
import {
  GetProductsManage,
  SaveProducts,
  UpdateProducts,
  UpdateProductsStock,
  DeleteProducts,
  UploadImages,
} from "../../../../services/ProductService";

import { SaveStockDetail } from "../../../../services/DetailService";

function ProductManager() {
  //#region Constantes
  const [isLoading, setIsLoading] = useState(false);

  const [valorDolar, setvalorDolar] = useState(0);

  const [idProducto, setIdProducto] = useState("");

  const [nombre, setNombre] = useState("");
  const [prevNombre, setPrevNombre] = useState("");

  const [descripcion, setDescripcion] = useState("");
  const [prevDescripcion, setPrevDescripcion] = useState("");

  const [tipoPrecioMinorista, setTipoPrecioMinorista] = useState("");
  const [tipoPrecioMayorista, setTipoPrecioMayorista] = useState("");

  const [divisa, setDivisa] = useState("");
  const [prevDivisa, setPrevDivisa] = useState("");

  const [precioMinorista, setPrecioMinorista] = useState("");
  const [prevPrecioMinorista, setPrevPrecioMinorista] = useState("");

  const [precioMayorista, setPrecioMayorista] = useState("");
  const [prevPrecioMayorista, setPrevPrecioMayorista] = useState("");

  const [precio, setPrecio] = useState("");
  const [prevPrecio, setPrevPrecio] = useState("");

  const [porcentajeMinorista, setPorcentajeMinorista] = useState("");
  const [prevPorcentajeMinorista, setPrevPorcentajeMinorista] = useState("");

  const [porcentajeMayorista, setPorcentajeMayorista] = useState("");
  const [prevPorcentajeMayorista, setPrevPorcentajeMayorista] = useState("");

  const [stock, setStock] = useState("");
  const [prevStock, setPrevStock] = useState("");

  const [unidadesQuitar, setUnidadesQuitar] = useState("");
  const [unidadesAgregar, setUnidadesAgregar] = useState("");

  const [accion, setAccion] = useState("");

  const [cantidad, setCantidad] = useState("");

  const [motivo, setMotivo] = useState("");

  const [otro, setOtro] = useState("");

  // Mantener el valor original del stock
  const [originalStock, setOriginalStock] = useState("");

  const [ocultar, setOcultar] = useState("");
  const [prevOcultar, setPrevOcultar] = useState("");

  var checkbox = document.getElementById("ocultar");

  const [idCategoria, setIdCategoria] = useState("");
  const [prevIdCategoria, setPrevIdCategoria] = useState("");

  const [idImagen, setIdImagen] = useState("");

  const [urlImagen, setUrlImagen] = useState("");
  const [prevUrlImagen, setPrevUrlImagen] = useState("");

  const [modalTitle, setModalTitle] = useState("");

  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);

  const [originalProductsList, setOriginalProductsList] = useState(products);

  const [title, setTitle] = useState(["Detalles de Productos"]);

  const [categorySign, setCategorySign] = useState("+");

  const [filterName, setFilterName] = useState("");

  const [filterType, setFilterType] = useState("");

  const [imageSelected, setImageSelected] = useState();

  const [query, setQuery] = useState("");

  const [hidden, setHidden] = useState(false);

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

  //#region Constantes de la paginacion
  const [currentPage, setCurrentPage] = useState(1);
  const [productsPerPage, setProductsPerPage] = useState(20);
  const lastIndex = currentPage * productsPerPage;
  const firstIndex = lastIndex - productsPerPage;
  const productsTable = products.slice(firstIndex, lastIndex);
  const npage = Math.ceil(products.length / productsPerPage);
  const numbers = [...Array(npage + 1).keys()].slice(1);

  const [maxPageNumbersToShow, setMaxPageNumbersToShow] = useState(9);
  const minPageNumbersToShow = 0;
  //#endregion
  //#endregion

  //#region UseEffect
  useEffect(() => {
    (async () => {
      setIsLoading(true);

      try {
        await Promise.all([
          GetProductsManage(setProducts),
          GetProductsManage(setOriginalProductsList),
          GetCategoriesManage(setCategories),
          GetCotizacionDolarUnicamente(setvalorDolar),
        ]);
        setIsLoading(false);
      } catch (error) {
        // Manejar errores aquí si es necesario
        setIsLoading(false);
      }
    })();

    if (window.matchMedia("(max-width: 500px)").matches) {
      setProductsPerPage(1);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 600px)").matches) {
      setProductsPerPage(2);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 700px)").matches) {
      setProductsPerPage(3);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 800px)").matches) {
      setProductsPerPage(4);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 900px)").matches) {
      setProductsPerPage(5);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 1000px)").matches) {
      setProductsPerPage(6);
      setMaxPageNumbersToShow(1);
    } else if (window.matchMedia("(max-width: 1140px)").matches) {
      setProductsPerPage(7);
      setMaxPageNumbersToShow(1);
    } else {
      setProductsPerPage(10);
      setMaxPageNumbersToShow(9);
    }
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

    connection.on("MensajeCrudCategoria", async () => {
      try {
        GetCategoriesManage(setCategories);
        GetProductsManage(setProducts);
        GetProductsManage(setOriginalProductsList);
      } catch (error) {
        console.error("Error al obtener las categorias: " + error);
      }
    });

    connection.on("MensajeUpdateCotizacion", async () => {
      try {
        GetCotizacionDolarUnicamente(setvalorDolar);
      } catch (error) {
        console.error("Error al obtener la cotización: " + error);
      }
    });

    connection.on("MensajeCreateDetalleStock", async () => {
      try {
        GetProductsManage(setProducts);
        GetProductsManage(setOriginalProductsList);
      } catch (error) {
        console.error("Error al obtener la cotización: " + error);
      }
    });

    connection.on("MensajeCrudPedido", async () => {
      try {
        GetProductsManage(setProducts);
        GetProductsManage(setOriginalProductsList);
      } catch (error) {
        console.error("Error al obtener la cotización: " + error);
      }
    });

    return () => {
      connection.stop();
    };
  }, []);
  //#endregion

  //#region Función para mostrar '+' o '-' cuando se toca en el 'collapse' del filtro de categoría
  const handleCategoryClick = () => {
    setCategorySign(categorySign === "+" ? "-" : "+");
  };
  //#endregion

  //#region Función para actualizar el stock según la cantidad de unidades a quitar
  const handleUnidadesQuitarChange = (event) => {
    const unidades = event.target.value;

    // Verificar si el valor ingresado es un número válido y no contiene el carácter "-"
    if (!isNaN(unidades) && !unidades.includes("-")) {
      setUnidadesQuitar(unidades);
      setCantidad(unidades); // Actualizar cantidad también
      setAccion("Quitar");

      if (unidades > originalStock) {
        Swal.fire({
          icon: "error",
          title: "Stock insuficiente",
          text: "No puedes quitar más unidades de las que hay en stock.",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#f27474",
        }).then(setUnidadesQuitar(0), setStock(originalStock));
      } else if (unidades === "") {
        setStock(originalStock);
      } else {
        const nuevoStock = originalStock - parseInt(unidades);
        setStock(nuevoStock >= 0 ? nuevoStock : 0); // No permitir stock negativo
      }
    } else {
      // Mostrar SweetAlert indicando que no se puede ingresar el carácter "-"
      Swal.fire({
        icon: "error",
        title: "Carácter inválido",
        text: "No puedes ingresar números negativos en este campo.",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
    }
  };
  //#endregion

  //#region Función para actualizar el stock según las unidades para agregar
  const handleUnidadesAgregarChange = (event) => {
    const unidades = event.target.value;

    // Verificar si el valor ingresado es un número válido y no negativo
    if (!isNaN(unidades) && unidades >= 0) {
      setUnidadesAgregar(unidades);
      setCantidad(unidades); // Actualizar cantidad también
      setAccion("Agregar");

      if (unidades === "") {
        setStock(originalStock);
      } else {
        const nuevoStock = originalStock + parseInt(unidades); // Usar el valor actual del estado del stock
        setStock(nuevoStock >= 0 ? nuevoStock : 0); // No permitir stock negativo
      }
    } else {
      // Mostrar SweetAlert indicando que no se pueden ingresar números negativos
      Swal.fire({
        icon: "error",
        title: "Número inválido",
        text: "No puedes ingresar números negativos en este campo.",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
    }
  };
  //#endregion

  //#region Función para borrar la negrita al filtro (item) seleccionado anteriormente (aplica al filtro de: categoria)
  const ClearBold = () => {
    // borrar la negritra a cualquier item seleccionado anteriormente
    const items = document.getElementsByClassName("items-collapse"); // identificamos el boton con el nombre de la categoria
    for (let i = 0; i < items.length; i++) {
      //recorremos los botones con las opciones de categorías
      if (items[i].classList.contains("active")) {
        // reconoce la categoria clickeada
        items[i].classList.remove("active"); // le remueve la clase 'active' a la categoria que se clickeo anterioremnte para filtrar y asi luego de limpiar el filtro ya no tiene mas la clase 'active'
        break;
      }
    }

    if (hidden === true) {
      setHidden(false);
    }
  };
  //#endregion

  //#region Función para borrar cualquier filtro
  const ClearFilter = () => {
    setProducts(originalProductsList); // trae la lista de productos original, sin ningun filtro
    setQuery("");
    setFilterName("");
    setFilterType("");
    setTitle("Detalles de Productos");
    document.getElementById("clear-filter").style.display = "none";
    document.getElementById("clear-filter2").style.display = "none"; // esconde del DOM el boton de limpiar filtros
    ClearBold();
    setCurrentPage(1);
    window.scrollTo(0, 0);
  };
  //#endregion

  //#region Funciónes de los filtros
  const filterResultCategory = (category) => {
    setProducts(originalProductsList);
    const result = originalProductsList.filter((originalProductsList) => {
      return originalProductsList.nombreCategoria === category;
    });
    setTitle(`Detalles de Productos de ${category}`);
    setProducts(result);
    setQuery("");
    document.getElementById("clear-filter").style.display = "flex";
    document.getElementById("clear-filter2").style.display = "flex";
    setFilterName(category);
    setFilterType("category");
    ClearBold();
    setCurrentPage(1);
    window.scrollTo(0, 0);
  };

  const filterResultHidden = (hidden) => {
    if (hidden === false) {
      setProducts(originalProductsList);
      const result = originalProductsList.filter((originalProductsList) => {
        return originalProductsList.ocultar === true;
      });
      setTitle("Detalles de Productos ocultos");
      setProducts(result);
      setQuery("");
      document.getElementById("clear-filter").style.display = "flex";
      document.getElementById("clear-filter2").style.display = "flex";
      setFilterName("Oculto");
      setFilterType("hidden");
      ClearBold();
      setCurrentPage(1);
      window.scrollTo(0, 0);
    } else {
      ClearFilter();
    }
  };
  //#endregion

  //#region Función para filtrar producto mediante una consulta personalizada
  const search = () => {
    setProducts(originalProductsList);
    const result = originalProductsList.filter(
      (product) =>
        product.nombre.toLowerCase().includes(query.toLowerCase()) ||
        product.idProducto
          .toString()
          .toLowerCase()
          .includes(query.toLowerCase())
    );
    setProducts(result);
    document.getElementById("clear-filter").style.display = "flex";
    document.getElementById("clear-filter2").style.display = "flex";
    setTitle(
      `Detalles de Productos con: "${
        query.charAt(0).toUpperCase() + query.slice(1)
      }"`
    );
    setFilterName(query.charAt(0).toUpperCase() + query.slice(1));
    setFilterType("search");
    ClearBold();
    setCurrentPage(1);
    window.scrollTo(0, 0);
    if (query === "") {
      document.getElementById("clear-filter").style.display = "none";
      document.getElementById("clear-filter2").style.display = "none";
      setFilterType("");
      setTitle("Detalles de Productos");
      window.scrollTo(0, 0);
    }
  };
  //#endregion

  //#region Función para subir una imagen a cloudinary
  const uploadImage = async () => {
    try {
      const result = await UploadImages(imageSelected);
      return result;
    } catch (error) {
      console.log(error);
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
  function ClearProductInputs() {
    setIdProducto("");

    setNombre("");
    setDescripcion("");
    setDivisa("");
    setPrecioMinorista("");
    setPrecioMayorista("");
    setPrecio("");
    setPorcentajeMinorista("");
    setPorcentajeMayorista("");
    setStock("");
    setOcultar("");
    setIdCategoria("");
    setIdImagen("");
    setUrlImagen("");

    setTipoPrecioMayorista("");
    setTipoPrecioMinorista("");

    setUnidadesQuitar("");
    setUnidadesAgregar("");
    setCantidad("");
    setMotivo("");
    setOtro("");
    setAccion("");

    document.getElementById("image-url").value = "";
  }
  //#endregion

  //#region Función para obtener los valores almacenados de un producto y cargar cada uno de ellos en su input correspondiente
  function RetrieveProductInputs(product) {
    setIdProducto(product.idProducto);
    setNombre(product.nombre);
    setDescripcion(product.descripcion);
    setDivisa(product.idDivisa);
    setPrecioMinorista(product.precioMinorista);
    setPrecioMayorista(product.precioMayorista);
    setPrecio(product.precio);
    setPorcentajeMinorista(product.porcentajeMinorista);
    setPorcentajeMayorista(product.porcentajeMayorista);
    setStock(product.stockTransitorio);
    setOcultar(product.ocultar);
    setIdCategoria(product.idCategoria);
    setIdImagen(product.idImagen);
    setUrlImagen(product.urlImagen);

    if (product.precioMinorista > 0 && product.porcentajeMinorista === 0) {
      setTipoPrecioMinorista("Manual");
    } else if (
      product.porcentajeMinorista > 0 &&
      product.precioMinorista === 0
    ) {
      setTipoPrecioMinorista("Porcentual");
    }

    if (product.precioMayorista > 0 && product.porcentajeMayorista === 0) {
      setTipoPrecioMayorista("Manual");
    } else if (
      product.porcentajeMayorista > 0 &&
      product.precioMayorista === 0
    ) {
      setTipoPrecioMayorista("Porcentual");
    }

    setPrevNombre(product.nombre);
    setPrevDescripcion(product.descripcion);
    setPrevDivisa(product.idDivisa);
    setPrevPrecioMinorista(product.precioMinorista);
    setPrevPrecioMayorista(product.precioMayorista);
    setPrevPrecio(product.precio);
    setPrevPorcentajeMinorista(product.porcentajeMinorista);
    setPrevPorcentajeMayorista(product.porcentajeMayorista);
    setPrevStock(product.stockTransitorio);
    setPrevOcultar(product.ocultar);
    setPrevIdCategoria(product.idCategoria);
    setPrevUrlImagen(product.urlImagen);
  }
  //#endregion

  //#region Función para volver el formulario a su estado inicial, borrando los valores de los inputs, cargando los selects y refrezcando la lista de productos
  function InitialState() {
    ClearProductInputs();
    GetProductsManage(setProducts);
    GetProductsManage(setOriginalProductsList);
    GetCategoriesManage(setCategories);
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

  //#region Función para cerrar el modal para quitar stock manualmente mediante el codigo
  function CloseModalQuitar() {
    $(document).ready(function () {
      $("#btn-close-modal-quitar").click();
    });
  }
  //#endregion

  //#region Función para cerrar el modal para agregar stock manualmente mediante el codigo
  function CloseModalAgregar() {
    $(document).ready(function () {
      $("#btn-close-modal-agregar").click();
    });
  }
  //#endregion

  //#region Función para mostrar el boton de Guardar de manera normal
  function ShowSaveButton() {
    const btnSave = document.getElementById("btn-save");
    const divBtnSave = document.getElementById("div-btn-save");
    btnSave.style.pointerEvents = "all";
    btnSave.style.opacity = "1";
    divBtnSave.style.cursor = "default";
  }
  //#endregion

  //#region Función para verificar si los valores ingresados a traves de los inputs son correctos
  function IsValid() {
    if (nombre === "") {
      Swal.fire({
        icon: "error",
        title: "El nombre no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#nombre").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (descripcion === "") {
      Swal.fire({
        icon: "error",
        title: "La descripción no puede estar vacía",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#descripcion").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (divisa === "") {
      Swal.fire({
        icon: "error",
        title: "La divisa no puede estar vacía",
        text: "Seleccione una divisa",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#divisa").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (
      precio === "" &&
      tipoPrecioMayorista === "Porcentual" &&
      tipoPrecioMinorista === "Porcentual"
    ) {
      Swal.fire({
        icon: "error",
        title: "El costo del producto no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#precio").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (
      precio == 0 &&
      (tipoPrecioMayorista == "Porcentual" ||
        tipoPrecioMinorista == "Porcentual")
    ) {
      Swal.fire({
        icon: "error",
        title:
          "El campo de costo del producto no puede estar vacío si hay precios definidos como porcentaje.",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#precio").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (tipoPrecioMinorista === "") {
      Swal.fire({
        icon: "error",
        title: "El tipo de precio minorista no puede estar vacío",
        text: "Seleccione un tipo de precio minorista",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#tipoPrecioMinorista").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (precioMinorista === "") {
      Swal.fire({
        icon: "error",
        title: "El precio minorista manual no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#precioMinorista").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (porcentajeMinorista === "") {
      Swal.fire({
        icon: "error",
        title: "El porcentaje de ganancia minorista no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#porcentajeMinorista").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (tipoPrecioMayorista === "") {
      Swal.fire({
        icon: "error",
        title: "El tipo de precio mayorista no puede estar vacío",
        text: "Seleccione un tipo de precio mayorista",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#tipoPrecioMayorista").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (precioMayorista === "") {
      Swal.fire({
        icon: "error",
        title: "El precio mayorista manual no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#precioMayorista").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (porcentajeMayorista === "") {
      Swal.fire({
        icon: "error",
        title: "El porcentaje de ganancia mayorista no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#porcentajeMayorista").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (stock === "") {
      Swal.fire({
        icon: "error",
        title: "El Stock no puede estar vacío. Si no hay ingrese 0",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#stock").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (ocultar === "") {
      Swal.fire({
        icon: "error",
        title: "Debe indicar si se encuentra oculto",
        text: "Clickeé el botón en caso de que el mismo se encuentre oculto",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (idCategoria === "") {
      Swal.fire({
        icon: "error",
        title: "La categoría no puede estar vacía",
        text: "Seleccione una categoría",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (urlImagen === "") {
      Swal.fire({
        icon: "error",
        title: "El campo imagen no puede estar vacío",
        text: "Suba una imagen",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#imagen").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si los valores ingresados a traves de los inputs son correctos (Cuando el rol es vendedor)
  function IsValidVendedor() {
    if (nombre === "") {
      Swal.fire({
        icon: "error",
        title: "El nombre no puede estar vacío",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#nombre").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (descripcion === "") {
      Swal.fire({
        icon: "error",
        title: "La descripción no puede estar vacía",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#descripcion").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (idCategoria === "") {
      Swal.fire({
        icon: "error",
        title: "La categoría no puede estar vacía",
        text: "Seleccione una categoría",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    } else if (urlImagen === "") {
      Swal.fire({
        icon: "error",
        title: "El campo imagen no puede estar vacío",
        text: "Suba una imagen",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#imagen").focus();
        }, 500);
      });
      if (modalTitle === "Registrar Producto") {
        ShowSaveButton();
      }
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si el valor "nombre" ingresado a traves del input no esta repetido
  function IsRepeated() {
    for (let i = 0; i < products.length; i++) {
      if (
        nombre.toLowerCase() === products[i].nombre.toLowerCase() &&
        nombre !== prevNombre
      ) {
        Swal.fire({
          icon: "error",
          title: "El nombre ingresado ya se encuentra registrado",
          text: "Modifique el campo",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#f27474",
        }).then(function () {
          setTimeout(function () {
            $("#nombre").focus();
          }, 500);
        });

        if (modalTitle === "Registrar Producto") {
          ShowSaveButton();
        }

        return true;
      }
    }
    return false;
  }
  //#endregion

  //#region Función para verificar si los valores de los inputs estan vacios
  function IsEmpty() {
    if (nombre !== "") {
      return false;
    } else if (descripcion !== "") {
      return false;
    } else if (divisa !== "") {
      return false;
    } else if (precioMinorista !== "") {
      return false;
    } else if (precioMayorista !== "") {
      return false;
    } else if (precio !== "") {
      return false;
    } else if (porcentajeMinorista !== "") {
      return false;
    } else if (porcentajeMayorista !== "") {
      return false;
    } else if (stock !== "") {
      return false;
    } else if (ocultar !== false) {
      return false;
    } else if (idCategoria !== "") {
      return false;
    } else if (urlImagen !== "") {
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para verificar si se actualizo al menos un valor de los inputs
  function IsUpdated() {
    if (
      prevNombre.toLowerCase() !== nombre.toLocaleLowerCase() ||
      prevDescripcion !== descripcion ||
      prevDivisa !== divisa ||
      prevPrecioMinorista !== precioMinorista ||
      prevPrecioMayorista !== precioMayorista ||
      prevPrecio !== precio ||
      prevPorcentajeMinorista !== porcentajeMinorista ||
      prevPorcentajeMayorista !== porcentajeMayorista ||
      prevStock !== stock ||
      prevOcultar !== ocultar ||
      prevIdCategoria !== idCategoria ||
      prevUrlImagen !== urlImagen
    ) {
      return true;
    }
    return false;
  }
  //#endregion

  //#region Función para insertar un producto
  async function SaveProduct(event) {
    event.preventDefault();

    const btnSave = document.getElementById("btn-save");
    const divBtnSave = document.getElementById("div-btn-save");
    btnSave.style.pointerEvents = "none";
    btnSave.style.opacity = "0.5";
    divBtnSave.style.cursor = "wait";

    const isValid = rolUsuario === "Vendedor" ? IsValidVendedor() : IsValid();
    const isRepeated = IsRepeated();

    if (isValid && !isRepeated) {
      try {
        const { imageUrl, imageId } = await uploadImage();
        if (imageUrl != null) {
          const responseProductPromise = SaveProducts(
            {
              nombre: `${nombre.charAt(0).toUpperCase() + nombre.slice(1)}`,
              descripcion: descripcion,
              idDivisa: rolUsuario === "Vendedor" ? 2 : divisa,
              precio: rolUsuario === "Vendedor" ? 0 : precio,
              porcentajeMinorista:
                rolUsuario === "Vendedor" ? 0 : porcentajeMinorista,
              porcentajeMayorista:
                rolUsuario === "Vendedor" ? 0 : porcentajeMayorista,
              precioMinorista: rolUsuario === "Vendedor" ? 0 : precioMinorista,
              precioMayorista: rolUsuario === "Vendedor" ? 0 : precioMayorista,
              stock: rolUsuario === "Vendedor" ? 0 : stock,
              idCategoria: idCategoria,
              idImagen: imageId,
              urlImagen: imageUrl,
              ocultar: rolUsuario === "Vendedor" ? true : ocultar,
            },
            headers
          );

          responseProductPromise.then((responseProduct) => {
            if (rolUsuario !== "Vendedor") {
              SaveStockDetail(
                {
                  accion: "Agregar",
                  cantidad: stock,
                  motivo: "Creacion de stock de producto",
                  idProducto: responseProduct.data.idProducto,
                },
                headers
              );
            }
          });

          Swal.fire({
            icon: "success",
            title: "Producto registrado exitosamente!",
            showConfirmButton: false,
            timer: 2000,
          });
          CloseModal();
          ShowSaveButton();
          InitialState();
          await GetProductsManage(setProducts);
          await GetProductsManage(setOriginalProductsList);
          setTitle("Detalles de Productos");
          setQuery("");
          document.getElementById("clear-filter").style.display = "none";
          document.getElementById("clear-filter2").style.display = "none";
        }
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

  //#region Función para verificar si el valor ingresado a traves del input Otro es correcto
  function IsValidOtro() {
    if (motivo === "Otro" && otro === "") {
      Swal.fire({
        icon: "error",
        title: "Debe especificar el motivo",
        text: "Complete el campo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      }).then(function () {
        setTimeout(function () {
          $("#otro1").focus();
          $("#otro").focus();
        }, 500);
      });
      return false;
    }
    return true;
  }
  //#endregion

  //#region Función para actualizar un producto ya existente
  async function UpdateProduct(event) {
    let updatedImage = urlImagen; // Variable de estado para almacenar la URL de la imagen
    let updatedImageId = idImagen; // Variable de estado para almacenar la URL de la imagen

    event.preventDefault();

    if (urlImagen !== prevUrlImagen) {
      const { imageUrl, imageId } = await uploadImage();

      updatedImageId = imageId;
      updatedImage = imageUrl; // Actualiza la variable de estado con la nueva URL de la imagen
    }

    if (IsUpdated() === false) {
      Swal.fire({
        icon: "error",
        title: "No puede actualizar el producto sin modificar ningun campo",
        text: "Modifique al menos un campo para poder actualizarlo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#F27474",
      });
    } else if (
      IsValid() === true &&
      IsUpdated() === true &&
      IsRepeated() === false
    ) {
      try {
        await UpdateProducts(
          products.find((u) => u.idProducto === idProducto).idProducto ||
            idProducto,
          {
            idProducto: idProducto,
            nombre: `${nombre.charAt(0).toUpperCase() + nombre.slice(1)}`,
            descripcion: descripcion,
            idDivisa: divisa,
            precio: precio,
            porcentajeMinorista: porcentajeMinorista,
            porcentajeMayorista: porcentajeMayorista,
            precioMinorista: precioMinorista,
            precioMayorista: precioMayorista,
            idCategoria: idCategoria,
            idImagen: updatedImageId,
            urlImagen: updatedImage,
            ocultar: ocultar,
          },
          headers
        );
        Swal.fire({
          icon: "success",
          title: "Producto actualizado exitosamente!",
          showConfirmButton: false,
          timer: 2000,
        });
        CloseModal();

        // InitialState();
        ClearProductInputs();
        await GetProductsManage(setProducts);
        GetCategoriesManage(setCategories);

        setProducts((prevProducts) => {
          setOriginalProductsList(prevProducts);

          if (filterType === "category") {
            const result = prevProducts.filter((product) => {
              return product.nombreCategoria === filterName;
            });

            setTitle(`Detalles de Productos de ${filterName}`);
            setProducts(result);
            setQuery("");
            document.getElementById("clear-filter").style.display = "flex";
            document.getElementById("clear-filter2").style.display = "flex";
            setFilterName(filterName);
            setFilterType("category");
            ClearBold();
            setCurrentPage(1);
          }

          if (filterType === "hidden") {
            const result = prevProducts.filter((product) => {
              return product.ocultar === true;
            });
            setTitle("Detalles de Productos ocultos");
            setProducts(result);
            setQuery("");
            document.getElementById("clear-filter").style.display = "flex";
            document.getElementById("clear-filter2").style.display = "flex";
            setFilterName("Oculto");
            setFilterType("hidden");
            ClearBold();
            setCurrentPage(1);
          }

          if (filterType === "search") {
            const result = prevProducts.filter((product) => {
              return product.nombre
                .toLowerCase()
                .includes(filterName.toLowerCase());
            });
            setProducts(result);
            document.getElementById("clear-filter").style.display = "flex";
            document.getElementById("clear-filter2").style.display = "flex";
            setTitle(
              `Detalles de Productos con: "${
                filterName.charAt(0).toUpperCase() + filterName.slice(1)
              }"`
            );
            setFilterName(
              filterName.charAt(0).toUpperCase() + filterName.slice(1)
            );
            setFilterType("search");
            ClearBold();
            setCurrentPage(1);
            if (filterName === "") {
              document.getElementById("clear-filter").style.display = "none";
              document.getElementById("clear-filter2").style.display = "none";
              setFilterType("");
              setTitle("Detalles de Productos");
            }
          }
          if (filterType === "other") {
            setProducts(prevProducts);
          } else {
            return prevProducts;
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

  //#region Función para actualizar el stock de un producto ya existente
  async function UpdateProductStock(event) {
    event.preventDefault();

    if (!IsUpdated()) {
      Swal.fire({
        icon: "error",
        title: "No puede actualizar el stock del producto sin modificarlo",
        text: "Modifique el stock para poder actualizarlo",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#F27474",
      }).then(function () {
        setTimeout(function () {
          $("#unidadesAgregar").focus();
          $("#unidadesQuitar").focus();
        }, 500);
      });
    } else if (
      (unidadesQuitar !== "" || unidadesAgregar !== "") &&
      IsValidOtro()
    ) {
      try {
        await Promise.all([
          UpdateProductsStock(
            products.find((u) => u.idProducto === idProducto).idProducto ||
              idProducto,
            {
              idProducto: idProducto,
              stock: stock,
            },
            headers
          ),

          SaveStockDetail(
            {
              accion: accion,
              cantidad: cantidad,
              motivo:
                (otro && otro.trim() !== ""
                  ? otro.trim()
                  : motivo && motivo.trim() !== ""
                  ? motivo.trim()
                  : "-"
                )
                  .charAt(0)
                  .toUpperCase() +
                (otro && otro.trim() !== ""
                  ? otro.trim()
                  : motivo && motivo.trim() !== ""
                  ? motivo.trim()
                  : "-"
                ).slice(1),
              idProducto: idProducto,
            },
            headers
          ),
        ]);

        Swal.fire({
          icon: "success",
          title: "Stock del producto actualizado exitosamente!",
          showConfirmButton: false,
          timer: 2000,
        });

        ClearProductInputs();
        CloseModalQuitar();
        CloseModalAgregar();
        GetProductsManage(setProducts);
        GetProductsManage(setOriginalProductsList);
      } catch (error) {
        Swal.fire({
          title: error,
          icon: "error",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#f27474",
        });
      }
    }
  }
  //#endregion

  //#region Función para eliminar un producto existente
  async function DeleteProduct(id) {
    try {
      let resultado = await DeleteProducts(id, headers);
      if (resultado.data.statusCode === 400) {
        Swal.fire({
          icon: "error",
          title:
            "No puede eliminar este producto ya que el mismo se encuentra seleccionado dentro de uno o mas detalles de pedido",
          text: "Primero debera eliminar el/los detalles de pedido que contienen el producto que desea eliminar",
          confirmButtonText: "Aceptar",
          confirmButtonColor: "#f27474",
        });
      } else {
        Swal.fire({
          icon: "success",
          title: "Producto eliminado exitosamente!",
          showConfirmButton: false,
          timer: 2000,
        });
        InitialState();
        ClearFilter();
      }
    } catch (error) {
      Swal.fire({
        icon: "error",
        title: error,
        text: error.message,
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#f27474",
      });
    }
  }
  //#endregion

  //#region Return
  return (
    <div>
      <Helmet>
        <title>Catapp | Administrar Productos</title>
      </Helmet>

      <section id="products" className="general-container">
        <div className="general-content">
          <div className="general-title">
            <div className="title-header">
              <Link to="/panel" className="btn btn-info btn-back">
                <div className="btn-back-content">
                  <Back className="back" />
                  <p className="p-back">Regresar</p>
                </div>
              </Link>

              <h2 className="title title-general">{title}</h2>
              {isLoading === false && (
                <button
                  type="button"
                  className="btn btn-success btn-add"
                  hidden={categories.length === 0}
                  data-bs-toggle="modal"
                  data-bs-target="#modal"
                  onClick={() => {
                    ClearProductInputs();
                    setModalTitle("Registrar Producto");
                    setTimeout(function () {
                      $("#nombre").focus();
                    }, 500);
                    setOcultar(false);
                    setTipoPrecioMinorista("");
                    setTipoPrecioMayorista("");
                  }}
                >
                  <div className="btn-add-content">
                    <Add className="add" />
                    <p className="p-add">Añadir</p>
                  </div>
                </button>
              )}
            </div>

            {categories.length === 0 && isLoading === false && (
              <Link
                to="/administrar-categorias"
                className="btn btn-add-disabled"
              >
                Para añadir un producto, agregue al menos una categoría.
              </Link>
            )}

            {isLoading === false &&
              (products.length > 1 || products.length === 0 ? (
                <p className="total">Hay {products.length} productos.</p>
              ) : (
                <p className="total">Hay {products.length} producto.</p>
              ))}
          </div>

          {/* modal con el formulario para registrar un producto */}
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
                    {modalTitle}
                  </h1>
                </div>
                <div className="modal-body">
                  <div className="container mt-4">
                    <form>
                      <div className="form-group">
                        <input
                          type="text"
                          className="input"
                          id="idProducto"
                          hidden
                          value={idProducto}
                          onChange={(event) => {
                            setIdProducto(event.target.value);
                          }}
                        />

                        <label className="label">Nombre:</label>
                        <div className="form-group-input nombre-input">
                          <span className="input-group-text">
                            <ProductInput className="input-group-svg" />
                          </span>
                          <input
                            type="text"
                            className="input"
                            id="nombre"
                            value={nombre}
                            onChange={(event) => {
                              setNombre(event.target.value);
                            }}
                          />
                        </div>

                        <label className="label">Descripción:</label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <DescriptionInput className="input-group-svg" />
                          </span>
                          <textarea
                            className="input input-textarea"
                            id="descripcion"
                            value={descripcion}
                            onChange={(event) => {
                              setDescripcion(event.target.value);
                            }}
                          />
                        </div>

                        {(rolUsuario === "Supervisor" ||
                          rolUsuario === "SuperAdmin") &&
                          (tipoPrecioMinorista !== "Manual" ||
                            tipoPrecioMayorista !== "Manual") && (
                            <>
                              <label className="label selects" htmlFor="divisa">
                                Divisa:
                              </label>
                              <div
                                className={`form-group-input divisa-input ${
                                  valorDolar > 0 ? "" : " divisa-div"
                                }`}
                              >
                                <span className="input-group-text">
                                  <PriceInput className="input-group-svg" />
                                </span>
                                <select
                                  className="input"
                                  name="divisa"
                                  id="divisa"
                                  value={divisa}
                                  onChange={(e) => setDivisa(e.target.value)}
                                >
                                  <option hidden key={0} value="0">
                                    Seleccione una divisa
                                  </option>
                                  <option
                                    className="btn-option"
                                    value="1"
                                    disabled={valorDolar === 0}
                                  >
                                    Dólar
                                  </option>
                                  <option className="btn-option" value="2">
                                    Peso
                                  </option>
                                </select>
                              </div>
                              <div
                                className="form-group-input divisa-input establecer-cotizacion-div"
                                hidden={valorDolar > 0}
                              >
                                <Link
                                  to="/administrar-cotizacion"
                                  target="_blank"
                                  rel="noopener noreferrer"
                                  className="forgot-password"
                                >
                                  Establecer Cotización Dólar
                                </Link>
                              </div>
                            </>
                          )}

                        {divisa !== "" &&
                          (tipoPrecioMinorista !== "Manual" ||
                            tipoPrecioMayorista !== "Manual") &&
                          (rolUsuario === "Supervisor" ||
                            rolUsuario === "SuperAdmin") && (
                            <>
                              <label className="label">Costo:</label>
                              <div className="form-group-input desc-input">
                                <span className="input-group-text">
                                  {divisa == 1 ? (
                                    <DolarInput className="input-group-svg-divisa" />
                                  ) : divisa == 2 ? (
                                    <PesoInput className="input-group-svg-divisa" />
                                  ) : (
                                    <PriceInput className="input-group-svg" />
                                  )}
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
                            </>
                          )}

                        {(rolUsuario === "Supervisor" ||
                          rolUsuario === "SuperAdmin") && (
                          <>
                            <label
                              className="label selects"
                              htmlFor="tiposPrecio"
                            >
                              Tipo precio minorista:
                            </label>
                            <div className="form-group-input tipoPrecioMinorista-input divisa-input">
                              <span className="input-group-text">
                                <TypePriceInput className="input-group-svg" />
                              </span>
                              <select
                                className="input"
                                name="tiposPrecio"
                                id="tiposPrecio"
                                value={tipoPrecioMinorista}
                                onChange={(e) => {
                                  const selectedValue = e.target.value;
                                  setTipoPrecioMinorista(selectedValue);

                                  if (selectedValue === "Manual") {
                                    setPorcentajeMinorista(0);
                                  } else if (selectedValue === "Porcentual") {
                                    setPrecioMinorista(0);
                                  }

                                  if (
                                    selectedValue === "Manual" &&
                                    tipoPrecioMayorista === "Manual" &&
                                    precio === ""
                                  ) {
                                    setPrecio(0);
                                  }
                                }}
                              >
                                <option hidden key={0} value="0">
                                  Seleccione una tipo de precio minorista
                                </option>
                                <option className="btn-option" value="Manual">
                                  Manual (Pesos)
                                </option>
                                <option
                                  className="btn-option"
                                  value="Porcentual"
                                >
                                  Porcentual
                                </option>
                              </select>
                            </div>
                          </>
                        )}

                        {tipoPrecioMinorista === "Manual" &&
                          (rolUsuario === "Supervisor" ||
                            rolUsuario === "SuperAdmin") && (
                            <>
                              <label className="label">
                                Precio Minorista (manual):
                              </label>
                              <div className="form-group-input desc-input">
                                <span className="input-group-text">
                                  <PesoInput className="input-group-svg-divisa" />
                                </span>
                                <input
                                  type="number"
                                  step="1"
                                  min={0}
                                  className="input"
                                  id="precioMinorista"
                                  value={precioMinorista}
                                  onChange={(event) => {
                                    setPrecioMinorista(event.target.value);
                                  }}
                                />
                              </div>
                            </>
                          )}

                        {tipoPrecioMinorista === "Porcentual" &&
                          (rolUsuario === "Supervisor" ||
                            rolUsuario === "SuperAdmin") && (
                            <>
                              <label className="label">
                                Porcentaje de ganancia minorista:
                              </label>
                              <div className="form-group-input desc-input">
                                <span className="input-group-text">
                                  <PercentageInput className="input-group-svg" />
                                </span>
                                <input
                                  type="number"
                                  step="1"
                                  min={0}
                                  className="input"
                                  id="porcentajeMinorista"
                                  value={porcentajeMinorista}
                                  onChange={(event) => {
                                    setPorcentajeMinorista(event.target.value);
                                  }}
                                />
                              </div>
                            </>
                          )}

                        {(rolUsuario === "Supervisor" ||
                          rolUsuario === "SuperAdmin") && (
                          <>
                            <label
                              className="label selects"
                              htmlFor="tiposPrecio2"
                            >
                              Tipo precio mayorista:
                            </label>
                            <div className="form-group-input tipoPrecioMinorista-input divisa-input">
                              <span className="input-group-text">
                                <TypePriceInput className="input-group-svg" />
                              </span>
                              <select
                                className="input"
                                name="tiposPrecio2"
                                id="tiposPrecio2"
                                value={tipoPrecioMayorista}
                                onChange={(e) => {
                                  const selectedValue = e.target.value;
                                  setTipoPrecioMayorista(selectedValue);

                                  if (selectedValue === "Manual") {
                                    setPorcentajeMayorista(0);
                                  } else if (selectedValue === "Porcentual") {
                                    setPrecioMayorista(0);
                                  }

                                  if (
                                    selectedValue === "Manual" &&
                                    tipoPrecioMinorista === "Manual" &&
                                    precio === ""
                                  ) {
                                    setPrecio(0);
                                  }
                                }}
                              >
                                <option hidden key={0} value="0">
                                  Seleccione una tipo de precio mayorista
                                </option>
                                <option className="btn-option" value="Manual">
                                  Manual (Pesos)
                                </option>
                                <option
                                  className="btn-option"
                                  value="Porcentual"
                                >
                                  Porcentual
                                </option>
                              </select>
                            </div>
                          </>
                        )}

                        {tipoPrecioMayorista === "Manual" && (
                          <>
                            <label className="label">
                              Precio Mayorista (manual):
                            </label>
                            <div className="form-group-input desc-input">
                              <span className="input-group-text">
                                <PesoInput className="input-group-svg-divisa" />
                              </span>
                              <input
                                type="number"
                                step="1"
                                min={0}
                                className="input"
                                id="precioMayorista"
                                value={precioMayorista}
                                onChange={(event) => {
                                  setPrecioMayorista(event.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}

                        {tipoPrecioMayorista === "Porcentual" &&
                          (rolUsuario === "Supervisor" ||
                            rolUsuario === "SuperAdmin") && (
                            <>
                              <label className="label">
                                Porcentaje de ganancia mayorista:
                              </label>
                              <div className="form-group-input desc-input">
                                <span className="input-group-text">
                                  <PercentageInput className="input-group-svg" />
                                </span>
                                <input
                                  type="number"
                                  step="1"
                                  min={0}
                                  className="input"
                                  id="porcentajeMayorista"
                                  value={porcentajeMayorista}
                                  onChange={(event) => {
                                    setPorcentajeMayorista(event.target.value);
                                  }}
                                />
                              </div>
                            </>
                          )}

                        {modalTitle === "Registrar Producto" &&
                          (rolUsuario === "Supervisor" ||
                            rolUsuario === "SuperAdmin") && (
                            <>
                              <label className="label">Stock:</label>
                              <div className="form-group-input desc-input">
                                <span className="input-group-text">
                                  <StockInput className="input-group-svg" />
                                </span>
                                <input
                                  type="number"
                                  step="1"
                                  min={0}
                                  className="input"
                                  id="stock"
                                  value={stock}
                                  onChange={(event) => {
                                    setStock(event.target.value);
                                  }}
                                />
                              </div>
                            </>
                          )}

                        <label className="label selects" htmlFor="categorias">
                          Categoria:
                        </label>
                        <div className="form-group-input">
                          <span className="input-group-text">
                            <CategoryInput className="input-group-svg" />
                          </span>
                          <select
                            className="input"
                            name="categorias"
                            id="categorias"
                            value={idCategoria}
                            onChange={(e) => setIdCategoria(e.target.value)}
                          >
                            <option hidden key={0} value="0">
                              Seleccione una categoría
                            </option>
                            {Array.from(categories).map((opts, i) => (
                              <option
                                className="btn-option"
                                key={i}
                                value={opts.idCategoria}
                              >
                                {opts.nombre}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>

                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <>
                          <div className="form-group oculto">
                            <label className="label">Ocultar</label>
                            <input
                              type="checkbox"
                              className="form-check-input tick"
                              id="ocultar"
                              checked={ocultar}
                              onChange={() => {
                                setOcultar(checkbox.checked);
                              }}
                            />
                            <label
                              htmlFor="ocultar"
                              className="lbl-switch"
                            ></label>
                          </div>
                        </>
                      )}

                      <div className="form-group">
                        <label className="label">Imagen:</label>
                        <div className="form-group-input">
                          <span className="input-group-text">
                            <ImageInput className="input-group-svg" />
                          </span>
                          <input
                            id="image-url"
                            type="file"
                            className="input-file"
                            accept="image/*"
                            onChange={(event) => {
                              setImageSelected(event.target.files[0]);

                              setUrlImagen(event.target.value);
                            }}
                          />
                        </div>
                      </div>
                      <div>
                        {modalTitle === "Registrar Producto" ? (
                          <div id="div-btn-save">
                            <button
                              className="btn btn-success btnadd"
                              id="btn-save"
                              onClick={SaveProduct}
                            >
                              <div className="btn-save-update-close">
                                <Save className="save-btn" />
                                <p className="p-save-update-close">Guardar</p>
                              </div>
                            </button>
                          </div>
                        ) : (
                          <div id="div-btn-update">
                            <button
                              className="btn btn-warning btn-edit-color"
                              id="btn-update"
                              onClick={UpdateProduct}
                            >
                              <div className="btn-save-update-close">
                                <Update className="update-btn" />
                                <p className="p-save-update-close">
                                  Actualizar
                                </p>
                              </div>
                            </button>
                          </div>
                        )}
                      </div>
                    </form>
                  </div>
                </div>
                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => {
                      if (modalTitle === "Registrar Producto") {
                        if (IsEmpty() === true) {
                          ClearProductInputs();
                          CloseModal();
                        } else {
                          Swal.fire({
                            icon: "warning",
                            title:
                              "¿Está seguro de que desea cerrar el formulario?",
                            text: "Se perderán todos los datos cargados",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            cancelButtonText: "Cancelar",
                            confirmButtonColor: "#f8bb86",
                            cancelButtonColor: "#6c757d",
                          }).then((result) => {
                            if (result.isConfirmed) {
                              ClearProductInputs();
                              CloseModal();
                            }
                          });
                        }
                      } else if (modalTitle === "Actualizar Producto") {
                        if (IsUpdated() === false) {
                          ClearProductInputs();
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
                              ClearProductInputs();
                              CloseModal();
                            }
                          });
                        }
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

          {/* modal con el formulario para quitar stock de un producto */}
          <div
            className="modal fade"
            id="modalQuitar"
            data-bs-backdrop="static"
            data-bs-keyboard="false"
            tabIndex="-1"
            aria-labelledby="staticBackdropLabel"
            aria-hidden="true"
          >
            <div className="modal-dialog">
              <div className="modal-content">
                <div className="modal-header">
                  <h1 className="modal-title quitar" id="exampleModalLabel">
                    Quitar unidades de stock
                  </h1>
                </div>
                <div className="modal-body">
                  <div className="container mt-4">
                    <form>
                      <div className="form-group">
                        <label className="label">
                          Cantidad de unidades para quitar:
                        </label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <Quitar className="input-group-svg" />
                          </span>
                          <input
                            type="number"
                            step="1"
                            min={0}
                            className="input"
                            id="unidadesQuitar"
                            value={unidadesQuitar}
                            onChange={handleUnidadesQuitarChange} // Actualizar el stock al cambiar las unidades
                          />
                        </div>

                        <label className="label">Stock modificado:</label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <StockInput className="input-group-svg" />
                          </span>
                          <input
                            type="number"
                            style={{
                              backgroundColor: "#d3d3d3",
                              cursor: "default",
                            }}
                            step="1"
                            min={0}
                            className="input"
                            id="stock"
                            value={stock}
                            onChange={(event) => {
                              setStock(event.target.value);
                            }}
                            readOnly
                          />
                        </div>

                        <label className="label selects" htmlFor="motivo">
                          Motivo:
                        </label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <MotivoInput className="input-group-svg" />
                          </span>
                          <select
                            className="input"
                            name="motivos"
                            id="motivo"
                            value={motivo}
                            onChange={(e) => {
                              setMotivo(e.target.value);
                            }}
                          >
                            <option hidden key={0} value="0">
                              Seleccione un motivo (Opcional)
                            </option>
                            <option value="Venta/Pedido">Venta/Pedido</option>
                            <option value="Devolucion">Devolucion</option>
                            <option value="Falla">Falla</option>
                            <option value="Pérdida">Pérdida</option>
                            <option value="Robo">Robo</option>
                            <option value="Traslado">Traslado</option>
                            <option value="Obsolescencia">Obsolescencia</option>
                            <option value="Otro">Otro</option>
                          </select>
                        </div>

                        {motivo === "Otro" && (
                          <>
                            <label className="label">Otro:</label>
                            <div className="form-group-input">
                              <span className="input-group-text">
                                <OtroInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="otro"
                                value={otro}
                                onChange={(e) => {
                                  setOtro(e.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}
                      </div>

                      <div id="div-btn-update">
                        <button
                          className="btn btn-warning btn-edit-color"
                          id="btn-update"
                          onClick={UpdateProductStock}
                        >
                          <div className="btn-save-update-close">
                            <Update className="update-btn" />
                            <p className="p-save-update-close">Actualizar</p>
                          </div>
                        </button>
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
                        CloseModalQuitar();
                        CloseModalAgregar();
                        ClearProductInputs();
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
                            CloseModalQuitar();
                            CloseModalAgregar();
                            ClearProductInputs();
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
                    id="btn-close-modal-quitar"
                    data-bs-dismiss="modal"
                  ></button>
                </div>
              </div>
            </div>
          </div>

          {/* modal con el formulario para agregar stock de un producto */}
          <div
            className="modal fade"
            id="modalAgregar"
            data-bs-backdrop="static"
            data-bs-keyboard="false"
            tabIndex="-1"
            aria-labelledby="staticBackdropLabel"
            aria-hidden="true"
          >
            <div className="modal-dialog">
              <div className="modal-content">
                <div className="modal-header">
                  <h1 className="modal-title agregar" id="exampleModalLabel">
                    Agregar unidades de stock
                  </h1>
                </div>
                <div className="modal-body">
                  <div className="container mt-4">
                    <form>
                      <div className="form-group">
                        <label className="label">
                          Cantidad de unidades para agregar:
                        </label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <Agregar className="input-group-svg" />
                          </span>
                          <input
                            type="number"
                            step="1"
                            min={0}
                            className="input"
                            id="unidadesAgregar"
                            value={unidadesAgregar}
                            onChange={handleUnidadesAgregarChange} // Actualizar el stock al cambiar las unidades
                          />
                        </div>

                        <label className="label">Stock modificado:</label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <StockInput className="input-group-svg" />
                          </span>
                          <input
                            type="number"
                            style={{
                              backgroundColor: "#d3d3d3",
                              cursor: "default",
                            }}
                            step="1"
                            min={0}
                            className="input"
                            id="stock"
                            value={stock}
                            onChange={(event) => {
                              setStock(event.target.value);
                            }}
                            readOnly
                          />
                        </div>

                        <label className="label selects" htmlFor="motivo">
                          Motivo:
                        </label>
                        <div className="form-group-input desc-input">
                          <span className="input-group-text">
                            <MotivoInput className="input-group-svg" />
                          </span>
                          <select
                            className="input"
                            name="motivos"
                            id="motivo"
                            value={motivo}
                            onChange={(e) => {
                              setMotivo(e.target.value);
                            }}
                          >
                            <option hidden key={0} value="0">
                              Seleccione un motivo (Opcional)
                            </option>
                            <option value="Compra">Compra</option>
                            <option value="Reabastecimiento">
                              Reabastecimiento
                            </option>
                            <option value="Traslado">Traslado</option>
                            <option value="Otro">Otro</option>
                          </select>
                        </div>

                        {motivo === "Otro" && (
                          <>
                            <label className="label">Otro:</label>
                            <div className="form-group-input">
                              <span className="input-group-text">
                                <OtroInput className="input-group-svg" />
                              </span>
                              <input
                                type="text"
                                className="input"
                                id="otro1"
                                value={otro}
                                onChange={(e) => {
                                  setOtro(e.target.value);
                                }}
                              />
                            </div>
                          </>
                        )}
                      </div>

                      <div id="div-btn-update">
                        <button
                          className="btn btn-warning btn-edit-color"
                          id="btn-update"
                          onClick={UpdateProductStock}
                        >
                          <div className="btn-save-update-close">
                            <Update className="update-btn" />
                            <p className="p-save-update-close">Actualizar</p>
                          </div>
                        </button>
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
                        CloseModalQuitar();
                        CloseModalAgregar();
                        ClearProductInputs();
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
                            CloseModalQuitar();
                            CloseModalAgregar();
                            ClearProductInputs();
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
                    id="btn-close-modal-agregar"
                    data-bs-dismiss="modal"
                  ></button>
                </div>
              </div>
            </div>
          </div>

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
                      onClick={handleCategoryClick}
                      data-bs-toggle="collapse"
                      href="#collapseCategory"
                      role="button"
                      aria-expanded="false"
                      aria-controls="collapseCategory"
                    >
                      <p className="filter-btn-name">CATEGORÍAS</p>
                      <p className="filter-btn">{categorySign}</p>
                    </div>
                    <div className="collapse" id="collapseCategory">
                      <div className="card-collapse">
                        {categories.map((category, index) => {
                          return (
                            <p
                              key={index}
                              id="items-collapse"
                              className={`items-collapse ${
                                category.isActive ? "active" : ""
                              }`}
                              onClick={() => {
                                filterResultCategory(category.nombre);
                                setCategories((prevState) =>
                                  prevState.map((cat) =>
                                    cat.nombre === category.nombre
                                      ? { ...cat, isActive: !cat.isActive }
                                      : cat.nombre !== category.nombre &&
                                        cat.isActive
                                      ? { ...cat, isActive: false }
                                      : cat
                                  )
                                );
                              }}
                            >
                              {category.nombre}
                            </p>
                          );
                        })}
                      </div>
                    </div>

                    <p className="filter-separator"></p>

                    <div className="filter-btn-container">
                      <p className="filter-btn-name">OCULTO</p>
                      <p className="filter-btn">
                        <input
                          type="checkbox"
                          className="form-check-input tick"
                          id="hidden"
                          checked={hidden}
                          onChange={() => {
                            setHidden(!hidden);
                            filterResultHidden(hidden);
                          }}
                        />
                        <label htmlFor="hidden" className="lbl-switch"></label>
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

          {products.length > 0 && (
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
          )}

          {/* tabla de productos */}
          {isLoading ? (
            <div className="loading-generaltable-div">
              <Loader />
              <p className="bold-loading">Cargando productos...</p>
            </div>
          ) : (
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
                    Nombre
                  </th>
                  <th className="table-title" scope="col">
                    Descripción
                  </th>
                  {(rolUsuario === "Supervisor" ||
                    rolUsuario === "SuperAdmin") && (
                    <th className="table-title" scope="col">
                      Divisa
                    </th>
                  )}
                  {(rolUsuario === "Supervisor" ||
                    rolUsuario === "SuperAdmin") && (
                    <th className="table-title" scope="col">
                      Costo
                    </th>
                  )}

                  <th className="table-title porc-text" scope="col">
                    <div className="porc-btn-container">
                      <Link
                        to="/catalogo-minorista"
                        target="_blank"
                        rel="noopener noreferrer"
                        className="btn btn-secondary btn-delete3"
                        aria-label="Catalogo Minorista"
                      >
                        <Page className="edit2" />
                      </Link>
                      {rolUsuario === "Vendedor"
                        ? "$ Minorista"
                        : "% Minorista"}
                    </div>
                  </th>

                  <th className="table-title porc-text" scope="col">
                    <div className="porc-btn-container">
                      <Link
                        to="/catalogo-mayorista"
                        target="_blank"
                        rel="noopener noreferrer"
                        className="btn btn-secondary btn-delete3"
                        aria-label="Catalogo Matorista"
                      >
                        <Page className="edit2" />
                      </Link>
                      {rolUsuario === "Vendedor"
                        ? "$ Mayorista"
                        : "% Mayorista"}
                    </div>
                  </th>

                  <th className="table-title" scope="col">
                    Stock
                  </th>
                  <th className="table-title" scope="col">
                    Categoría
                  </th>
                  <th className="table-title" scope="col">
                    Oculto
                  </th>
                  <th className="table-title" scope="col">
                    Imagen
                  </th>
                  <th className="table-title" scope="col">
                    Acciones
                  </th>
                </tr>
              </thead>

              {products.length > 0 ? (
                productsTable.map(function fn(product, index) {
                  return (
                    <tbody key={1 + product.idProducto}>
                      <tr>
                        <th
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                          scope="row"
                        >
                          {index + 1}
                        </th>
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          {product.idProducto}{" "}
                        </td>
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          {product.nombre}
                        </td>
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-overflow table-name"
                              : product.ocultar
                              ? "hidden-product table-overflow table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-overflow table-name"
                              : "table-name table-overflow"
                          }
                        >
                          {product.descripcion}
                        </td>
                        {/* <td className={product.stockTransitorio === 0 ? "zero-stock" : "table-name"}>{product.divisa}</td> */}

                        {(rolUsuario === "Supervisor" ||
                          rolUsuario === "SuperAdmin") && (
                          <td
                            className={
                              product.ocultar && product.stockTransitorio === 0
                                ? "zero-stock-hidden table-name"
                                : product.ocultar
                                ? "hidden-product table-name"
                                : product.stockTransitorio === 0
                                ? "zero-stock table-name"
                                : "table-name"
                            }
                          >
                            {/* {(tipoPrecioMinorista !== "Manual" || tipoPrecioMayorista !== "Manual") && product.divisa === "Peso" ? ( */}
                            {product.divisa === "Peso" ||
                            (product.precioMinorista !== 0 &&
                              product.precioMayorista !== 0) ? (
                              <PesoInput className="input-group-svg-divisa-big" />
                            ) : product.divisa == "Dólar" ? (
                              <DolarInput className="input-group-svg-divisa-big" />
                            ) : (
                              <div>No hay SVG disponible</div>
                            )}
                          </td>
                        )}

                        {(rolUsuario === "Supervisor" ||
                          rolUsuario === "SuperAdmin") && (
                          <td
                            className={
                              product.ocultar && product.stockTransitorio === 0
                                ? "zero-stock-hidden table-name"
                                : product.ocultar
                                ? "hidden-product table-name"
                                : product.stockTransitorio === 0
                                ? "zero-stock table-name"
                                : "table-name"
                            }
                          >
                            {/* Condición para verificar si tipoPrecioMinorista o tipoPrecioMayorista no son "Manual" */}
                            {product.precioMinorista !== 0 &&
                            product.precioMayorista !== 0
                              ? "-"
                              : `$${product.precio
                                  .toLocaleString("es-ES", {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 2,
                                  })
                                  .replace(",", ".")
                                  .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}`}
                          </td>
                        )}
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          {product.precioMinorista > 0 ? (
                            <div>
                              {rolUsuario === "Supervisor" ||
                              rolUsuario === "SuperAdmin" ? (
                                <>
                                  <PesoInput className="input-group-svg-divisa-big2" />
                                  $
                                  {product.precioMinorista
                                    .toLocaleString("es-ES", {
                                      minimumFractionDigits: 0,
                                      maximumFractionDigits: 2,
                                    })
                                    .replace(",", ".")
                                    .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}{" "}
                                  (manual)
                                </>
                              ) : (
                                <>
                                  $
                                  {product.precioMinorista
                                    .toLocaleString("es-ES", {
                                      minimumFractionDigits: 0,
                                      maximumFractionDigits: 2,
                                    })
                                    .replace(",", ".")
                                    .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                </>
                              )}
                            </div>
                          ) : product.porcentajeMinorista > 0 ? (
                            <div>
                              {product.divisa === "Peso" ? (
                                <div>
                                  {(rolUsuario === "Supervisor" ||
                                    rolUsuario === "SuperAdmin") && (
                                    <>
                                      {product.porcentajeMinorista.toLocaleString()}
                                      %
                                      <br />
                                    </>
                                  )}

                                  <p
                                    className={
                                      rolUsuario === "Supervisor" ||
                                      rolUsuario === "SuperAdmin"
                                        ? "precio-pesos"
                                        : ""
                                    }
                                  >
                                    $
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMinorista /
                                              100)) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : product.divisa == "Dólar" ? (
                                <div>
                                  {(rolUsuario === "Supervisor" ||
                                    rolUsuario === "SuperAdmin") && (
                                    <>
                                      {product.porcentajeMinorista.toLocaleString()}
                                      %
                                      <br />
                                    </>
                                  )}
                                  <p
                                    className={
                                      rolUsuario === "Supervisor" ||
                                      rolUsuario === "SuperAdmin"
                                        ? "precio-pesos"
                                        : ""
                                    }
                                  >
                                    $
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMinorista / 100) *
                                          valorDolar) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : (
                                <div>-</div>
                              )}
                            </div>
                          ) : (
                            <div>-</div>
                          )}
                        </td>

                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          {product.precioMayorista > 0 ? (
                            <div>
                              {rolUsuario === "Supervisor" ||
                              rolUsuario === "SuperAdmin" ? (
                                <>
                                  <PesoInput className="input-group-svg-divisa-big2" />
                                  $
                                  {product.precioMayorista
                                    .toLocaleString("es-ES", {
                                      minimumFractionDigits: 0,
                                      maximumFractionDigits: 2,
                                    })
                                    .replace(",", ".")
                                    .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}{" "}
                                  (manual)
                                </>
                              ) : (
                                <>
                                  $
                                  {product.precioMayorista
                                    .toLocaleString("es-ES", {
                                      minimumFractionDigits: 0,
                                      maximumFractionDigits: 2,
                                    })
                                    .replace(",", ".")
                                    .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                </>
                              )}
                            </div>
                          ) : product.porcentajeMayorista > 0 ? (
                            <div>
                              {product.divisa === "Peso" ? (
                                <div>
                                  {(rolUsuario === "Supervisor" ||
                                    rolUsuario === "SuperAdmin") && (
                                    <>
                                      {product.porcentajeMayorista.toLocaleString()}
                                      %
                                      <br />
                                    </>
                                  )}
                                  <p
                                    className={
                                      rolUsuario === "Supervisor" ||
                                      rolUsuario === "SuperAdmin"
                                        ? "precio-pesos"
                                        : ""
                                    }
                                  >
                                    $
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMayorista /
                                              100)) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : product.divisa == "Dólar" ? (
                                <div>
                                  {(rolUsuario === "Supervisor" ||
                                    rolUsuario === "SuperAdmin") && (
                                    <>
                                      {product.porcentajeMayorista.toLocaleString()}
                                      %
                                      <br />
                                    </>
                                  )}
                                  <p
                                    className={
                                      rolUsuario === "Supervisor" ||
                                      rolUsuario === "SuperAdmin"
                                        ? "precio-pesos"
                                        : ""
                                    }
                                  >
                                    $
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMayorista / 100) *
                                          valorDolar) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : (
                                <div>-</div>
                              )}
                            </div>
                          ) : (
                            <div>-</div>
                          )}
                        </td>
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden stock-div table-name"
                              : product.ocultar
                              ? "hidden-product stock-div table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock stock-div table-name"
                              : "table-name stock-div"
                          }
                        >
                          <div className="stock-btns">
                            {(rolUsuario === "Supervisor" ||
                              rolUsuario === "SuperAdmin") && (
                              <button
                                type="button"
                                className="btn btn-danger btn-delete3"
                                aria-label="Quitar"
                                data-bs-toggle="modal"
                                data-bs-target="#modalQuitar"
                                onClick={() => {
                                  RetrieveProductInputs(product);
                                  setOriginalStock(product.stockTransitorio);
                                }}
                              >
                                <Quitar className="edit2" />
                              </button>
                            )}
                            {product.stockTransitorio}
                            {(rolUsuario === "Supervisor" ||
                              rolUsuario === "SuperAdmin") && (
                              <button
                                type="button"
                                className="btn btn-success btn-add2"
                                aria-label="Agregar"
                                data-bs-toggle="modal"
                                data-bs-target="#modalAgregar"
                                onClick={() => {
                                  RetrieveProductInputs(product);
                                  setOriginalStock(product.stockTransitorio);
                                }}
                              >
                                <Agregar className="edit2" />
                              </button>
                            )}
                          </div>
                          <Link
                            to={`/detalles/${product.idProducto}`}
                            title="Ver detalles"
                            type="button"
                            className="btn btn-secondary svg-btn"
                          >
                            <Stock className="svg" />
                          </Link>
                          {product.stock - product.stockTransitorio !== 0 && (
                            <p className="stock-pendiente">
                              {product.stock - product.stockTransitorio}{" "}
                              {product.stock - product.stockTransitorio === 1
                                ? "Pendiente"
                                : "Pendientes"}
                            </p>
                          )}
                        </td>
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          {product.nombreCategoria}
                        </td>
                        {product.ocultar ? (
                          <td
                            className={
                              product.ocultar && product.stockTransitorio === 0
                                ? "zero-stock-hidden table-name"
                                : product.ocultar
                                ? "hidden-product table-name"
                                : product.stockTransitorio === 0
                                ? "zero-stock table-name"
                                : "table-name"
                            }
                          >
                            Si
                          </td>
                        ) : (
                          <td
                            className={
                              product.ocultar && product.stockTransitorio === 0
                                ? "zero-stock-hidden table-name"
                                : product.ocultar
                                ? "hidden-product table-name"
                                : product.stockTransitorio === 0
                                ? "zero-stock table-name"
                                : "table-name"
                            }
                          >
                            No
                          </td>
                        )}
                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          <img
                            src={product.urlImagen}
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
                            className="list-img"
                            alt="Producto"
                          />
                        </td>

                        <td
                          className={
                            product.ocultar && product.stockTransitorio === 0
                              ? "zero-stock-hidden table-name"
                              : product.ocultar
                              ? "hidden-product table-name"
                              : product.stockTransitorio === 0
                              ? "zero-stock table-name"
                              : "table-name"
                          }
                        >
                          <button
                            type="button"
                            className="btn btn-warning btn-edit"
                            aria-label="Modificar"
                            data-bs-toggle="modal"
                            data-bs-target="#modal"
                            onClick={() => {
                              RetrieveProductInputs(product);
                              setModalTitle("Actualizar Producto");
                            }}
                          >
                            <Edit className="edit" />
                          </button>

                          {(rolUsuario === "Supervisor" ||
                            rolUsuario === "SuperAdmin") && (
                            <button
                              type="button"
                              className="btn btn-danger btn-delete"
                              aria-label="Eliminar"
                              onClick={() =>
                                Swal.fire({
                                  title:
                                    "Esta seguro de que desea eliminar el siguiente producto: " +
                                    product.nombre +
                                    "?",
                                  imageUrl: `${product.urlImagen}`,
                                  imageWidth: 200,
                                  imageHeight: 200,
                                  imageAlt: "Producto a eliminar",
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
                                    DeleteProduct(product.idProducto);
                                  }
                                })
                              }
                            >
                              <Delete className="delete" />
                            </button>
                          )}
                        </td>
                      </tr>
                    </tbody>
                  );
                })
              ) : (
                <tbody>
                  <tr className="tr-name1">
                    <td className="table-name table-name1" colSpan={13}>
                      Sin registros
                    </td>
                  </tr>
                </tbody>
              )}
            </table>
          )}

          {/* tabla de productos para excel */}
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
                  Nombre
                </th>
                <th className="table-title" scope="col">
                  Descripción
                </th>
                {(rolUsuario === "Supervisor" ||
                  rolUsuario === "SuperAdmin") && (
                  <th className="table-title" scope="col">
                    Divisa
                  </th>
                )}
                {(rolUsuario === "Supervisor" ||
                  rolUsuario === "SuperAdmin") && (
                  <th className="table-title" scope="col">
                    Costo
                  </th>
                )}
                <th className="table-title" scope="col">
                  {rolUsuario === "Vendedor" ? "$ Minorista" : "% Minorista"}
                </th>
                {(rolUsuario === "Supervisor" ||
                  rolUsuario === "SuperAdmin") && (
                  <th className="table-title" scope="col">
                    Precio Minorista (Manual)
                  </th>
                )}
                <th className="table-title" scope="col">
                  {rolUsuario === "Vendedor" ? "$ Mayorista" : "% Mayorista"}
                </th>
                {(rolUsuario === "Supervisor" ||
                  rolUsuario === "SuperAdmin") && (
                  <th className="table-title" scope="col">
                    Precio Mayorista (Manual)
                  </th>
                )}
                <th className="table-title" scope="col">
                  Stock
                </th>
                <th className="table-title" scope="col">
                  Categoría
                </th>
                <th className="table-title" scope="col">
                  Oculto
                </th>
                <th className="table-title" scope="col">
                  URL Imagen
                </th>
              </tr>
            </thead>

            {products.length > 0 ? (
              products.map(function fn(product) {
                return (
                  <tbody key={1 + product.idProducto}>
                    <tr>
                      <td>{product.idProducto} </td>
                      <td>{product.nombre}</td>
                      <td>{product.descripcion}</td>
                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td>{product.divisa}</td>
                      )}

                      {rolUsuario === "Vendedor" && (
                        <td>
                          {product.precioMinorista > 0 ? (
                            <div>
                              <>{product.precioMinorista.toLocaleString()}</>
                            </div>
                          ) : product.porcentajeMinorista > 0 ? (
                            <div>
                              {product.divisa === "Peso" ? (
                                <div>
                                  <p>
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMinorista /
                                              100)) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : product.divisa == "Dólar" ? (
                                <div>
                                  <p>
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMinorista / 100) *
                                          valorDolar) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : (
                                <div>-</div>
                              )}
                            </div>
                          ) : (
                            <div>-</div>
                          )}
                        </td>
                      )}

                      {rolUsuario === "Vendedor" && (
                        <td>
                          {product.precioMayorista > 0 ? (
                            <div>
                              <>{product.precioMinorista.toLocaleString()}</>
                            </div>
                          ) : product.porcentajeMayorista > 0 ? (
                            <div>
                              {product.divisa === "Peso" ? (
                                <div>
                                  <p>
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMayorista /
                                              100)) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : product.divisa == "Dólar" ? (
                                <div>
                                  <p>
                                    {(
                                      Math.round(
                                        (product.precio *
                                          (1 +
                                            product.porcentajeMayorista / 100) *
                                          valorDolar) /
                                          50
                                      ) * 50
                                    )
                                      .toLocaleString("es-ES", {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2,
                                      })
                                      .replace(",", ".")
                                      .replace(/\B(?=(\d{3})+(?!\d))/g, ".")}
                                  </p>
                                </div>
                              ) : (
                                <div>-</div>
                              )}
                            </div>
                          ) : (
                            <div>-</div>
                          )}
                        </td>
                      )}

                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td>{product.precio.toLocaleString()}</td>
                      )}
                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td>{product.porcentajeMinorista.toLocaleString()}</td>
                      )}
                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td>{product.precioMinorista.toLocaleString()}</td>
                      )}
                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td>{product.porcentajeMayorista.toLocaleString()}</td>
                      )}
                      {(rolUsuario === "Supervisor" ||
                        rolUsuario === "SuperAdmin") && (
                        <td>{product.precioMayorista.toLocaleString()}</td>
                      )}
                      <td>{product.stockTransitorio}</td>
                      <td>{product.nombreCategoria}</td>
                      {product.ocultar ? <td>Si</td> : <td>No</td>}
                      <td>{product.urlImagen}</td>
                    </tr>
                  </tbody>
                );
              })
            ) : (
              <tbody>
                <tr>
                  <td colSpan={11}>Sin registros</td>
                </tr>
              </tbody>
            )}
          </table>

          <div className="pagination-count-container2">
            <div className="pagination-count">
              {products.length > 0 ? (
                products.length === 1 ? (
                  <p className="total">
                    Producto {firstIndex + 1} de {products.length}
                  </p>
                ) : (
                  <p className="total">
                    Productos {firstIndex + 1} a{" "}
                    {productsTable.length + firstIndex} de {products.length}
                  </p>
                )
              ) : (
                <></>
              )}
            </div>

            {products.length > 0 ? (
              <ul className="pagination-manager">
                <li className="page-item">
                  <div className="page-link" onClick={prePage}>
                    {"<"}
                  </div>
                </li>

                <li className="numbers">
                  {numbers.map((n, i) => {
                    if (n === currentPage) {
                      return (
                        <ul className="page-item-container" key={i}>
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
                        <ul className="page-item-container" key={i}>
                          <li className="page-item" key={i}>
                            <div
                              className="page-link"
                              onClick={() => changeCPage(n)}
                            >
                              {n}
                            </div>
                          </li>
                        </ul>
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
                        <ul className="page-item-container" key={i}>
                          <li className="page-item" key={i}>
                            <div className="page-link">...</div>
                          </li>
                        </ul>
                      );
                    } else {
                      return null;
                    }
                  })}
                </li>

                <li className="page-item">
                  <div className="page-link" onClick={nextPage}>
                    {">"}
                  </div>
                </li>
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

export default ProductManager;
