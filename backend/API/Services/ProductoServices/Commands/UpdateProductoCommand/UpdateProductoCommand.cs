using API.Dtos.ProductoDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.ProductoServices.Commands.UpdateProductoCommand
{
  public class UpdateProductoCommand : IRequest<ProductoDto>
  {
    [JsonIgnore]
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int IdDivisa { get; set; }
    public float Precio { get; set; }
    public float PorcentajeMinorista { get; set; }
    public float PorcentajeMayorista { get; set; }
    public float PrecioMinorista { get; set; }
    public float PrecioMayorista { get; set; }
    public int IdCategoria { get; set; }
    public string IdImagen { get; set; } = null!;
    public string UrlImagen { get; set; } = null!;
    public bool Ocultar { get; set; }
    public bool EnPromocion { get; set; }
    public bool EnDestacado { get; set; }
  }
}
