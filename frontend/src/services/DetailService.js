import axios from "axios";

//#region Función para obtener detalles de stock por ID de producto
async function GetDetailsById(id) {
  const token = localStorage.getItem("token"); // Obtener el token almacenado en el localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  const response = await axios.get(`https://localhost:7207/stock/${id}`, {
    headers,
  });
  return response.data;
}
//#endregion

//#region Función para guardar un detalle de stock en la base de datos
async function SaveStockDetail(data, headers) {
  return axios.post("https://localhost:7207/stock", data, { headers });
}
//#endregion

export { GetDetailsById, SaveStockDetail };
