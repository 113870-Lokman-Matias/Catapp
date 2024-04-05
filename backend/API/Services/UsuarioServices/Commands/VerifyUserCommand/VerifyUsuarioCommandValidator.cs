using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.VerifyUsuarioCommand
{
  public class VerifyUsuarioCommandValidator : AbstractValidator<VerifyUsuarioCommand>
  {
    private readonly CatalogoContext _context;
    public VerifyUsuarioCommandValidator(CatalogoContext context)
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

      RuleFor(p => p.CodigoVerificacion)
      .NotNull().WithMessage("El código de verificación no puede ser nulo")
      .NotEmpty().WithMessage("El código de verificación no puede estar vacío");

      RuleFor(u => u)
          .MustAsync(ExisteCodigo).WithMessage("Código de verificación incorrecto");
    }

    private async Task<bool> ExisteCodigo(VerifyUsuarioCommand command, CancellationToken token)
    {
      var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == command.Username || u.Email == command.Email);

      if (usuario != null)
      {
        // Verifica si el código de verificación ingresado es igual al código de verificación del usuario en la base de datos
        return command.CodigoVerificacion == usuario.CodigoVerificacion;
      }

      return false;
    }
  }
}
