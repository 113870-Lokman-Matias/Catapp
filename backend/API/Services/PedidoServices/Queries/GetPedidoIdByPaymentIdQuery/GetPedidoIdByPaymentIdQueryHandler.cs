using API.Data;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidoIdByPaymentIdQuery
{
    public class GetPedidoIdByPaymentIdQueryHandler : IRequestHandler<GetPedidoIdByPaymentIdQuery, PedidoDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<GetPedidoIdByPaymentIdQuery> _validator;

        public GetPedidoIdByPaymentIdQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetPedidoIdByPaymentIdQuery> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<PedidoDto> Handle(GetPedidoIdByPaymentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var PedidoVacio = new PedidoDto
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage)),
                        IsSuccess = false
                    };

                    return PedidoVacio;
                }
                else
                {
                    var paymentId = request.paymentId;

                    var pedido = await _context.Pedidos
                    .Where(x => x.PaymentId == paymentId)
                    .Select(x => new PedidoDto
                    {
                        IdPedido = x.IdPedido,
                    })
                    .FirstOrDefaultAsync();


                    if (pedido == null)
                    {
                        var PedidoVacio = new PedidoDto
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            ErrorMessage = "No existe el pedido",
                            IsSuccess = false
                        };

                        return PedidoVacio;
                    }
                    else
                    {
                        pedido.StatusCode = StatusCodes.Status200OK;
                        pedido.IsSuccess = true;
                        pedido.ErrorMessage = "";

                        return pedido;
                    }
                }
            }
            catch (Exception ex)
            {
                var PedidoVacio = new PedidoDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };

                return PedidoVacio;
            }
        }
    }
}