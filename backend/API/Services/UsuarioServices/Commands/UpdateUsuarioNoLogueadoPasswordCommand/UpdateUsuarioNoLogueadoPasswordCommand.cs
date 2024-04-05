using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioNoLogueadoPasswordCommand
{
  public class UpdateUsuarioNoLogueadoPasswordCommand : IRequest<UsuarioDto>
  {
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int CodigoVerificacion { get; set; }
  }
}
