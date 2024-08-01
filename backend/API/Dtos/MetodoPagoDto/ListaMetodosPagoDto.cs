using API.AnswerBase;

namespace API.Dtos.MetodoPagoDto
{
  public class ListaMetodosPagoDto : RespuestaBase
  {
    public List<ListaMetodoPagoDto>? MetodosPago { get; set; }
  }
}
