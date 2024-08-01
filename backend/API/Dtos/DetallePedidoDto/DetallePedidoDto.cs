namespace API.Dtos.DetallePedidoDto
{
  public class DetallePedidoDto
  {
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public string? Aclaracion { get; set; }
    public float PrecioUnitario { get; set; }
  }
}
