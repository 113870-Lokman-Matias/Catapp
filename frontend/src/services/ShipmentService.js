import axios from "axios";

//#region Función para obtener unicamente el costo del envío
async function GetCostoEnvioUnicamente(state) {
  const result = await axios.get("https://localhost:7207/envio");
  state(result.data.precio);
}
//#endregion

//#region Función para obtener el costo del envío y su id
async function GetCostoEnvio(state) {
  const result = await axios.get("https://localhost:7207/envio");
  state(result.data);
}
//#endregion

//#region Función para actualizar el costo de envío en la base de datos
async function UpdateCostoEnvio(data, headers) {
  return axios.put("https://localhost:7207/envio", data, { headers });
}
//#endregion

//#region Export
export { GetCostoEnvio, UpdateCostoEnvio, GetCostoEnvioUnicamente };
//#endregion
