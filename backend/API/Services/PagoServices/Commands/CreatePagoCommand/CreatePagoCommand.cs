using API.Dtos.PagoDto;
using MediatR;

namespace API.Services.PagoServices.Commands.CreatePagoCommand
{
  public class CreatePagoCommand : IRequest<PagoDto>
  {
    public string Url { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
  }
}
