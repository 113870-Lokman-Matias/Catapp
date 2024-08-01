using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidosQuery
{
  public record GetPedidosQuery(string? Type = null, int? Seller = null, int? Shipment = null, int? Payment = null, bool? Status = null) : IRequest<ListaPedidosDto>;
}
