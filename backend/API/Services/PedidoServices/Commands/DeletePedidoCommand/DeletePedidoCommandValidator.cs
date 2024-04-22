using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Commands.DeletePedidoCommand
{
  public class DeletePedidoCommandValidator : AbstractValidator<DeletePedidoCommand>
  {
    private readonly CatalogoContext _context;

    public DeletePedidoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.IdPedido)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(PedidoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un pedido existente");
    }

    private async Task<bool> PedidoExiste(Guid id, CancellationToken token)
    {
      bool existe = await _context.Pedidos.AnyAsync(p => p.IdPedido == id);
      return existe;
    }

  }
}
