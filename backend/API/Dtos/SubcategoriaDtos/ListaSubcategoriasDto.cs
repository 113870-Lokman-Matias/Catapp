using API.AnswerBase;

namespace API.Dtos.SubcategoriaDtos
{
  public class ListaSubcategoriasDto : RespuestaBase
  {
    public List<ListaSubcategoriaDto>? Subcategorias { get; set; }
  }
}
