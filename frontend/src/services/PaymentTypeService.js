import axios from "axios";

//#region Función para obtener todas los medios de pago
async function GetPaymentTypes() {
  const result = await axios.get("https://localhost:7207/metodospago");
  return result.data.metodosPago;
}
//#endregion

//#region Función para guardar un medio de pago en la base de datos
async function SavePaymentTypes(data, headers) {
  return axios.post("https://localhost:7207/metodospago", data, { headers });
}
//#endregion

//#region Función para actualizar un medio de pago en la base de datos
async function UpdatePaymentTypes(id, data, headers) {
  return axios.put(`https://localhost:7207/metodospago/${id}`, data, {
    headers,
  });
}
//#endregion

//#region Función para eliminar un medio de pago de la base de datos
async function DeletePaymentTypes(id, headers) {
  return axios.delete(`https://localhost:7207/metodospago/${id}`, { headers });
}
//#endregion

//#region Export
export {
  GetPaymentTypes,
  SavePaymentTypes,
  UpdatePaymentTypes,
  DeletePaymentTypes,
};
//#endregion
