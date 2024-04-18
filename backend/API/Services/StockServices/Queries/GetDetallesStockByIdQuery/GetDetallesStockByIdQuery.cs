using API.Dtos.StockDtos;
using MediatR;

namespace API.Services.StockServices.Queries.GetDetallesStockByIdQuery
{
  public record GetDetallesStockByIdQuery(int id) : IRequest<ListaStocksDto>;
}
