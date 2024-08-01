using API.Dtos.ProductoDtos;
using MediatR;

namespace API.Services.ProductoServices.Queries.GetProductosByQueryQuery
{
  public record GetProductosByQueryQuery(string query, int client) : IRequest<ListaProductosDto>;
}
