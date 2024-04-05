using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.CreateUsuarioCommand
{
  public class CreateUsuarioCommand : IRequest<UsuarioDto>
  {
    public int IdRol { get; set; }
    public string Nombre { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Activo { get; set; }
    public string Email { get; set; } = null!;
  }
}
