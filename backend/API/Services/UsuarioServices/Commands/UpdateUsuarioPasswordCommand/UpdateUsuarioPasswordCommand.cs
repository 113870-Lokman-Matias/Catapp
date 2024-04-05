using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioPasswordCommand
{
  public class UpdateUsuarioPasswordCommand : IRequest<UsuarioDto>
  {
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
  }
}
