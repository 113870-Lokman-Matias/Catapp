using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidosQuery
{
  public class GetPedidosQuery : IRequest<ListaPedidosDto>
  {
  }
}
