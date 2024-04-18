namespace API.Dtos.StockDtos
{
  public class ListaStockDto
  {
    public int IdDetallesStock { get; set; }
    public string Accion { get; set; } = null!;
    public int Cantidad { get; set; }
    public string Motivo { get; set; } = null!;
    public DateTimeOffset Fecha { get; set; }
    public string Modificador { get; set; } = null!;
    public string? NombreProducto { get; set; }
  }
}
