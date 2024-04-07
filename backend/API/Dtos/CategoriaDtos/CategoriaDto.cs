using API.AnswerBase;

namespace API.Dtos.CategoriaDtos
{
  public class CategoriaDto : RespuestaBase
  {
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public string IdImagen { get; set; } = null!;
    public string UrlImagen { get; set; } = null!;
  }
}
