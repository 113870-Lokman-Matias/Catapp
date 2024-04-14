using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductosByCategoryQuery
{
  public class GetProductosByCategoryQueryValidator : AbstractValidator<GetProductosByCategoryQuery>
  {
    private readonly CatalogoContext _context;

    public GetProductosByCategoryQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.category)
          .NotEmpty().WithMessage("La categoría no puede estar vacía")
          .NotNull().WithMessage("La categoría no puede ser nula")
          .MustAsync(CategoriaExiste).WithMessage("No hay muebles con la categoría: {PropertyValue}");
    }

    private async Task<bool> CategoriaExiste(string category, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.IdCategoriaNavigation.Nombre == category);
      return existe;
    }

  }
}
