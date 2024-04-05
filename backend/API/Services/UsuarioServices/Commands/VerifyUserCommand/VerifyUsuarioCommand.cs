using API.Dtos.UsuarioDtos;
using MediatR;

namespace API.Services.UsuarioServices.Commands.VerifyUsuarioCommand
{
  public class VerifyUsuarioCommand : IRequest<UsuarioDto>
  {
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int CodigoVerificacion { get; set; }
  }
}
