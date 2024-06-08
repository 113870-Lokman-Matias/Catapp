using API.Dtos.MetodoPagoDto;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.MetodoPagoServices.Commands.UpdateMetodoPagoCommand
{
  public class UpdateMetodoPagoCommand : IRequest<MetodoPagoDto>
  {
    [JsonIgnore]
    public int IdMetodoPago { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Habilitado { get; set; }
    public int Disponibilidad { get; set; }
    public int DisponibilidadCatalogo { get; set; }
  }
}
