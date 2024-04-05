using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioNoLogueadoPasswordCommand
{
  public class UpdateUsuarioNoLogueadoPasswordCommandValidator : AbstractValidator<UpdateUsuarioNoLogueadoPasswordCommand>
  {
    private readonly CatalogoContext _context;
    public UpdateUsuarioNoLogueadoPasswordCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(u => u.Email)
          .NotEmpty().WithMessage("El email no puede estar vacío")
          .NotNull().WithMessage("El email no puede ser nulo")
          .EmailAddress().WithMessage("El formato del email no es válido");

      RuleFor(u => u.Password)
          .NotEmpty().WithMessage("La contraseña no puede estar vacía")
          .NotNull().WithMessage("La contraseña no puede ser nulo")
          .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

      RuleFor(p => p.CodigoVerificacion)
         .NotNull().WithMessage("El código de verificación no puede ser nulo")
         .NotEmpty().WithMessage("El código de verificación no puede estar vacío");

      RuleFor(u => u)
     .MustAsync(ExisteCodigo).WithMessage("Código de verificación incorrecto");
    }

    private async Task<bool> ExisteCodigo(UpdateUsuarioNoLogueadoPasswordCommand command, CancellationToken token)
    {
      var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == command.Email);

      if (usuario != null)
      {
        // Verifica si el código de verificación ingresado es igual al código de verificación del usuario en la base de datos
        return command.CodigoVerificacion == usuario.CodigoVerificacion;
      }

      return false;
    }
  }
}
