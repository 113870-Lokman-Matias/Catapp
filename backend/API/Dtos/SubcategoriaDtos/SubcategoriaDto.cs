using API.AnswerBase;

namespace API.Dtos.SubcategoriaDtos
{
  public class SubcategoriaDto : RespuestaBase
  {
    public int IdSubcategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdCategoria { get; set; }
  }
}
