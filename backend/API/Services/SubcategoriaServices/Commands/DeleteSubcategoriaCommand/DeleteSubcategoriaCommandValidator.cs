using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.SubcategoriaServices.Commands.DeleteSubcategoriaCommand
{
  public class DeleteSubcategoriaCommandValidator : AbstractValidator<DeleteSubcategoriaCommand>
  {
    private readonly CatalogoContext _context;

    public DeleteSubcategoriaCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdSubcategoria)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(SubcategoriaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de una subcategoría existente");
    }

    private async Task<bool> SubcategoriaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Subcategorias.AnyAsync(c => c.IdSubcategoria == id);
      return existe;
    }

  }
}
