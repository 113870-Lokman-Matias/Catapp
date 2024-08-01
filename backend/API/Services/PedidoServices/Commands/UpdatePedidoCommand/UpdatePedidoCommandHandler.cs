using API.Data;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;


namespace API.Services.PedidoServices.Commands.UpdatePedidoCommand
{
  public class UpdatePedidoCommandHandler : IRequestHandler<UpdatePedidoCommand, PedidoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdatePedidoCommand> _validator;

    public UpdatePedidoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdatePedidoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<PedidoDto> Handle(UpdatePedidoCommand request, CancellationToken cancellationToken)
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
            PedidoToUpdate.CostoEnvio = request.CostoEnvio;
            if (request.IdVendedor == -1)
            {
              PedidoToUpdate.IdVendedor = null; // Asignar null si el valor es -1 (representando "ninguno")
            }
            else
            {
              PedidoToUpdate.IdVendedor = request.IdVendedor; // Asignar el valor normal
            }
            PedidoToUpdate.IdMetodoPago = request.IdMetodoPago;
            PedidoToUpdate.IdMetodoEntrega = request.IdMetodoEntrega;

            await _context.SaveChangesAsync();

            var pedidoDto = _mapper.Map<PedidoDto>(PedidoToUpdate);

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
