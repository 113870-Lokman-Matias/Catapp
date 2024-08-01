using API.Dtos.PedidoDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.PedidoServices.Commands.DeletePedidoCommand
{
  public class DeletePedidoCommand : IRequest<PedidoDto>
  {
    [JsonIgnore]
    public Guid IdPedido { get; set; }
  }
}
