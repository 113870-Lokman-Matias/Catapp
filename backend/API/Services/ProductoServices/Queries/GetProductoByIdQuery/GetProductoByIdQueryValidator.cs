using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductoByIdQuery
{
  public class GetProductoByIdQueryValidator : AbstractValidator<GetProductoByIdQuery>
  {
    private readonly CatalogoContext _context;

    public GetProductoByIdQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.id)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(ProductoExiste).WithMessage("No hay ningun producto con el id: {PropertyValue}");
    }

    private async Task<bool> ProductoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.IdProducto == id);
      return existe;
    }

  }
}
