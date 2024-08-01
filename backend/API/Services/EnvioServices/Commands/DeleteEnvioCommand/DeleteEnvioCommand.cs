using API.Dtos.EnvioDto;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.EnvioServices.Commands.DeleteEnvioCommand
{
  public class DeleteEnvioCommand : IRequest<EnvioDto>
  {
    [JsonIgnore]
    public int IdEnvio { get; set; }
  }
}
