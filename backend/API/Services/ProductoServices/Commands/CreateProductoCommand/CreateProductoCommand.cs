using API.Dtos.ProductoDtos;
using MediatR;

namespace API.Services.ProductoServices.Commands.CreateProductoCommand
{
  public class CreateProductoCommand : IRequest<ProductoDto>
  {
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int IdDivisa { get; set; }
    public float Precio { get; set; }
    public float PorcentajeMinorista { get; set; }
    public float PorcentajeMayorista { get; set; }
    public float PrecioMinorista { get; set; }
    public float PrecioMayorista { get; set; }
    public int Stock { get; set; }
    public int IdCategoria { get; set; }
    public string IdImagen { get; set; } = null!;
    public string UrlImagen { get; set; } = null!;
    public bool Ocultar { get; set; }
  }
}
