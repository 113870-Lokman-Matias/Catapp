using API.Data;
using API.Dtos.PedidoDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosQuery
{
  public class GetPedidosQueryHandler : IRequestHandler<GetPedidosQuery, ListaPedidosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;

    public GetPedidosQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaPedidosDto> Handle(GetPedidosQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var pedidos = await _context.Pedidos
            .Include(x => x.IdMetodoEntregaNavigation) // Incluir la relación de método de entrega
            .Include(x => x.IdTipoPedidoNavigation) // Incluir la relación de tipo de pedido
            .Include(x => x.IdVendedorNavigation) // Incluir la relación de vendedor
            .Include(x => x.IdMetodoPagoNavigation) // Incluir la relación de método de pago
            .Include(x => x.DetallePedidos) // Incluir la relación de detalles de pedido
                .ThenInclude(d => d.IdProductoNavigation) // Incluir la relación de producto en detalle de pedido
            .Select(x => new ListaPedidoDto
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
            .OrderByDescending(x => x.Fecha) // Ordenar por la fecha en orden descendente
            .ToListAsync();

        if (pedidos == null)
        {
          var ListaPedidosVacia = new ListaPedidosDto();

          ListaPedidosVacia.StatusCode = StatusCodes.Status404NotFound;
          ListaPedidosVacia.ErrorMessage = "No hay pedidos para mostrar";
          ListaPedidosVacia.IsSuccess = false;

          return ListaPedidosVacia;
        }
        else
        {
          var listaPedidosDto = new ListaPedidosDto();
          listaPedidosDto.Pedidos = pedidos;

          listaPedidosDto.StatusCode = StatusCodes.Status200OK;
          listaPedidosDto.IsSuccess = true;
          listaPedidosDto.ErrorMessage = "";

          return listaPedidosDto;
        }
      }
      catch (Exception ex)
      {
        var ListaPedidosVacia = new ListaPedidosDto();

        ListaPedidosVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaPedidosVacia.ErrorMessage = ex.Message;
        ListaPedidosVacia.IsSuccess = false;

        return ListaPedidosVacia;
      }
    }

  }
}
