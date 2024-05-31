using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.MetodoPagoServices.Commands.DeleteMetodoPagoCommand
{
  public class DeleteMetodoPagoCommandValidator : AbstractValidator<DeleteMetodoPagoCommand>
  {
    private readonly CatalogoContext _context;

    public DeleteMetodoPagoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdMetodoPago)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(MetodoPagoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un metodo de pago existente");
    }

    private async Task<bool> MetodoPagoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.MetodosPagos.AnyAsync(c => c.IdMetodoPago == id);
      return existe;
    }

  }
}
