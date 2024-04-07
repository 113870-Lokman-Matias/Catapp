namespace API.Dtos.CategoriaDtos
{
  public class ListaCategoriaDto
  {
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public string IdImagen { get; set; } = null!;
    public string UrlImagen { get; set; } = null!;
    public bool Ocultar { get; set; }
   }
}
