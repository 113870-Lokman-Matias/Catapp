using API.Dtos.PedidoDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.PedidoServices.Commands.UpdatePedidoCommand
{
  public class UpdatePedidoCommand : IRequest<PedidoDto>
  {
    [JsonIgnore]
    public Guid IdPedido { get; set; }
    public float CostoEnvio { get; set; }
    public int? IdVendedor { get; set; }
    public int IdMetodoPago { get; set; }
    public int IdMetodoEntrega { get; set; }
  }
}
