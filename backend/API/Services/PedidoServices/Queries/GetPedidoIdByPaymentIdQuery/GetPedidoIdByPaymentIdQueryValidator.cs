using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidoIdByPaymentIdQuery
{
  public class GetPedidoIdByPaymentIdQueryValidator : AbstractValidator<GetPedidoIdByPaymentIdQuery>
  {
    private readonly CatalogoContext _context;

    public GetPedidoIdByPaymentIdQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.paymentId)
          .NotEmpty().WithMessage("El id del pago no puede estar vacío")
          .NotNull().WithMessage("El id del pago no puede ser nulo")
          .MustAsync(PaymentIdExiste).WithMessage("No hay ningun pedido con el id de pago: {PropertyValue}");
    }

    private async Task<bool> PaymentIdExiste(string paymentId, CancellationToken token)
    {
      bool existe = await _context.Pedidos.AnyAsync(p => p.PaymentId == paymentId);
      return existe;
    }

  }
}
