using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.StockServices.Queries.GetDetallesStockByIdQuery
{
  public class GetDetallesStockByIdQueryValidator : AbstractValidator<GetDetallesStockByIdQuery>
  {
    private readonly CatalogoContext _context;

    public GetDetallesStockByIdQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.id)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(DetalleStockExiste).WithMessage("No hay detalles de stock con el id: {PropertyValue}");
    }

    private async Task<bool> DetalleStockExiste(int id, CancellationToken token)
    {
      bool existe = await _context.DetallesStocks.AnyAsync(p => p.IdProductoNavigation.IdProducto == id);
      return existe;
    }

  }
}
