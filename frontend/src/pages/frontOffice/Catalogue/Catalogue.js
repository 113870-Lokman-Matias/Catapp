import React, { useState, useEffect } from "react";
import { Helmet } from "react-helmet";
import Swal from "sweetalert2";

import { GetCategories } from "../../../services/CategoryService";
import {
  GetProductsByCategory,
  GetProducts,
} from "../../../services/ProductService";

//#region Imports de los SVG'S
import { ReactComponent as Zoom } from "../../../assets/svgs/zoom.svg";
import { ReactComponent as Close } from "../../../assets/svgs/closebtn.svg";
import { ReactComponent as Lupa } from "../../../assets/svgs/lupa.svg";
import Loader from "../../../components/Loaders/LoaderCircle";
//#endregion

import "./Catalogue.css";

const Catalogue = () => {
  //#region Constantes
  const [click, setClick] = useState(false);
  const handleClick = () => setClick(!click);
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

  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingProductByCategory, setIsLoadingProductByCategory] =
    useState(true);

  //#region Constantes necesarias para el filtro por busqueda
  const [originalProductsList, setOriginalProductsList] = useState([]);
  const [products, setProducts] = useState([]);
  const [query, setQuery] = useState("");
  //#endregion
  //#endregion

  //#region UseEffect
  useEffect(() => {
    // Si el query de la busqueda está vacío se ocultan el contenedor de filtrado de productos, por lo contrario se muestra el contenedor con las categorias y sus respectivos productos
    if (query === "") {
      document.getElementById("productos-filtrados").style.display = "none";
      document.getElementById("categorias-container").style.display = "flex";
    }

    // Funciones asincronas para obtener las categorias y todos los productos
    (async () => {
      try {
        await GetCategories(setCategories);
        await GetProducts(setOriginalProductsList);
      } catch (error) {
        console.log(error);
      } finally {
        setIsLoading(false);
      }
    })();
  }, [query]);
  //#endregion

  //#region Funcion para filtrar los productos por query
  const search = () => {
    setProducts(originalProductsList);
    const result = originalProductsList.filter(
      (product) =>
        product.nombre.toLowerCase().includes(query.toLowerCase()) ||
        product.descripcion.toLowerCase().includes(query.toLowerCase())
    );
    if (result) {
      document.getElementById("productos-filtrados").style.display = "flex";
      document.getElementById("categorias-container").style.display = "none";
    }
    setProducts(result);
    window.scrollTo(0, 0);

    if (query === "") {
      document.getElementById("productos-filtrados").style.display = "none";
      document.getElementById("categorias-container").style.display = "flex";
      window.scrollTo(0, 0);
    }
  };
  //#endregion

  //#region Funcion para abrir el "+" o "-" de cada categoria
  const handleCategoryClick = async (index) => {
    setCategorySign((prevSigns) => ({
      ...prevSigns,
      [index]: prevSigns[index] === "-" ? "+" : "-",
    }));

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

  //#region Funcion para borrar la busqueda por query
  const handleClearSearch = () => {
    // Función para limpiar la búsqueda (setear query a vacío)
    setQuery("");
  };
  //#endregion

  //#region Return
  return (
    <div>
      <Helmet>
        <title>Catapp | Catálogo</title>
      </Helmet>

      <section id="home" className="home-container">
        <div className="home-content">
          <div className="home-4">
            <div className="title-search">
              <h1 className="title categories-title title-nomarg">Catálogo</h1>

              <div
                className={
                  color
                    ? "pagination-count3 pagination-count3-fixed"
                    : "pagination-count3"
                }
              >
                <div className="search-container">
                  <div className="form-group-input-search2">
                    <span className="input-group-text2">
                      <Lupa className="input-group-svg" />
                    </span>
                    <input
                      className="search-input3"
                      type="text"
                      value={query}
                      onChange={(e) => setQuery(e.target.value)}
                      onKeyUp={search}
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
            </div>

            {/* Renderizar la lista de productos filtrados */}
            <div className="categorias-container" id="productos-filtrados">
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
                            return (
                              <div className="contenedor-producto" key={index}>
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
                                  </div>
                                  <div className="product-3-col"></div>
                                </div>
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

            <div className="categorias-container" id="categorias-container">
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
                      <div className="collapse" id={`collapseCategory${index}`}>
                        <div className="product-container">
                          {isLoadingProductByCategory === true ? (
                            <div className="loading-single-furniture">
                              <Loader />
                              <p className="bold-loading">
                                Cargando productos pertenecientes a la categoría
                                "{category.nombre}"...
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
                                return (
                                  <div
                                    className="contenedor-producto"
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
                                      </div>
                                      <div className="product-3-col"></div>
                                    </div>
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
          </div>
        </div>
      </section>
    </div>
  );
  //#endregion
};

export default Catalogue;
