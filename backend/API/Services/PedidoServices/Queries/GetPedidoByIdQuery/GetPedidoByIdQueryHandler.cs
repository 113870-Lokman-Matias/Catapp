using API.Data;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidoByIdQuery
{
    public class GetPedidoByIdQueryHandler : IRequestHandler<GetPedidoByIdQuery, PedidoDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<GetPedidoByIdQuery> _validator;

        public GetPedidoByIdQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetPedidoByIdQuery> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<PedidoDto> Handle(GetPedidoByIdQuery request, CancellationToken cancellationToken)
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
                    var pedidoId = request.id;

                    var pedido = await _context.Pedidos
                    .Where(x => x.IdPedido == pedidoId)
                    .Include(x => x.IdMetodoEntregaNavigation) // Incluir la relación de método de entrega
                    .Include(x => x.IdTipoPedidoNavigation) // Incluir la relación de tipo de pedido
                    .Include(x => x.IdVendedorNavigation) // Incluir la relación de vendedor
                    .Include(x => x.IdMetodoPagoNavigation) // Incluir la relación de método de pago
                    .Include(x => x.DetallePedidos) // Incluir la relación de detalles de pedido
                    .ThenInclude(d => d.IdProductoNavigation) // Incluir la relación de producto en detalle de pedido
                    .Select(x => new PedidoDto
                    {
                        IdPedido = x.IdPedido,
                        Cliente = x.IdClienteNavigation.NombreCompleto, // Obtener el nombre completo del cliente
                        Entrega = x.IdMetodoEntregaNavigation.Nombre,
                        Tipo = x.IdTipoPedidoNavigation.Nombre,
                        Vendedor = x.IdVendedorNavigation.Nombre, // Obtener el nombre del vendedor si no es nulo
                        CantidadProductos = x.DetallePedidos.Sum(d => d.Cantidad), // Sumar la cantidad de productos de todos los detalles
                        Subtotal = x.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario), // Calcular el subtotal sumando el precio unitario * cantidad de cada detalle
                        CostoEnvio = x.CostoEnvio,
                        Total = x.CostoEnvio == 0 ? x.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario) : x.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario) + x.CostoEnvio,
                        Abono = x.IdMetodoPagoNavigation.Nombre,
                        Detalle = $"Datos del cliente:\nDNI: {x.IdClienteNavigation.Dni}\n{(x.IdMetodoEntregaNavigation.IdMetodoEntrega == 1 ? "" : "\nDirección:\n" + x.Direccion + "\n\nEntre calles:\n" + x.EntreCalles + "\n")}\nNúmero de teléfono:\n{x.IdClienteNavigation.Telefono}\n----------------------------------\nPedido:\n{string.Join("\n", x.DetallePedidos.Select(d => $"{d.Cantidad} x {d.IdProductoNavigation.Nombre} (${d.PrecioUnitario} c/u)\n{(string.IsNullOrEmpty(d.Aclaracion) ? "" : "Aclaración: " + d.Aclaracion + "\n")}"))}",
                        Fecha = x.Fecha,
                        Verificado = x.Verificado
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