using API.AnswerBase;

namespace API.Dtos.UsuarioDtos
{
  public class ListaUsuariosDto : RespuestaBase
  {
    public List<ListaUsuarioDto>? Usuarios { get; set; }
  }
}
