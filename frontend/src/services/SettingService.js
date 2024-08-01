import axios from "axios";

//#region Función para obtener las configuraciones
async function GetInfoConfiguracion() {
  const result = await axios.get("https://localhost:7207/configuracion");
  return  result.data
}
//#endregion

//#region Función para actualizar la configuracion en la base de datos
async function UpdateConfiguracion(data, headers) {
  return axios.put("https://localhost:7207/configuracion", data, { headers });
}
//#endregion

//#region Export
export { GetInfoConfiguracion, UpdateConfiguracion };
//#endregion
