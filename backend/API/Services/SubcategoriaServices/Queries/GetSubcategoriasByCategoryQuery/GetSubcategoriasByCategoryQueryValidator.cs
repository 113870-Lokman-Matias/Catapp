using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryQuery
{
  public class GetSubcategoriasByCategoryQueryValidator : AbstractValidator<GetSubcategoriasByCategoryQuery>
  {
    private readonly CatalogoContext _context;

    public GetSubcategoriasByCategoryQueryValidator(CatalogoContext context)
    {
      _context = context;

       RuleFor(p => p.idCategory)
            .NotEmpty().WithMessage("El id no puede estar vacío")
            .NotNull().WithMessage("El id no puede ser nulo")
            .MustAsync(CategoriaExiste).WithMessage("No hay subcategorias con el id de categoría: {PropertyValue}");
    }

    private async Task<bool> CategoriaExiste(int idCategory, CancellationToken token)
    {
      bool existe = await _context.Subcategorias.AnyAsync(p => p.IdCategoriaNavigation.IdCategoria == idCategory);
      return existe;
    }

  }
}
