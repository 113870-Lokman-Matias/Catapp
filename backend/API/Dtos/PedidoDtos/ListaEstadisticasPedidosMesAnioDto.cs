using API.AnswerBase;

namespace API.Dtos.PedidoDtos
{
  public class ListaEstadisticasPedidosMesAnioDto : RespuestaBase
  {
    public Dictionary<string, float> Vendedores { get; set; } = new Dictionary<string, float>();

    public string? Producto1 { get; set; }
    public float CantidadVentasFacturacion1 { get; set; }
    public string? Producto2 { get; set; }
    public float CantidadVentasFacturacion2 { get; set; }
    public string? Producto3 { get; set; }
    public float CantidadVentasFacturacion3 { get; set; }
    public string? Producto4 { get; set; }
    public float CantidadVentasFacturacion4 { get; set; }
    public string? Producto5 { get; set; }
    public float CantidadVentasFacturacion5 { get; set; }
    public string? Producto6 { get; set; }
    public float CantidadVentasFacturacion6 { get; set; }
  }
}
