import axios from "axios";

//#region Función para obtener las categorías para la lista administrativa
async function GetCategoriesManage(state) {
  const token = localStorage.getItem("token"); // Obtener el token almacenado en el localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  const result = await axios.get("https://localhost:7207/categoria/manage", {
    headers,
  });
  const categorias = result.data.categorias || [];
  state(categorias);
}
//#endregion

//#region Función para obtener todas los categorías
async function GetCategories(state) {
  const result = await axios.get("https://localhost:7207/categoria");
  state(result.data.categorias);
}
//#endregion

//#region Función para guardar una categoría en la base de datos
async function SaveCategories(data, headers) {
  return axios.post("https://localhost:7207/categoria", data, { headers });
}
//#endregion

//#region Función para actualizar una categoría en la base de datos
async function UpdateCategories(id, data, headers) {
  return axios.put(`https://localhost:7207/categoria/${id}`, data, { headers });
}
//#endregion

//#region Función para eliminar una categoría de la base de datos
async function DeleteCategories(id, headers) {
  return axios.delete(`https://localhost:7207/categoria/${id}`, { headers });
}
//#endregion

//#region Función para subir imagen de la catgeoria a Cloudinary
const UploadImagesCategory = async (imageSelected) => {
  const formData = new FormData();
  formData.append("file", imageSelected);
  formData.append(
    "upload_preset",
    `${process.env.REACT_APP_UPLOAD_PRESET_NAME_CATEGORY}`
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
  GetCategoriesManage,
  GetCategories,
  SaveCategories,
  UpdateCategories,
  DeleteCategories,
  UploadImagesCategory,
};
//#endregion
