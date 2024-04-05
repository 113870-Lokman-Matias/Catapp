using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.LoginUsuarioCommand
{
  public class LoginUsuarioCommandValidator : AbstractValidator<LoginUsuarioCommand>
  {
    private readonly CatalogoContext _context;
    public LoginUsuarioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(u => u.Username)
          .NotEmpty().WithMessage("El usuario/email no puede estar vacío")
          .NotNull().WithMessage("El usuario/email no puede ser nulo")
          .When(u => string.IsNullOrEmpty(u.Email));

      RuleFor(u => u.Email)
          .NotEmpty().WithMessage("El usuario/email no puede estar vacío")
          .NotNull().WithMessage("El usuario/email no puede ser nulo")
          .When(u => string.IsNullOrEmpty(u.Username));

      RuleFor(u => u.Password)
          .NotEmpty().WithMessage("La contraseña no puede estar vacía")
          .NotNull().WithMessage("La contraseña no puede ser nulo");

      RuleFor(u => u)
          .MustAsync(ExisteUsario).WithMessage("Usuario/Email o contraseña incorrecta");
    }

    private async Task<bool> ExisteUsario(LoginUsuarioCommand command, CancellationToken token)
    {
      var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == command.Username || u.Email == command.Email);

      if (usuario != null)
      {
        bool passwordValida = BCrypt.Net.BCrypt.Verify(command.Password, usuario.Password);
        return passwordValida;
      }

      return false;
    }
  }
}
