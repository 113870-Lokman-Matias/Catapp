using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.LoginUsuarioCommand
{
  public class LoginUsuarioCommand : IRequest<TokenDto>
  {
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; } = null!;
  }
}
