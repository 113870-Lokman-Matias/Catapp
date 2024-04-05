using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioCommand
{
  public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
  {
    private readonly CatalogoContext _context;
    public UpdateUsuarioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(c => c.IdUsuario)
         .NotEmpty().WithMessage("El id no puede estar vacío")
         .NotNull().WithMessage("El id no puede ser nulo")
         .MustAsync(UsuarioExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un usuario existente");

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

      RuleFor(u => u.Activo)
          .NotNull().WithMessage("Activo no puede ser nulo");

      RuleFor(u => u.Email)
     .NotEmpty().WithMessage("El email no puede estar vacío")
     .NotNull().WithMessage("El email no puede ser nulo")
     .EmailAddress().WithMessage("El formato del email no es válido");
    }


    private async Task<bool> UsuarioExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(v => v.IdUsuario == id);
      return existe;
    }

    private async Task<bool> RolExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Roles.AnyAsync(p => p.IdRol == id);
      return existe;
    }

  }
}







