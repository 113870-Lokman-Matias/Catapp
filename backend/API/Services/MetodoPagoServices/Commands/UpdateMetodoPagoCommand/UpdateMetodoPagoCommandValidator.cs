using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.MetodoPagoServices.Commands.UpdateMetodoPagoCommand
{
  public class UpdateMetodoPagoCommandValidator : AbstractValidator<UpdateMetodoPagoCommand>
  {
    private readonly CatalogoContext _context;
    public UpdateMetodoPagoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdMetodoPago)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(MetodoPagoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un metodo de pago existente");

      RuleFor(c => c.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(c => c.Habilitado)
          .NotNull().WithMessage("Habilitado no puede ser nulo");

      RuleFor(c => c.Disponibilidad)
          .NotEmpty().WithMessage("La disponibilidad no puede estar vacía")
          .NotNull().WithMessage("La disponibilidad no puede ser nula");
    }

    private async Task<bool> MetodoPagoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.MetodosPagos.AnyAsync(c => c.IdMetodoPago == id);
      return existe;
    }

  }
}
