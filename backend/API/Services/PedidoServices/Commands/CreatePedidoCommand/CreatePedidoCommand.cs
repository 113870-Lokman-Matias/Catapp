using API.Dtos.DetallePedidoDto;
using API.Dtos.PedidoDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.PedidoServices.Commands.CreatePedidoCommand
{
  public class CreatePedidoCommand : IRequest<PedidoDto>
  {
    // 1. campos requeridos para crear el cliente
    public string NombreCompleto { get; set; } = null!;
    public long Dni { get; set; }
    public long Telefono { get; set; }

    // Tambien requeridos para el pedido
    public string? Direccion { get; set; }
    public string? EntreCalles { get; set; }

    [JsonIgnore]
    public string? PaymentId { get; set; }

    // 2. campos requeridos para crear el pedido
    public float CostoEnvio { get; set; }
    public int IdTipoPedido { get; set; }
    public int? IdVendedor { get; set; }
    public int IdMetodoPago { get; set; }
    public int IdMetodoEntrega { get; set; }


    // 3. campos requeridos para crear los detalles de pedido
    public List<DetallePedidoDto> Detalles { get; set; } = new List<DetallePedidoDto>();
  }
}
