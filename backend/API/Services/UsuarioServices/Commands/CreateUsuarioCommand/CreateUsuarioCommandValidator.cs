using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.CreateUsuarioCommand
{
  public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
  {
    private readonly CatalogoContext _context;

    public CreateUsuarioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.IdRol)
      .NotEmpty().WithMessage("El id del rol no puede estar vacío")
      .NotNull().WithMessage("El id del rol no puede ser nulo")
      .MustAsync(RolExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un rol existente");

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

      RuleFor(u => u.Activo)
          .NotNull().WithMessage("Activo no puede ser nulo");

      RuleFor(u => u.Email)
      .NotEmpty().WithMessage("El email no puede estar vacío")
      .NotNull().WithMessage("El email no puede ser nulo")
      .EmailAddress().WithMessage("El formato del email no es válido");

      RuleFor(u => u)
            .MustAsync(UsuarioExiste).WithMessage("Este usuario ya se encuentra registrado");
    }

    private async Task<bool> UsuarioExiste(CreateUsuarioCommand command, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(u => u.Username == command.Username);

      return !existe;
    }

    private async Task<bool> RolExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Roles.AnyAsync(p => p.IdRol == id);
      return existe;
    }

  }
}
