using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.SubcategoriaServices.Commands.UpdateSubcategoriaCommand
{
  public class UpdateSubcategoriaCommandValidator : AbstractValidator<UpdateSubcategoriaCommand>
  {
    private readonly CatalogoContext _context;
    public UpdateSubcategoriaCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdSubcategoria)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(IdSubcategoriaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de una subcategoría existente");

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
            .MustAsync(SubcategoriaNombreUnicoEnCategoria).WithMessage("Esta subcategoría ya se encuentra registrada en la misma categoría");
    }

    private async Task<bool> IdSubcategoriaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Subcategorias.AnyAsync(c => c.IdSubcategoria == id);
      return existe;
    }

    private async Task<bool> CategoriaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Categorias.AnyAsync(p => p.IdCategoria == id);
      return existe;
    }

    private async Task<bool> SubcategoriaNombreUnicoEnCategoria(UpdateSubcategoriaCommand command, CancellationToken token)
    {
      bool existe = await _context.Subcategorias.AnyAsync(c => c.Nombre == command.Nombre && c.IdCategoria == command.IdCategoria && c.IdSubcategoria != command.IdSubcategoria, token);
      return !existe;
    }

  }
}
