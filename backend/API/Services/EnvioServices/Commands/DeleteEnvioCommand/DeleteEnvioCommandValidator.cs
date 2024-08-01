using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EnvioServices.Commands.DeleteEnvioCommand
{
  public class DeleteEnvioCommandValidator : AbstractValidator<DeleteEnvioCommand>
  {
    private readonly CatalogoContext _context;

    public DeleteEnvioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdEnvio)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(FormaEntregaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de una forma de entrega existente");
    }

    private async Task<bool> FormaEntregaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Envios.AnyAsync(c => c.IdEnvio == id);
      return existe;
    }

  }
}
