using API.Dtos.EnvioDto;
using MediatR;

namespace API.Services.EnvioServices.Commands.CreateEnvioCommand
{
  public class CreateEnvioCommand : IRequest<EnvioDto>
  {
    public bool Habilitado { get; set; }

    public float Costo { get; set; }

    public string Nombre { get; set; } = null!;

    public int DisponibilidadCatalogo { get; set; }

    public string? Aclaracion { get; set; }
  }
}
