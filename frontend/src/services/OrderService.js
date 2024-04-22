import axios from "axios";

//#region Función para obtener los pedidos para la lista administrativa
async function GetOrders(state) {
  const token = localStorage.getItem("token"); // Obtener el token almacenado en el localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  const result = await axios.get("https://localhost:7207/pedido", { headers });
  const pedidos = result.data.pedidos || [];
  state(pedidos);
}
//#endregion

//#region Función para obtener los pedidos verificados
async function GetVerifiedOrders(state) {
  const token = localStorage.getItem("token"); // Obtener el token almacenado en el localStorage
  const headers = {
    Authorization: `Bearer ${token}`, // Agregar el encabezado Authorization con el valor del token
  };

  const result = await axios.get("https://localhost:7207/pedido/verificado", {
    headers,
  });
  const pedidos = result.data.pedidos || [];
  state(pedidos);
}
//#endregion

//#region Función para guardar un pedido en la base de datos
async function SaveOrders(data) {
  return axios.post("https://localhost:7207/pedido", data);
}
//#endregion

//#region Función para actualizar un pedido en la base de datos
async function UpdateOrders(id, data, headers) {
  return axios.put(`https://localhost:7207/pedido/${id}`, data, { headers });
}
//#endregion

//#region Función para actualizar el estado de verificado de un pedido en la base de datos
async function UpdateOrdersVerified(id, data, headers) {
  return axios.patch(`https://localhost:7207/pedido/${id}`, data, { headers });
}
//#endregion

//#region Función para eliminar un pedido de la base de datos
async function DeleteOrders(id, headers) {
  return axios.delete(`https://localhost:7207/pedido/${id}`, { headers });
}
//#endregion

//#region Export
export {
  GetOrders,
  GetVerifiedOrders,
  SaveOrders,
  UpdateOrders,
  UpdateOrdersVerified,
  DeleteOrders,
};
//#endregion
