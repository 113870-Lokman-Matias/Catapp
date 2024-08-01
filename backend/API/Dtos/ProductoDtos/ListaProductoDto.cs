namespace API.Dtos.ProductoDtos
{
  public class ListaProductoDto
  {
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Divisa { get; set; } = null!;
    public float PrecioPesos { get; set; }
    public int Stock { get; set; }
    public string? NombreCategoria { get; set; }
    public string UrlImagen { get; set; } = null!;
    public int IdDivisa { get; set; }
    public int StockTransitorio { get; set; }
    public string? NombreSubcategoria { get; set; }
  }
}
