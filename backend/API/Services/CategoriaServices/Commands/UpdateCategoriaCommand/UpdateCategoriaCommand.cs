using API.Dtos.CategoriaDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.CategoriaServices.Commands.UpdateCategoriaCommand
{
  public class UpdateCategoriaCommand : IRequest<CategoriaDto>
  {
    [JsonIgnore]
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
     public string IdImagen { get; set; } = null!;
    public string UrlImagen { get; set; } = null!;
    public bool Ocultar { get; set; }
  }
}
