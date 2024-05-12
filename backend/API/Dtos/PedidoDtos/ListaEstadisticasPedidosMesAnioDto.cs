using API.AnswerBase;

namespace API.Dtos.PedidoDtos
{
  public class ListaEstadisticasPedidosMesAnioDto : RespuestaBase
  {
    public Dictionary<string, int> Vendedores { get; set; } = new Dictionary<string, int>();

    public string? Producto1 { get; set; }
    public int CantidadVendida1 { get; set; }
    public string? Producto2 { get; set; }
    public int CantidadVendida2 { get; set; }
    public string? Producto3 { get; set; }
    public int CantidadVendida3 { get; set; }
    public string? Producto4 { get; set; }
    public int CantidadVendida4 { get; set; }
    public string? Producto5 { get; set; }
    public int CantidadVendida5 { get; set; }
  }
}
