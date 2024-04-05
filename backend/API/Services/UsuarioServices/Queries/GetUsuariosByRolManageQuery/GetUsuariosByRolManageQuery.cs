using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Queries.GetUsuariosByRolManageQuery
{
  public record GetUsuariosByRolManageQuery(string role) : IRequest<ListaUsuariosDto>;

}
