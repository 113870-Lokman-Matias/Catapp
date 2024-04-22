using API.AnswerBase;

namespace API.Dtos.PedidoDtos
{
  public class PedidoDto : RespuestaBase
  {
    public Guid IdPedido { get; set; }

    public string Cliente { get; set; } = null!;

    public string Entrega { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Vendedor { get; set; } = null!;

    public int CantidadProductos { get; set; }

    public float Subtotal { get; set; }

    public float CostoEnvio { get; set; }

    public float Total { get; set; }

    public string Abono { get; set; } = null!;

    public string Detalle { get; set; } = null!;

    public DateTimeOffset Fecha { get; set; }
    public bool Verificado { get; set; }
  }
}
