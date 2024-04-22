using API.Dtos.PedidoDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.PedidoServices.Commands.UpdateVerificadoPedidoCommand
{
  public class UpdateVerificadoPedidoCommand : IRequest<PedidoDto>
  {
    [JsonIgnore]
    public Guid IdPedido { get; set; }

    public bool Verificado { get; set; }
  }
}
