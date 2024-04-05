using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Queries.GetUsuariosByRolManageQuery
{
  public class GetUsuariosByRolManageQueryValidator : AbstractValidator<GetUsuariosByRolManageQuery>
  {
    private readonly CatalogoContext _context;

    public GetUsuariosByRolManageQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.role)
          .NotEmpty().WithMessage("El rol no puede estar vacío")
          .NotNull().WithMessage("El rol no puede ser nulo")
          .MustAsync(RolExiste).WithMessage("No hay usuarios con el rol: {PropertyValue}");
    }

    private async Task<bool> RolExiste(string role, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(p => p.IdRolNavigation.Nombre == role || p.IdRolNavigation.IdRol == 6);
      return existe;
    }

  }
}
