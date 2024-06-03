using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidoIdByPaymentIdQuery
{
  public record GetPedidoIdByPaymentIdQuery(string paymentId) : IRequest<PedidoDto>;
}
