using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.PagoDto;
using API.Services.PagoServices.Commands.CreatePagoCommand;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using API.Services.PedidoServices.Commands.CreatePedidoCommand;
using API.Dtos.DetallePedidoDto;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("pago")]
public class PagoController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly CatalogoContext _context;
  private readonly IHubContext<GeneralHub> _hubContext;

  public PagoController(IMediator mediator, CatalogoContext context, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _context = context;
    _hubContext = hubContext;
  }

  [HttpPost]
  public async Task<PagoDto> Pagar(CreatePagoCommand command)
  {
    var pagoCreado = await _mediator.Send(command);
    return pagoCreado;
  }


  [HttpPost("webhook")]
  public async Task<IActionResult> RecibirNotificacion()
  {
    using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
    {
      string requestBody = await reader.ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);

      if (data != null && data.data != null && data.data.id != null)
      {
        string id = data.data.id; // Extraer el ID de la notificación

        string accessToken = "APP_USR-2353346951556522-052620-9c8f370d4bf2f2eb5ad29b9258153a06-1831593012";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await client.GetAsync($"https://api.mercadopago.com/v1/payments/{id}");

        if (response.IsSuccessStatusCode)
        {
          string responseBody = await response.Content.ReadAsStringAsync();
          dynamic paymentInfo = JsonConvert.DeserializeObject(responseBody);

          // Console.WriteLine(paymentInfo);

          // Get IdMetodoPago for "MercadoPago"
          var metodoPago = await _context.MetodosPagos
              .FirstOrDefaultAsync(mp => mp.Nombre == "Mercado Pago");

          if (metodoPago == null)
          {
            return BadRequest("Metodo de pago 'MercadoPago' no encontrado.");
          }

          // Crear el comando CreatePedidoCommand con la información del pago
          var createPedidoCommand = new CreatePedidoCommand
          {
            NombreCompleto = paymentInfo.additional_info.payer.first_name,
            Dni = paymentInfo.additional_info.payer.phone.area_code,
            Telefono = paymentInfo.additional_info.payer.phone.number,
            Direccion = paymentInfo.additional_info.payer.address.street_name,
            EntreCalles = paymentInfo.additional_info.payer.address.street_number,
            PaymentId = paymentInfo.id,
            CostoEnvio = paymentInfo.shipping_amount,
            IdTipoPedido = int.Parse(paymentInfo.additional_info.payer.last_name.ToString()),
            IdVendedor = paymentInfo.additional_info.payer.address.zip_code,
            IdMetodoPago = metodoPago.IdMetodoPago,
            IdMetodoEntrega = (string.IsNullOrWhiteSpace((string)paymentInfo.additional_info.payer.address.street_name) &&
                   string.IsNullOrWhiteSpace((string)paymentInfo.additional_info.payer.address.street_number) &&
                   (paymentInfo.shipping_amount == 0 || paymentInfo.shipping_amount.ToString() == "0")) ? 1 : 2,

            Detalles = ((IEnumerable<dynamic>)paymentInfo.additional_info.items).Select(item => new DetallePedidoDto
            {
              IdProducto = item.id,
              Cantidad = item.quantity,
              PrecioUnitario = item.unit_price,
              Aclaracion = item.description
            }).ToList()
          };

          // Enviar el comando para crear el pedido
          var pedidoCreado = await _mediator.Send(createPedidoCommand);
          await _hubContext.Clients.All.SendAsync("MensajeCreatePedido", "Se ha creado un nuevo pedido");

          if (pedidoCreado.IsSuccess)
          {
            return Ok(pedidoCreado);
          }
          else
          {
            return BadRequest(pedidoCreado.ErrorMessage);
          }

        }
        else
        {
          // Manejar el error de la solicitud GET
          return BadRequest("Error al obtener la información del pago.");
        }
      }
      else
      {
        // Manejar el caso en el que los datos son nulos
        return BadRequest("Los datos de la notificación son nulos o no contienen el ID esperado.");
      }
    }
  }

}
