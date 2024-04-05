using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.CreateUsuarioNoLogueadoCommand
{
  public class CreateUsuarioNoLogueadoCommand : IRequest<UsuarioDto>
  {
    public string Nombre { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
  }
}
