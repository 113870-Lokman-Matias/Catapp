using API.Dtos.UsuarioDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioCommand
{
  public class UpdateUsuarioCommand : IRequest<UsuarioDto>
  {
    [JsonIgnore]
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
    public string Nombre { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool Activo { get; set; }
    public string Email { get; set; } = null!;
  }
}
