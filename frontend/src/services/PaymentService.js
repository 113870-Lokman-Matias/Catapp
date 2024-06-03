import axios from "axios";

//#region Función para realizar un pago con Mercado Pago
async function PayWithMercadoPago(data) {
  try {
    const response = await axios.post("https://localhost:7207/pago", data);
    return response.data;
  } catch (error) {
    console.error("Error en la solicitud:", error);
  }
}
//#endregion

export { PayWithMercadoPago };
