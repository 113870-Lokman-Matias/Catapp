using API.Dtos.SubcategoriaDtos;
using MediatR;

namespace API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryManageQuery
{
  public record GetSubcategoriasByCategoryManageQuery(int idCategory) : IRequest<ListaSubcategoriasDto>;
}
