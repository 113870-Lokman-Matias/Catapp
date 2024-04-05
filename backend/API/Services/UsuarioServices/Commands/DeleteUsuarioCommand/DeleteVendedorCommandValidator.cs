using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.DeleteUsuarioCommand
{
  public class DeleteUsuarioCommandValidator : AbstractValidator<DeleteUsuarioCommand>
  {
    private readonly CatalogoContext _context;

    public DeleteUsuarioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdUsuario)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(UsuarioExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un usuario existente");
    }

    private async Task<bool> UsuarioExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(v => v.IdUsuario == id);
      return existe;
    }

  }
}
