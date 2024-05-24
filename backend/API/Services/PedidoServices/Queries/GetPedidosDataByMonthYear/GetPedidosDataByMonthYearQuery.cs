using API.Dtos.PedidoDtos;
using MediatR;

namespace API.Services.PedidoServices.Queries.GetPedidosDataByMonthYearQuery
{
     public record GetPedidosDataByMonthYearQuery(int mes, int anio, int variable) : IRequest<ListaEstadisticasPedidosMesAnioDto>;
}
