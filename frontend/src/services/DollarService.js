import axios from "axios";

//#region Función para obtener unicamente la cotización del dolar
async function GetCotizacionDolarUnicamente(state) {
  const result = await axios.get("https://localhost:7207/cotizacion");
  state(result.data.precio);
}
//#endregion

//#region Función para obtener la cotización del dolar y su id
async function GetCotizacionDolar(state) {
  const result = await axios.get("https://localhost:7207/cotizacion");
  state(result.data);
}
//#endregion

//#region Función para actualizar la cotización del dolar en la base de datos
async function UpdateCotizacionDolar(data, headers) {
  return axios.put("https://localhost:7207/cotizacion", data, { headers });
}
//#endregion

//#region Función para obtener las cotizaciones del dolar blue en tiempo real (consumo de API externa)
async function GetCotizacionDolarBlue(state) {
  const result = await axios.get("https://api.bluelytics.com.ar/v2/latest");
  state(result.data.blue);
}
//#endregion

//#region Función para obtener la fecha de la cotización del dolar blue (consumo de API externa)
async function GetFechaDolarBlue(state) {
  const result = await axios.get("https://api.bluelytics.com.ar/v2/latest");
  state(result.data.last_update);
}
//#endregion

//#region Export
export {
  GetCotizacionDolar,
  UpdateCotizacionDolar,
  GetCotizacionDolarUnicamente,
  GetCotizacionDolarBlue,
  GetFechaDolarBlue,
};
//#endregion
