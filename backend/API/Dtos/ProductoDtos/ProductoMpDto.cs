using API.AnswerBase;

namespace API.Dtos.ProductoDtos;

public class ProductoMpDto : RespuestaBase
{
    public string IdProducto { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public string? Aclaracion { get; set; }
}
