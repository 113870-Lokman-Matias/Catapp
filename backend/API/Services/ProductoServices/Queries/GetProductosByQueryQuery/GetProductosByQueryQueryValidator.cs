using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductosByQueryQuery
{
  public class GetProductosByQueryQueryValidator : AbstractValidator<GetProductosByQueryQuery>
  {
    private readonly CatalogoContext _context;

    public GetProductosByQueryQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.query)
          .NotEmpty().WithMessage("La busqueda no puede estar vacía")
          .NotNull().WithMessage("La busqueda no puede ser nula")
          .MustAsync(QueryExiste).WithMessage("No hay productos con la busqueda: {PropertyValue}");

      RuleFor(p => p.client)
          .NotNull().WithMessage("El número de cliente no puede ser nulo");
    }

    private async Task<bool> QueryExiste(string query, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.Nombre.ToLower().Contains(query.ToLower()) || p.Descripcion.ToLower().Contains(query.ToLower()));
      return existe;
    }

  }
}
