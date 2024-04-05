using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.CreateUsuarioNoLogueadoCommand
{
  public class CreateUsuarioNoLogueadoCommandValidator : AbstractValidator<CreateUsuarioNoLogueadoCommand>
  {
    private readonly CatalogoContext _context;

    public CreateUsuarioNoLogueadoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(u => u.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(u => u.Username)
      .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío")
      .NotNull().WithMessage("El nombre de usuario no puede ser nulo");

      RuleFor(u => u.Password)
      .NotEmpty().WithMessage("La contraseña no puede estar vacía")
      .NotNull().WithMessage("La contraseña no puede ser nula")
      .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

      RuleFor(u => u.Email)
      .NotEmpty().WithMessage("El email no puede estar vacío")
      .NotNull().WithMessage("El email no puede ser nulo")
      .EmailAddress().WithMessage("El formato del email no es válido");

      RuleFor(u => u)
            .MustAsync(NombreUsuarioExiste).WithMessage("Este nombre de usuario ya se encuentra registrado")
            .MustAsync(EmailExiste).WithMessage("Este email ya se encuentra registrado");
    }

    private async Task<bool> NombreUsuarioExiste(CreateUsuarioNoLogueadoCommand command, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(u => u.Username == command.Username);

      return !existe;
    }

    private async Task<bool> EmailExiste(CreateUsuarioNoLogueadoCommand command, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(u => u.Email == command.Email);

      return !existe;
    }

    private async Task<bool> UsuarioExiste(CreateUsuarioNoLogueadoCommand command, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(u => u.Username == command.Username);

      return !existe;
    }

  }
}
