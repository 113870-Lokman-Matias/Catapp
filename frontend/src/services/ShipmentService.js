import axios from "axios";

//#region Función para obtener el costo del envío y su id
async function GetInfoEnvio() {
  const result = await axios.get("https://localhost:7207/envio");
  return  result.data
}
//#endregion

//#region Función para actualizar el costo de envío en la base de datos
async function UpdateCostoEnvio(data, headers) {
  return axios.put("https://localhost:7207/envio", data, { headers });
}
//#endregion

//#region Export
export { GetInfoEnvio, UpdateCostoEnvio };
//#endregion
