using API.Dtos.ProductoDtos;
using MediatR;

namespace API.Services.ProductoServices.Queries.GetProductosBySubcategoryQuery
{
  public record GetProductosBySubcategoryQuery(int idCategory, int idSubcategory, int client) : IRequest<ListaProductosDto>;
}
