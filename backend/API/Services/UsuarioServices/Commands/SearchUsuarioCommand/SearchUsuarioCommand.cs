using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.SearchUsuarioCommand
{
  public class SearchUsuarioCommand : IRequest<UsuarioDto>
  {
    public string? Username { get; set; }
    public string? Email { get; set; }
  }
}
