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
      "Bearer TEST-718354923242289-012400-c44c2ef741fc53df597d76a76ba62dbf-1070291733",
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
