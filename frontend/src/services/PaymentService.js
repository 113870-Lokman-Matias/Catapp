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

//#region Función para obtener la informacion del pago
async function GetPaymentInfo(paymentId) {
  const headers = {
    Authorization:
      "Bearer APP_USR-2353346951556522-052620-9c8f370d4bf2f2eb5ad29b9258153a06-1831593012",
  };

  const response = await axios.get(
    `https://api.mercadopago.com/v1/payments/${paymentId}`,
    {
      headers
    }
  );
  return response.data;
}
//#endregion

export { PayWithMercadoPago, GetPaymentInfo};
