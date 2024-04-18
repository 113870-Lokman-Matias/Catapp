using API.AnswerBase;

namespace API.Dtos.StockDtos
{
  public class ListaStocksDto : RespuestaBase
  {
    public List<ListaStockDto>? Stocks { get; set; }
  }
}
