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

//#region Función para obtener los pedidos verificados por fecha con filtros opcionales
async function GetVerifiedOrdersByDate(fechaDesde, fechaHasta, IdVendedor = null, IdTipoPedido = null, IdMetodoEntrega = null, IdMetodoPago = null) {
  const token = localStorage.getItem("token");
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  let url = `https://localhost:7207/pedido/${fechaDesde}/${fechaHasta}`;

  // Agregar los parámetros de los filtros opcionales a la URL si están presentes
  if (IdVendedor !== null) {
    url += `?IdVendedor=${IdVendedor}`;
  }
  if (IdTipoPedido !== null) {
    url += `&IdTipoPedido=${IdTipoPedido}`;
  }
  if (IdMetodoEntrega !== null) {
    url += `&IdMetodoEntrega=${IdMetodoEntrega}`;
  }
  if (IdMetodoPago !== null) {
    url += `&IdMetodoPago=${IdMetodoPago}`;
  }

  const result = await axios.get(url, { headers });

  return result.data;
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
  GetVerifiedOrdersByDate,
  SaveOrders,
  UpdateOrders,
  UpdateOrdersVerified,
  DeleteOrders,
};
//#endregion
