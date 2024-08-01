using API.Dtos.SubcategoriaDtos;
using MediatR;

namespace API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryQuery
{
  public record GetSubcategoriasByCategoryQuery(int idCategory) : IRequest<ListaSubcategoriasDto>;
}
