using API.Data;
using API.Dtos.DetallePedidoDto;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosDataByYearQuery
{
  public class GetPedidosDataByYearQueryHandler : IRequestHandler<GetPedidosDataByYearQuery, ListaEstadisticasPedidosAnioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetPedidosDataByYearQuery> _validator;

    public GetPedidosDataByYearQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetPedidosDataByYearQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaEstadisticasPedidosAnioDto> Handle(GetPedidosDataByYearQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaEstadisticasPedidosAnioVacia = new ListaEstadisticasPedidosAnioDto();

          ListaEstadisticasPedidosAnioVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaEstadisticasPedidosAnioVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaEstadisticasPedidosAnioVacia.IsSuccess = false;

          return ListaEstadisticasPedidosAnioVacia;
        }
        else
        {
          var pedidosQuery = await _context.Pedidos
          .Where(x => x.Verificado == true && x.Fecha.Year == request.anio)
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
            Detalle = $"Datos del cliente:\nDNI: {x.IdClienteNavigation.Dni}\n{(x.IdMetodoEntregaNavigation.IdMetodoEntrega == 1 ? "" : "\nDirección:\n" + x.IdClienteNavigation.Direccion + "\n\nEntre calles:\n" + x.IdClienteNavigation.EntreCalles + "\n")}\nNúmero de teléfono:\n{x.IdClienteNavigation.Telefono}\n----------------------------------\nPedido:\n{string.Join("\n", x.DetallePedidos.Select(d => $"{d.Cantidad} x {d.IdProductoNavigation.Nombre} (${d.PrecioUnitario} c/u)\n{(string.IsNullOrEmpty(d.Aclaracion) ? "" : "Aclaración: " + d.Aclaracion + "\n")}"))}",
            Fecha = x.Fecha,
            Verificado = x.Verificado,
            Detalles = x.DetallePedidos.Select(d => new DetallePedidoDto
            {
              IdProducto = d.IdProducto,
              Cantidad = d.Cantidad,
              Aclaracion = d.Aclaracion,
              PrecioUnitario = d.PrecioUnitario
            }).ToList()
          })
          .ToListAsync();

          if (pedidosQuery == null || !pedidosQuery.Any())
          {
            var ListaEstadisticasPedidosAnioVacia = new ListaEstadisticasPedidosAnioDto();

            ListaEstadisticasPedidosAnioVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaEstadisticasPedidosAnioVacia.ErrorMessage = "No hay datos de pedidos verificados en el año seleccionado";
            ListaEstadisticasPedidosAnioVacia.IsSuccess = false;

            return ListaEstadisticasPedidosAnioVacia;
          }
          else
          {
            var listaEstadisticasPedidosAnioDto = new ListaEstadisticasPedidosAnioDto();

            for (int mes = 1; mes <= 12; mes++)
            {
              var totalFacturacionMes = pedidosQuery
                  .Where(x => x.Fecha.Month == mes)
                  .Sum(x => x.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario) + x.CostoEnvio);

              var totalFacturacionMesSinEnvio = pedidosQuery
                  .Where(x => x.Fecha.Month == mes)
                  .Sum(x => x.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario));

              var cantidadPedidosMes = pedidosQuery
              .Count(x => x.Fecha.Month == mes);

              switch (mes)
              {
                case 1:
                  listaEstadisticasPedidosAnioDto.Facturacion1 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio1 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos1 = cantidadPedidosMes;
                  break;
                case 2:
                  listaEstadisticasPedidosAnioDto.Facturacion2 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio2 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos2 = cantidadPedidosMes;
                  break;
                case 3:
                  listaEstadisticasPedidosAnioDto.Facturacion3 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio3 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos3 = cantidadPedidosMes;
                  break;
                case 4:
                  listaEstadisticasPedidosAnioDto.Facturacion4 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio4 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos4 = cantidadPedidosMes;
                  break;
                case 5:
                  listaEstadisticasPedidosAnioDto.Facturacion5 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio5 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos5 = cantidadPedidosMes;
                  break;
                case 6:
                  listaEstadisticasPedidosAnioDto.Facturacion6 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio6 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos6 = cantidadPedidosMes;
                  break;
                case 7:
                  listaEstadisticasPedidosAnioDto.Facturacion7 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio7 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos7 = cantidadPedidosMes;
                  break;
                case 8:
                  listaEstadisticasPedidosAnioDto.Facturacion8 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio8 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos8 = cantidadPedidosMes;
                  break;
                case 9:
                  listaEstadisticasPedidosAnioDto.Facturacion9 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio9 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos9 = cantidadPedidosMes;
                  break;
                case 10:
                  listaEstadisticasPedidosAnioDto.Facturacion10 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio10 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos10 = cantidadPedidosMes;
                  break;
                case 11:
                  listaEstadisticasPedidosAnioDto.Facturacion11 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio11 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos11 = cantidadPedidosMes;
                  break;
                case 12:
                  listaEstadisticasPedidosAnioDto.Facturacion12 = totalFacturacionMes;
                  listaEstadisticasPedidosAnioDto.FacturacionSinEnvio12 = totalFacturacionMesSinEnvio;
                  listaEstadisticasPedidosAnioDto.CantidadPedidos12 = cantidadPedidosMes;
                  break;
              }
            }

            listaEstadisticasPedidosAnioDto.StatusCode = StatusCodes.Status200OK;
            listaEstadisticasPedidosAnioDto.IsSuccess = true;
            listaEstadisticasPedidosAnioDto.ErrorMessage = "";

            return listaEstadisticasPedidosAnioDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaEstadisticasPedidosAnioVacia = new ListaEstadisticasPedidosAnioDto();

        ListaEstadisticasPedidosAnioVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaEstadisticasPedidosAnioVacia.ErrorMessage = ex.Message;
        ListaEstadisticasPedidosAnioVacia.IsSuccess = false;

        return ListaEstadisticasPedidosAnioVacia;
      }
    }

  }
}
