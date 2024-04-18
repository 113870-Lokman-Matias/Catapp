using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.StockServices.Commands.CreateDetalleStockCommand
{
  public class CreateDetalleStockCommandValidator : AbstractValidator<CreateDetalleStockCommand>
  {
    private readonly CatalogoContext _context;

    public CreateDetalleStockCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.Accion)
      .NotEmpty().WithMessage("La accion no puede estar vacía")
      .NotNull().WithMessage("La accion no puede ser nula");

      RuleFor(p => p.Cantidad)
      .NotEmpty().WithMessage("La cantidad no puede estar vacía")
      .NotNull().WithMessage("La cantidad no puede ser nula");

      // RuleFor(p => p.Motivo)
      // .NotEmpty().WithMessage("El motivo no puede estar vacío")
      // .NotNull().WithMessage("El motivo no puede ser nulo");

      RuleFor(p => p.Modificador)
      .NotEmpty().WithMessage("El modificador no puede estar vacío")
      .NotNull().WithMessage("El modificador no puede ser nulo");

      RuleFor(p => p.IdProducto)
      .NotEmpty().WithMessage("El id del producto no puede estar vacío")
      .NotNull().WithMessage("El id del producto no puede ser nulo")
      .MustAsync(ProductoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un producto existente");
    }


    private async Task<bool> ProductoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.IdProducto == id);
      return existe;
    }

  }
}
