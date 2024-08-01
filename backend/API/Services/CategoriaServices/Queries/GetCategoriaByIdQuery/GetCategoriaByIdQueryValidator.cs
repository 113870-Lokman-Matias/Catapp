using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CategoriaServices.Queries.GetCategoriaByIdQuery
{
  public class GetCategoriaByIdQueryValidator : AbstractValidator<GetCategoriaByIdQuery>
  {
    private readonly CatalogoContext _context;

    public GetCategoriaByIdQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.id)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(CategoriaExiste).WithMessage("No hay ninguna categoría con el id: {PropertyValue}");
    }

    private async Task<bool> CategoriaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Categorias.AnyAsync(p => p.IdCategoria == id);
      return existe;
    }

  }
}
