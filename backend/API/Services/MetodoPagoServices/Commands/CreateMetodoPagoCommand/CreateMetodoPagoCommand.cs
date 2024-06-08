using API.Dtos.MetodoPagoDto;
using MediatR;

namespace API.Services.MetodoPagoServices.Commands.CreateMetodoPagoCommand
{
  public class CreateMetodoPagoCommand : IRequest<MetodoPagoDto>
  {
    public string Nombre { get; set; } = null!;
    public bool Habilitado { get; set; }
    public int Disponibilidad { get; set; }
    public int DisponibilidadCatalogo { get; set; }
  }
}
