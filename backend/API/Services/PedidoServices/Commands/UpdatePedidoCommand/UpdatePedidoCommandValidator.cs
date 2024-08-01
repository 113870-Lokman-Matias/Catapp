using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Commands.UpdatePedidoCommand
{
  public class UpdatePedidoCommandValidator : AbstractValidator<UpdatePedidoCommand>
  {
    private readonly CatalogoContext _context;

    public UpdatePedidoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.IdPedido)
         .NotEmpty().WithMessage("El id no puede estar vacío")
         .NotNull().WithMessage("El id no puede ser nulo")
         .MustAsync(PedidoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un pedido existente");

      RuleFor(p => p.CostoEnvio)
           .NotNull().WithMessage("El costo de envío no puede ser nulo");

      RuleFor(p => p.IdVendedor)
      .NotEmpty().WithMessage("El id del vendedor no puede estar vacío")
      .NotNull().WithMessage("El id del vendedor no puede ser nulo")
      .MustAsync(VendedorExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un vendedor existente")
      .Unless(p => p.IdVendedor == -1);

      RuleFor(p => p.IdMetodoPago)
      .NotEmpty().WithMessage("El id del metodo de pago no puede estar vacío")
      .NotNull().WithMessage("El id del metodo de pago no puede ser nulo")
      .MustAsync(MetodoPagoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un metodo de pago existente");

      RuleFor(p => p.IdMetodoEntrega)
      .NotEmpty().WithMessage("El id del metodo de entrega no puede estar vacío")
      .NotNull().WithMessage("El id del metodo de entrega no puede ser nulo")
      .MustAsync(MetodoEntregaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un metodo de entrega existente");
    }

    private async Task<bool> PedidoExiste(Guid id, CancellationToken token)
    {
      bool existe = await _context.Pedidos.AnyAsync(p => p.IdPedido == id);
      return existe;
    }

    private async Task<bool> VendedorExiste(int? id, CancellationToken token)
    {
      bool existe = await _context.Usuarios.AnyAsync(p => p.IdUsuario == id);
      return existe;
    }

    private async Task<bool> MetodoPagoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.MetodosPagos.AnyAsync(p => p.IdMetodoPago == id);
      return existe;
    }

    private async Task<bool> MetodoEntregaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Envios.AnyAsync(p => p.IdEnvio == id);
      return existe;
    }

  }
}
