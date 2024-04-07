using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CategoriaServices.Commands.CreateCategoriaCommand
{
  public class CreateCategoriaCommandValidator : AbstractValidator<CreateCategoriaCommand>
  {
    private readonly CatalogoContext _context;

    public CreateCategoriaCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(p => p.IdImagen)
          .NotEmpty().WithMessage("El id de la imagen no puede estar vacío")
          .NotNull().WithMessage("El id de la imagen no puede ser nulo");

      RuleFor(c => c.UrlImagen)
          .NotEmpty().WithMessage("La url de la imagen no puede estar vacía")
          .NotNull().WithMessage("La url de la imagen no puede ser nula");

      RuleFor(c => c.Ocultar)
          .NotNull().WithMessage("Ocultar no puede ser nulo");

      RuleFor(c => c)
          .MustAsync(CategoriaExiste).WithMessage("Esta categoría ya se encuentra registrada");
    }

    private async Task<bool> CategoriaExiste(CreateCategoriaCommand command, CancellationToken token)
    {
      bool existe = await _context.Categorias.AnyAsync(c => c.Nombre == command.Nombre);

      return !existe;
    }

  }
}
