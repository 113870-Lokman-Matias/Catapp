using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EnvioServices.Commands.CreateEnvioCommand
{
  public class CreateEnvioCommandValidator : AbstractValidator<CreateEnvioCommand>
  {
    private readonly CatalogoContext _context;

    public CreateEnvioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.Habilitado)
          .NotNull().WithMessage("Habilitado no puede ser nulo");

      RuleFor(p => p.Costo)
          .NotNull().WithMessage("El precio no puede ser nulo");

      RuleFor(p => p.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(c => c.DisponibilidadCatalogo)
          .NotEmpty().WithMessage("La disponibilidad del catálogo no puede estar vacía")
          .NotNull().WithMessage("La disponibilidad del catálogo no puede ser nula");
      RuleFor(c => c)
          .MustAsync(FormaEntregaExiste).WithMessage("Esta forma de entrega ya se encuentra registrada con el mismo nombre y aclaración");
    }

    private async Task<bool> FormaEntregaExiste(CreateEnvioCommand command, CancellationToken token)
    {
      bool existe = await _context.Envios.AnyAsync(c => c.Nombre == command.Nombre && c.Aclaracion == command.Aclaracion, token);

      return !existe;
    }

  }
}
