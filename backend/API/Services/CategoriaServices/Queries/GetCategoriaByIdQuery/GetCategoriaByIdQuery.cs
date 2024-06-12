using API.Dtos.CategoriaDtos;
using MediatR;

namespace API.Services.CategoriaServices.Queries.GetCategoriaByIdQuery
{
  public record GetCategoriaByIdQuery(int id) : IRequest<CategoriaDto>;
}
