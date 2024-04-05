using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.SearchUsuarioCommand
{
  public class SearchUsuarioCommandValidator : AbstractValidator<SearchUsuarioCommand>
  {
    private readonly CatalogoContext _context;
    public SearchUsuarioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(u => u.Username)
          .NotEmpty().WithMessage("El usuario/email no puede estar vacío")
          .NotNull().WithMessage("El usuario/email no puede ser nulo")
          .When(u => string.IsNullOrEmpty(u.Email));

      RuleFor(u => u.Email)
          .NotEmpty().WithMessage("El usuario/email no puede estar vacío")
          .NotNull().WithMessage("El usuario/email no puede ser nulo")
          .EmailAddress().WithMessage("El formato del email no es válido")
          .When(u => string.IsNullOrEmpty(u.Username));

      RuleFor(u => u)
          .MustAsync(ExisteUsario).WithMessage("Usuario/Email no registrado");
    }

    private async Task<bool> ExisteUsario(SearchUsuarioCommand command, CancellationToken token)
    {
      var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == command.Username || u.Email == command.Email);

      if (usuario != null)
      {
        return true;
      }

      return false;
    }
  }
}
