using API.Dtos.UsuarioDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.UsuarioServices.Commands.UpdateActivoUsuarioCommand
{
  public class UpdateActivoUsuarioCommand : IRequest<UsuarioDto>
  {
    [JsonIgnore]
    public int IdUsuario { get; set; }
    public bool Activo { get; set; }
    public int IdRol { get; set; }
  }
}
