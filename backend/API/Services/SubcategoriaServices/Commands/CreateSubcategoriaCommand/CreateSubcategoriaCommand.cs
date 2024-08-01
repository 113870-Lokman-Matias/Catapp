using API.Dtos.SubcategoriaDtos;
using MediatR;

namespace API.Services.SubcategoriaServices.Commands.CreateSubcategoriaCommand
{
  public class CreateSubcategoriaCommand : IRequest<SubcategoriaDto>
  {
    public string Nombre { get; set; } = null!;
    public bool Ocultar { get; set; }
    public int IdCategoria { get; set; }
  }
}
