import axios from "axios";

//#region Función para obtener los productos para la lista administrativa con filtros opcionales de Query, Category y Hidden
async function GetProductsManage(Query = null, Category = null, Hidden = null) {
  const token = localStorage.getItem("token"); // Obtener el token almacenado en el localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  let url = "https://localhost:7207/producto/manage";

  // Agregar los parámetros de los filtros opcionales a la URL si están presentes
  if (Query !== null) {
    url += `?Query=${Query}`;
  }
  if (Category !== null) {
    url += `&Category=${Category}`;
  }
  if (Hidden !== null) {
    url += `&Hidden=${Hidden}`;
  }

  const result = await axios.get(url, { headers });
  const productos = result.data.productos || [];
  return productos;
}
//#endregion

//#region Funcion para obtener todos los productos por categoria
async function GetProductsByCategory(category, client) {
  const result = await axios.get(
    "https://localhost:7207/producto/categoria/" + category + "/" + client
  );
  return result.data.productos || [];
}
//#endregion

//#region Funcion para obtener todos los productos por categoria
async function GetProductsBySubcategory(idCategory, idSubcategory, client) {
  const result = await axios.get(
    "https://localhost:7207/producto/subcategoria/" +
      idCategory +
      "/" +
      idSubcategory +
      "/" +
      client
  );
  return result.data.productos || [];
}
//#endregion

//#region Funcion para obtener todos los productos por query
async function GetProductsByQuery(query, client) {
  const result = await axios.get(
    "https://localhost:7207/producto/query/" + query + "/" + client
  );
  return result.data.productos || [];
}
//#endregion

//#region Función para obtener un producto por su ID
async function GetProductById(id, client) {
  const token = localStorage.getItem("token"); // Obtener el token almacenado en el localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  const response = await axios.get(
    `https://localhost:7207/producto/${id}/${client}`,
    {
      headers,
    }
  );
  return response.data;
}
//#endregion

//#region Función para guardar un producto en la base de datos
async function SaveProducts(data, headers) {
  return axios.post("https://localhost:7207/producto", data, { headers });
}
//#endregion

//#region Función para actualizar un producto en la base de datos
async function UpdateProducts(id, data, headers) {
  return axios.put(`https://localhost:7207/producto/${id}`, data, { headers });
}
//#endregion

//#region Función para actualizar el stock de los productos
async function UpdateProductsStock(id, data, headers) {
  return axios.patch(`https://localhost:7207/producto/${id}`, data, {
    headers,
  });
}
//#endregion

//#region Función para eliminar un producto de la base de datos
async function DeleteProducts(id, headers) {
  return axios.delete(`https://localhost:7207/producto/${id}`, { headers });
}
//#endregion

//#region Función para subir imagen del producto a Cloudinary
const UploadImages = async (imageSelected) => {
  const formData = new FormData();
  formData.append("file", imageSelected);
  formData.append(
    "upload_preset",
    `${process.env.REACT_APP_UPLOAD_PRESET_NAME}`
  );

  try {
    const response = await axios.post(
      `https://api.cloudinary.com/v1_1/${process.env.REACT_APP_CLOUD_NAME}/image/upload`,
      formData
    );
    const imageUrl = response.data.secure_url;
    const imageId = response.data.public_id;
    const result = { imageUrl, imageId };
    return result;
  } catch (error) {
    console.log(error);
  }
};
//#endregion

//#region Export
export {
  GetProductsManage,
  GetProductsByCategory,
  GetProductsBySubcategory,
  GetProductsByQuery,
  GetProductById,
  SaveProducts,
  UpdateProducts,
  UpdateProductsStock,
  DeleteProducts,
  UploadImages,
};
//#endregion
