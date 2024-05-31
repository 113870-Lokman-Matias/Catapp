using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidosQuery
{
  public record GetPedidosQuery(string? Type = null, bool? Status = null) : IRequest<ListaPedidosDto>;
}
