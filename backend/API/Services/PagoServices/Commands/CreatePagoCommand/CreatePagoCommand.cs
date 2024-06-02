using API.Dtos.PagoDto;
using API.Dtos.ProductoDtos;
using API.Dtos.ClienteDto;
using MediatR;

namespace API.Services.PagoServices.Commands.CreatePagoCommand
{
  public class CreatePagoCommand : IRequest<PagoDto>
  {
    public string Url { get; set; }
    public List<ProductoMpDto> Productos { get; set; }
    public ClienteMpDto Cliente { get; set; }
    public decimal CostoEnvio { get; set; }
  }
}
