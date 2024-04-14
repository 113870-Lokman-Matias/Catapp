using API.Dtos.ProductoDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.ProductoServices.Commands.UpdateStockProductoCommand
{
  public class UpdateStockProductoCommand : IRequest<ProductoDto>
  {
    [JsonIgnore]
    public int IdProducto { get; set; }
    public int Stock { get; set; }
  }
}
