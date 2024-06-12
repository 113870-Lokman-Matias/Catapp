using API.Dtos.SubcategoriaDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.SubcategoriaServices.Commands.UpdateSubcategoriaCommand
{
  public class UpdateSubcategoriaCommand : IRequest<SubcategoriaDto>
  {
    [JsonIgnore]
    public int IdSubcategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Ocultar { get; set; }
    public int IdCategoria { get; set; }
  }
}
