using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidoByIdQuery
{
  public class GetPedidoByIdQueryValidator : AbstractValidator<GetPedidoByIdQuery>
  {
    private readonly CatalogoContext _context;

    public GetPedidoByIdQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.id)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(PedidoExiste).WithMessage("No hay ningun pedido con el id: {PropertyValue}");
    }

    private async Task<bool> PedidoExiste(Guid id, CancellationToken token)
    {
      bool existe = await _context.Pedidos.AnyAsync(p => p.IdPedido == id);
      return existe;
    }

  }
}
