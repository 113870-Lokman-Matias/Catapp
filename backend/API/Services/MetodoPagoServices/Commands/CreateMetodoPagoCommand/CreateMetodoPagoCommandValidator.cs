using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.MetodoPagoServices.Commands.CreateMetodoPagoCommand
{
  public class CreateMetodoPagoCommandValidator : AbstractValidator<CreateMetodoPagoCommand>
  {
    private readonly CatalogoContext _context;

    public CreateMetodoPagoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(c => c.Habilitado)
          .NotNull().WithMessage("Habilitado no puede ser nulo");

      RuleFor(c => c.Disponibilidad)
          .NotEmpty().WithMessage("La disponibilidad no puede estar vacía")
          .NotNull().WithMessage("La disponibilidad no puede ser nula");

      RuleFor(c => c)
          .MustAsync(MetodoPagoExiste).WithMessage("Este metodo de pago ya se encuentra registrado");
    }

    private async Task<bool> MetodoPagoExiste(CreateMetodoPagoCommand command, CancellationToken token)
    {
      bool existe = await _context.MetodosPagos.AnyAsync(c => c.Nombre == command.Nombre);

      return !existe;
    }

  }
}
