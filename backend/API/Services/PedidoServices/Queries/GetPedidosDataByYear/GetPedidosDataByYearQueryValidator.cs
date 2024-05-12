using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosDataByYearQuery
{
  public class GetPedidosDataByYearQueryValidator : AbstractValidator<GetPedidosDataByYearQuery>
  {
    private readonly CatalogoContext _context;

    public GetPedidosDataByYearQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.anio)
             .NotEmpty().WithMessage("El año no puede estar vacío")
             .NotNull().WithMessage("El año no puede ser nulo")
             .GreaterThan(2023).WithMessage("El año ingresado debe ser mayor o igual a 2024")
             .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("El año ingresado debe ser menor o igual al año actual");
    }

  }
}
