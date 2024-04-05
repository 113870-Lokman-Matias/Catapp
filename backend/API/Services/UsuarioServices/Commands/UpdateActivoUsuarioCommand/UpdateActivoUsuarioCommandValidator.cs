using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Commands.UpdateActivoUsuarioCommand
{
  public class UpdateActivoUsuarioCommandValidator : AbstractValidator<UpdateActivoUsuarioCommand>
  {
    private readonly CatalogoContext _context;

    public UpdateActivoUsuarioCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.IdUsuario)
         .NotEmpty().WithMessage("El id no puede estar vacío")
         .NotNull().WithMessage("El id no puede ser nulo")
         .MustAsync(UsuarioExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un usuario existente");

      RuleFor(p => p.Activo)
         .NotNull().WithMessage("Activo no puede ser nulo");

      RuleFor(p => p.IdRol)
         .NotEmpty().WithMessage("El id del rol no puede estar vacío")
         .NotNull().WithMessage("El id del rol no puede ser nulo")
         .MustAsync(RolExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un rol existente");
    }

    private async Task<bool> UsuarioExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(p => p.IdUsuario == id);
      return existe;
    }

    private async Task<bool> RolExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Roles.AnyAsync(p => p.IdRol == id);
      return existe;
    }

  }
}
