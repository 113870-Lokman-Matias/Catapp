using API.Dtos.StockDtos;
using MediatR;

namespace API.Services.StockServices.Commands.CreateDetalleStockCommand
{
  public class CreateDetalleStockCommand : IRequest<StockDto>
  {
    public string Accion { get; set; } = null!;
    public int Cantidad { get; set; }
    public string? Motivo { get; set; }
    public int IdProducto { get; set; }
  }
}
