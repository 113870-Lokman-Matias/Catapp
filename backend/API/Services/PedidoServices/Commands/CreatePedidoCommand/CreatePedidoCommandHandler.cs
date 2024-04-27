using API.Data;
using API.Dtos.PedidoDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Commands.CreatePedidoCommand
{
    public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, PedidoDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePedidoCommand> _validator;

        public CreatePedidoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreatePedidoCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<PedidoDto> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var pedidoVacio = new PedidoDto
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage)),
                        IsSuccess = false
                    };

                    return pedidoVacio;
                }

                // Crear o actualizar el cliente
                var cliente = await CrearOActualizarClienteAsync(request);

                // Crear el pedido
                var pedido = await CrearPedidoAsync(request, cliente);

                // Agregar detalles al pedido
                await AgregarDetallesPedidoAsync(request, pedido, request.IdTipoPedido);

                var pedidoDto = _mapper.Map<PedidoDto>(pedido);
                pedidoDto.StatusCode = StatusCodes.Status200OK;
                pedidoDto.IsSuccess = true;
                pedidoDto.ErrorMessage = string.Empty;

                return pedidoDto;
            }
            catch (Exception ex)
            {
                var pedidoVacio = new PedidoDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };

                return pedidoVacio;
            }
        }

        private async Task<Cliente> CrearOActualizarClienteAsync(CreatePedidoCommand request)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Dni == request.Dni);

            if (cliente == null)
            {
                cliente = new Cliente
                {
                    NombreCompleto = request.NombreCompleto,
                    Dni = request.Dni,
                    Telefono = request.Telefono,
                    Direccion = request.Direccion,
                    EntreCalles = request.EntreCalles
                };

                _context.Clientes.Add(cliente);
            }
            else
            {
                // Actualizar los campos del cliente si son diferentes
                if (cliente.NombreCompleto != request.NombreCompleto)
                    cliente.NombreCompleto = request.NombreCompleto;

                if (cliente.Telefono != request.Telefono)
                    cliente.Telefono = request.Telefono;

                if (cliente.Direccion != request.Direccion)
                    cliente.Direccion = request.Direccion;

                if (cliente.EntreCalles != request.EntreCalles)
                    cliente.EntreCalles = request.EntreCalles;

                _context.Clientes.Update(cliente);
            }

            await _context.SaveChangesAsync();

            return cliente;
        }

        private async Task<Pedido> CrearPedidoAsync(CreatePedidoCommand request, Cliente cliente)
        {
            var pedidoToCreate = _mapper.Map<Pedido>(request);

            pedidoToCreate.IdPedido = Guid.NewGuid();
            if (request.IdVendedor == -1)
            {
                pedidoToCreate.IdVendedor = null; // Asignar null si el valor es -1 (representando "ninguno")
            }
            else
            {
                pedidoToCreate.IdVendedor = request.IdVendedor; // Asignar el valor normal
            }

            pedidoToCreate.Fecha = DateTimeOffset.Now.ToUniversalTime();
            // Verificar si el abono es igual a 5 y establecer como verificado

            if (request.IdMetodoPago == 5)
            {
                pedidoToCreate.Verificado = true;
            }
            else
            {
                pedidoToCreate.Verificado = false;
            }

            pedidoToCreate.IdCliente = cliente.IdCliente;

            await _context.AddAsync(pedidoToCreate);
            await _context.SaveChangesAsync();

            return pedidoToCreate;
        }

        private async Task AgregarDetallesPedidoAsync(CreatePedidoCommand request, Pedido pedido, int idTipoPedido)
        {
            foreach (var detalleDto in request.Detalles)
            {
                var detallePedido = new DetallePedido
                {
                    IdProducto = detalleDto.IdProducto,
                    Cantidad = detalleDto.Cantidad,
                    Aclaracion = detalleDto.Aclaracion,
                    PrecioUnitario = detalleDto.PrecioUnitario,
                    IdPedido = pedido.IdPedido
                };

                _context.DetallePedidos.Add(detallePedido);

                // Crear un registro en DetallesStock
                var detalleStock = new DetallesStock
                {
                    Accion = "Quitar",
                    Cantidad = detalleDto.Cantidad,
                    Motivo = idTipoPedido == 1 ? $"Pedido Minorista: {pedido.IdPedido}" : $"Pedido Mayorista: {pedido.IdPedido}",
                    Fecha = DateTimeOffset.Now.ToUniversalTime(),
                    Modificador = "Cliente",
                    IdProducto = detalleDto.IdProducto
                };

                _context.DetallesStocks.Add(detalleStock);


                if (request.IdMetodoPago == 5){
                    // Actualizar el stock transitorio y normal del producto restando la cantidad del detalle
                    var producto = await _context.Productos.FindAsync(detalleDto.IdProducto);
                    if (producto != null)
                    {
                        producto.StockTransitorio -= detalleDto.Cantidad;
                        producto.Stock -= detalleDto.Cantidad;
                        _context.Productos.Update(producto);
                    }
                }
                else {
                    // Actualizar el stock transitorio del producto restando la cantidad del detalle
                    var producto = await _context.Productos.FindAsync(detalleDto.IdProducto);
                    if (producto != null)
                    {
                        producto.StockTransitorio -= detalleDto.Cantidad;
                        _context.Productos.Update(producto);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}