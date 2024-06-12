using API.Dtos.SubcategoriaDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.SubcategoriaServices.Commands.DeleteSubcategoriaCommand
{
  public class DeleteSubcategoriaCommand : IRequest<SubcategoriaDto>
  {
    [JsonIgnore]
    public int IdSubcategoria { get; set; }
  }
}
