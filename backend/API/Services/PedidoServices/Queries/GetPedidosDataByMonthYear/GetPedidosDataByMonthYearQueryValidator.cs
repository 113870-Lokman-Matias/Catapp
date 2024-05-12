using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosDataByMonthYearQuery
{
  public class GetPedidosDataByMonthYearQueryValidator : AbstractValidator<GetPedidosDataByMonthYearQuery>
  {
    private readonly CatalogoContext _context;

    public GetPedidosDataByMonthYearQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.mes)
              .NotEmpty().WithMessage("El año no puede estar vacío")
              .NotNull().WithMessage("El año no puede ser nulo")
              .InclusiveBetween(1, 12).WithMessage("El mes debe estar en el rango de 1 a 12");

      RuleFor(p => p.anio)
             .NotEmpty().WithMessage("El año no puede estar vacío")
             .NotNull().WithMessage("El año no puede ser nulo")
             .GreaterThan(2023).WithMessage("El año ingresado debe ser mayor o igual a 2024")
             .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("El año ingresado debe ser menor o igual al año actual");
    }

  }
}
