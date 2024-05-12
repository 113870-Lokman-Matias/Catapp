using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidosDataByYearQuery
{
     public record GetPedidosDataByYearQuery(int anio) : IRequest<ListaEstadisticasPedidosAnioDto>;
}
