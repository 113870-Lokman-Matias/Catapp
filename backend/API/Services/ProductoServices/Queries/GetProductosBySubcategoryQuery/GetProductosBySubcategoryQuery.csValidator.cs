using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductosBySubcategoryQuery
{
  public class GetProductosBySubcategoryQueryValidator : AbstractValidator<GetProductosBySubcategoryQuery>
  {
    private readonly CatalogoContext _context;

    public GetProductosBySubcategoryQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.idCategory)
            .NotEmpty().WithMessage("La categoría no puede estar vacía")
            .NotNull().WithMessage("La categoría no puede ser nula")
            .MustAsync(CategoriaExiste).WithMessage("No hay productos con la categoría de id: {PropertyValue}");

      RuleFor(p => p.idSubcategory)
            .NotEmpty().WithMessage("La subcategoría no puede estar vacía")
            .NotNull().WithMessage("La subcategoría no puede ser nula")
            .MustAsync((query, idSubcategory, cancellationToken) => SubcategoriaExiste(query.idCategory, idSubcategory, cancellationToken))
            .WithMessage((query, idSubcategory) => $"No hay productos con la subcategoría de id: '{idSubcategory}' en la categoría de id: '{query.idCategory}'");
    
      RuleFor(p => p.client)
            .NotNull().WithMessage("El número de cliente no puede ser nulo");
    }

    private async Task<bool> CategoriaExiste(int idCategory, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.IdCategoriaNavigation.IdCategoria == idCategory);
      return existe;
    }

    private async Task<bool> SubcategoriaExiste(int idCategory, int idSubcategory, CancellationToken cancellationToken)
    {
      return await _context.Subcategorias.AnyAsync(s => s.IdSubcategoria == idSubcategory && s.IdCategoriaNavigation.IdCategoria == idCategory, cancellationToken);
    }

  }
}
