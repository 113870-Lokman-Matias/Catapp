using API.AnswerBase;

namespace API.Dtos.ProductoDtos;

public class ProductoDto : RespuestaBase
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Divisa { get; set; } = null!;
    public float Precio { get; set; }
    public float PorcentajeMinorista { get; set; }
    public float PorcentajeMayorista { get; set; }
    public float PrecioMinorista { get; set; }
    public float PrecioMayorista { get; set; }
    public float PrecioPesos { get; set; }
    public int Stock { get; set; }
    public string? NombreCategoria { get; set; }
    public string IdImagen { get; set; } = null!;
    public string UrlImagen { get; set; } = null!;
    public bool Ocultar { get; set; }
    public int IdDivisa { get; set; }
    public int StockTransitorio { get; set; }
    public string? NombreSubcategoria { get; set; }
}
