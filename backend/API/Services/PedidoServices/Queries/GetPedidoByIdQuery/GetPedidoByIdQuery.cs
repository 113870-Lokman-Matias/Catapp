using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidoByIdQuery
{
  public record GetPedidoByIdQuery(Guid id) : IRequest<PedidoDto>;
}
