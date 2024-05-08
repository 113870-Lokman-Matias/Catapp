using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidosByDateQuery
{
     public record GetPedidosByDateQuery(DateTimeOffset fechaDesde, DateTimeOffset fechaHasta, int? IdVendedor = null, int? IdTipoPedido = null, int? IdMetodoEntrega = null, int? IdMetodoPago = null) : IRequest<ListaEstadisticasPedidosDto>;
}
