using API.AnswerBase;

namespace API.Dtos.PedidoDtos
{
  public class ListaPedidosDto : RespuestaBase
  {
    public List<ListaPedidoDto>? Pedidos { get; set; }
  }
}
