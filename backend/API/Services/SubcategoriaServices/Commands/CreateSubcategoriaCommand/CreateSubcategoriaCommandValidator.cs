using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.SubcategoriaServices.Commands.CreateSubcategoriaCommand
{
  public class CreateSubcategoriaCommandValidator : AbstractValidator<CreateSubcategoriaCommand>
  {
    private readonly CatalogoContext _context;

    public CreateSubcategoriaCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(c => c.Ocultar)
          .NotNull().WithMessage("Ocultar no puede ser nulo");

      RuleFor(p => p.IdCategoria)
          .NotEmpty().WithMessage("El id de la categoria no puede estar vacío")
          .NotNull().WithMessage("El id de la categoria no puede ser nulo")
          .MustAsync(CategoriaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de una categoria existente");

      RuleFor(c => c)
          .MustAsync(SubcategoriaExiste).WithMessage("Esta subcategoría ya se encuentra registrada en la misma categoría");
    }

    private async Task<bool> SubcategoriaExiste(CreateSubcategoriaCommand command, CancellationToken token)
    {
      bool existe = await _context.Subcategorias.AnyAsync(c => c.Nombre == command.Nombre && c.IdCategoria == command.IdCategoria, token);
      return !existe;
    }


    private async Task<bool> CategoriaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Categorias.AnyAsync(p => p.IdCategoria == id);
      return existe;
    }

  }
}
