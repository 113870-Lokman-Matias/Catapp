using API.Data;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace API.Services.PedidoServices.Commands.UpdateVerificadoPedidoCommand
{
  public class UpdateVerificadoPedidoCommandHandler : IRequestHandler<UpdateVerificadoPedidoCommand, PedidoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateVerificadoPedidoCommand> _validator;

    public UpdateVerificadoPedidoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateVerificadoPedidoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<PedidoDto> Handle(UpdateVerificadoPedidoCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var PedidoVacio = new PedidoDto();

          PedidoVacio.StatusCode = StatusCodes.Status400BadRequest;
          PedidoVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          PedidoVacio.IsSuccess = false;

          return PedidoVacio;
        }
        else
        {
          var PedidoToUpdate = await _context.Pedidos.FindAsync(request.IdPedido);

          if (PedidoToUpdate == null)
          {
            var PedidoVacio = new PedidoDto();

            PedidoVacio.StatusCode = StatusCodes.Status404NotFound;
            PedidoVacio.ErrorMessage = "El pedido no existe";
            PedidoVacio.IsSuccess = false;

            return PedidoVacio;
          }
          else
          {
            // Verificar si se ha cambiado el estado de verificado
            if (PedidoToUpdate.Verificado != request.Verificado)
            {
              // Si el pedido pasa de no verificado a verificado, actualizar el stock de los productos
              if (request.Verificado)
              {
                var detallesPedido = await _context.DetallePedidos.Where(d => d.IdPedido == request.IdPedido).ToListAsync();
                foreach (var detalle in detallesPedido)
                {
                  var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                  if (producto != null)
                  {
                    producto.Stock = producto.StockTransitorio; // Actualizar el stock al stock transitorio
                    _context.Productos.Update(producto);
                  }
                }
              }
              else // Si el pedido pasa de verificado a no verificado, restaurar el stock original de los productos
              {
                var detallesPedido = await _context.DetallePedidos.Where(d => d.IdPedido == request.IdPedido).ToListAsync();
                foreach (var detalle in detallesPedido)
                {
                  var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                  if (producto != null)
                  {
                    producto.Stock += detalle.Cantidad; // Restaurar el stock original sumando la cantidad
                    _context.Productos.Update(producto);
                  }
                }
              }

              PedidoToUpdate.Verificado = request.Verificado;
              await _context.SaveChangesAsync();

              _context.Attach(PedidoToUpdate);

              var pedidoDto = _mapper.Map<PedidoDto>(PedidoToUpdate);

              pedidoDto.StatusCode = StatusCodes.Status200OK;
              pedidoDto.IsSuccess = true;
              pedidoDto.ErrorMessage = "";

              return pedidoDto;
            }
            else
            {
              // No se ha cambiado el estado de verificado
              var pedidoDto = _mapper.Map<PedidoDto>(PedidoToUpdate);

              pedidoDto.StatusCode = StatusCodes.Status200OK;
              pedidoDto.IsSuccess = true;
              pedidoDto.ErrorMessage = "";

              return pedidoDto;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var PedidoVacio = new PedidoDto();

        PedidoVacio.StatusCode = StatusCodes.Status400BadRequest;
        PedidoVacio.ErrorMessage = ex.Message;
        PedidoVacio.IsSuccess = false;

        return PedidoVacio;
      }
    }

  }
}
