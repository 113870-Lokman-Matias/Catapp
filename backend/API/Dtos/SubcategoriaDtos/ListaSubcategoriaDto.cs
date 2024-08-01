namespace API.Dtos.SubcategoriaDtos
{
  public class ListaSubcategoriaDto
  {
    public int IdSubcategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Ocultar { get; set; }
    public int IdCategoria { get; set; }
  }
}
