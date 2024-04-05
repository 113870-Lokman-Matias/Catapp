using API.AnswerBase;

namespace API.Dtos.UsuarioDtos
{
  public class UsuarioDto : RespuestaBase
  {
    public int IdUsuario { get; set; }
    public string Rol { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool Activo { get; set; }
    public string Email { get; set; } = null!;
  }
}
