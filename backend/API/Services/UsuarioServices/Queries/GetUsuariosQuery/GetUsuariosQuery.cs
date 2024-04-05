using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Queries.GetUsuariosQuery
{
  public class GetUsuariosQuery : IRequest<ListaUsuariosDto>
  {
  }
}
