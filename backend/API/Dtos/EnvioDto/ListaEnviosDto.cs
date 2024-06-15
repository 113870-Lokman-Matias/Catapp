using API.AnswerBase;

namespace API.Dtos.EnvioDto
{
  public class ListaEnviosDto : RespuestaBase
  {
    public List<ListaEnvioDto>? Envios { get; set; }
  }
}
