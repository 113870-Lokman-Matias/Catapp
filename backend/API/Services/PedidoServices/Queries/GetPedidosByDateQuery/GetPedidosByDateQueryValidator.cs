using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosByDateQuery
{
  public class GetPedidosByDateQueryValidator : AbstractValidator<GetPedidosByDateQuery>
  {
    private readonly CatalogoContext _context;

    public GetPedidosByDateQueryValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.fechaDesde)
          .NotEmpty().WithMessage("La fecha desde no puede estar vacía")
          .NotNull().WithMessage("La fecha desde no puede ser nula")
          .Must((query, fechaDesde) => fechaDesde <= query.fechaHasta)
        .WithMessage("La fecha desde debe ser anterior o igual a la fecha hasta");

      RuleFor(p => p.fechaHasta)
     .NotEmpty().WithMessage("La fecha hasta no puede estar vacía")
     .NotNull().WithMessage("La fecha hasta no puede ser nula");

      When(p => p.IdVendedor.HasValue, () =>
            {
              RuleFor(p => p.IdVendedor)
                  .Must(id => _context.Usuarios.Any(v => v.IdUsuario == id))
                  .WithMessage("El ID del vendedor especificado no es válido");
            });

      When(p => p.IdTipoPedido.HasValue, () =>
      {
        RuleFor(p => p.IdTipoPedido)
                  .Must(id => _context.TiposPedidos.Any(tp => tp.IdTipoPedido == id))
                  .WithMessage("El ID del tipo de pedido especificado no es válido");
      });

      When(p => p.IdMetodoEntrega.HasValue, () =>
      {
        RuleFor(p => p.IdMetodoEntrega)
                  .Must(id => _context.MetodosEntregas.Any(me => me.IdMetodoEntrega == id))
                  .WithMessage("El ID del método de entrega especificado no es válido");
      });

      When(p => p.IdMetodoPago.HasValue, () =>
      {
        RuleFor(p => p.IdMetodoPago)
                  .Must(id => _context.MetodosPagos.Any(mp => mp.IdMetodoPago == id))
                  .WithMessage("El ID del método de pago especificado no es válido");
      });

    }

  }
}
