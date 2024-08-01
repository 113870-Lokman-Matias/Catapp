using API.Data;
using API.Models;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace API.Services.PedidoServices.Commands.DeletePedidoCommand
{
  public class DeletePedidoCommandHandler : IRequestHandler<DeletePedidoCommand, PedidoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeletePedidoCommand> _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeletePedidoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeletePedidoCommand> validator, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PedidoDto> Handle(DeletePedidoCommand request, CancellationToken cancellationToken)
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
          var PedidoToDelete = await _context.Pedidos.FindAsync(request.IdPedido);

          if (PedidoToDelete == null)
          {
            var PedidoVacio = new PedidoDto();

            PedidoVacio.StatusCode = StatusCodes.Status404NotFound;
            PedidoVacio.ErrorMessage = "El pedido no existe";
            PedidoVacio.IsSuccess = false;

            return PedidoVacio;
          }
          else
          {
            // Consultar y eliminar los detalles de pedidos relacionados
            var detallesAEliminar = await _context.DetallePedidos.Where(d => d.IdPedido == request.IdPedido).ToListAsync();
            _context.DetallePedidos.RemoveRange(detallesAEliminar);

            // Revertir la actualización del stock transitorio de los productos si el pedido no está verificado
            if (!PedidoToDelete.Verificado)
            {
              foreach (var detalle in detallesAEliminar)
              {
                var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                if (producto != null)
                {
                  producto.StockTransitorio += detalle.Cantidad;
                  _context.Productos.Update(producto);


                  // Crear un nuevo registro en DetallesStock por la cancelación del pedido
                  var detalleStock = new DetallesStock
                  {
                    Accion = "Agregar", // Acción "Agregar" porque se agrega de nuevo el producto
                    Cantidad = detalle.Cantidad,
                    Motivo = PedidoToDelete.IdTipoPedido == 1 ? $"Cancelación de Pedido Minorista: {PedidoToDelete.IdPedido}" : $"Cancelación de Pedido Mayorista: {PedidoToDelete.IdPedido}",
                    // Motivo = $"Cancelación de Pedido: {PedidoToDelete.IdPedido}", // Motivo por la cancelación del pedido
                    Fecha = DateTimeOffset.Now.ToUniversalTime(),
                    Modificador = _httpContextAccessor.HttpContext?.User?.Identity?.Name,
                    IdProducto = detalle.IdProducto
                  };

                  _context.DetallesStocks.Add(detalleStock);
                }
              }
            }

            await _context.SaveChangesAsync();

            _context.Pedidos.Remove(PedidoToDelete);
            await _context.SaveChangesAsync();

            var pedidoDto = _mapper.Map<PedidoDto>(PedidoToDelete);

            pedidoDto.StatusCode = StatusCodes.Status200OK;
            pedidoDto.IsSuccess = true;
            pedidoDto.ErrorMessage = "";

            return pedidoDto;
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