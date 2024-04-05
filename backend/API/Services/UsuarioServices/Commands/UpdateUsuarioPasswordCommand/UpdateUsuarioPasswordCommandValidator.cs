using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioPasswordCommand
{
  public class UpdateUsuarioPasswordCommandValidator : AbstractValidator<UpdateUsuarioPasswordCommand>
  {
    private readonly CatalogoContext _context;
    public UpdateUsuarioPasswordCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(u => u.Username)
          .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío")
          .NotNull().WithMessage("El nombre de usuario no puede ser nulo");

      RuleFor(u => u.Password)
          .NotEmpty().WithMessage("La contraseña no puede estar vacía")
          .NotNull().WithMessage("La contraseña no puede ser nulo")
          .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
    }
  }
}
