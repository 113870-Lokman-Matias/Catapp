using API.Dtos.MetodoPagoDto;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.MetodoPagoServices.Commands.DeleteMetodoPagoCommand
{
  public class DeleteMetodoPagoCommand : IRequest<MetodoPagoDto>
  {
    [JsonIgnore]
    public int IdMetodoPago { get; set; }
  }
}
