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
            .Cascade(CascadeMode.StopOnFirstFailure) // Detener validación en el primer fallo
            .NotEmpty().WithMessage("La categoría no puede estar vacía")
            .NotNull().WithMessage("La categoría no puede ser nula")
            .MustAsync(CategoriaExiste).WithMessage("No hay productos con la categoría: {PropertyValue}");
      
      RuleFor(p => p.client)
            .NotNull().WithMessage("El número de cliente no puede ser nulo");
    }

    private async Task<bool> CategoriaExiste(string category, CancellationToken token)
    {
        // Si la categoría es "Promociones" o "Destacados", no aplicar la validación
        if (category.Equals("Promociones", StringComparison.OrdinalIgnoreCase) || category.Equals("Destacados", StringComparison.OrdinalIgnoreCase))
        {
            return true; // Se considera válida automáticamente
        }

        bool existe = await _context.Productos.AnyAsync(p => p.IdCategoriaNavigation.Nombre == category);
        return existe;
    }

  }
}
