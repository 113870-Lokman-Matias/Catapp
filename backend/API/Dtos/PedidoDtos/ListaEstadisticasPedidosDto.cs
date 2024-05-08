using API.AnswerBase;

namespace API.Dtos.PedidoDtos
{
  public class ListaEstadisticasPedidosDto : RespuestaBase
  {
    public List<ListaPedidoDto>? Pedidos { get; set; }

    public int CantidadPedidos { get; set; }
    public int CantidadProductos { get; set; }
    public float MontoTotalFacturado { get; set; }
    public float PromedioMontoTotalFacturado { get; set; }
    public int CantidadClientes { get; set; }


    public string? PrimerVendedorMasVentas { get; set; }
    public int CantidadVentasPrimerVendedor { get; set; }
    public string? SegundoVendedorMasVentas { get; set; }
    public int CantidadVentasSegundoVendedor { get; set; }
    public string? TercerVendedorMasVentas { get; set; }
    public int CantidadVentasTercerVendedor { get; set; }


    public string? PrimerProductoMasVendido { get; set; }
    public int CantidadPrimerProductoVendidos { get; set; }
    public string? UrlPrimerProductoMasVendido { get; set; }
    public string? SegundoProductoMasVendido { get; set; }
    public int CantidadSegundoProductoVendidos { get; set; }
    public string? UrlSegundoProductoMasVendido { get; set; }
    public string? TercerProductoMasVendido { get; set; }
    public int CantidadTercerProductoVendidos { get; set; }
    public string? UrlTercerProductoMasVendido { get; set; }


    public string? PrimerCategoriaMasAparecida { get; set; }
    public int CantidadPrimerCategoriaMasAparecida { get; set; }
    public string? SegundaCategoriaMasAparecida { get; set; }
    public int CantidadSegundaCategoriaMasAparecida { get; set; }
    public string? TerceraCategoriaMasAparecida { get; set; }
    public int CantidadTerceraCategoriaMasAparecida { get; set; }
  }
}
