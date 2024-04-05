using API.Dtos.UsuarioDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.UsuarioServices.Commands.DeleteUsuarioCommand
{
  public class DeleteUsuarioCommand : IRequest<UsuarioDto>
  {
    [JsonIgnore]
    public int IdUsuario { get; set; }
  }
}
