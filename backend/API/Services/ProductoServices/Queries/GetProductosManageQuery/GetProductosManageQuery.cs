using API.Dtos.ProductoDtos;
using MediatR;

namespace API.Services.ProductoServices.Queries.GetProductosManageQuery
{
  public record GetProductosManageQuery(string? Query = null, string? Category = null, bool? Hidden = null, bool? Stock = null) : IRequest<ListaProductosManageDto>;
}
