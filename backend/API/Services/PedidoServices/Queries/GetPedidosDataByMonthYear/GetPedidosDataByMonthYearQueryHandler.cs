﻿using API.Data;
using API.Dtos.DetallePedidoDto;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosDataByMonthYearQuery
{
  public class GetPedidosDataByMonthYearQueryHandler : IRequestHandler<GetPedidosDataByMonthYearQuery, ListaEstadisticasPedidosMesAnioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetPedidosDataByMonthYearQuery> _validator;

    public GetPedidosDataByMonthYearQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetPedidosDataByMonthYearQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaEstadisticasPedidosMesAnioDto> Handle(GetPedidosDataByMonthYearQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaEstadisticasPedidosMesAnioVacia = new ListaEstadisticasPedidosMesAnioDto();

          ListaEstadisticasPedidosMesAnioVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaEstadisticasPedidosMesAnioVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaEstadisticasPedidosMesAnioVacia.IsSuccess = false;

          return ListaEstadisticasPedidosMesAnioVacia;
        }
        else
        {
          var pedidosQuery = await _context.Pedidos
          .Where(x => x.Verificado == true && x.Fecha.Year == request.anio && x.Fecha.Month == request.mes)
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
            AclaracionEntrega = x.IdMetodoEntregaNavigation.Aclaracion,
            Tipo = x.IdTipoPedidoNavigation.Nombre,
            Vendedor = x.IdVendedorNavigation.Nombre, // Obtener el nombre del vendedor si no es nulo
            CantidadProductos = x.DetallePedidos.Sum(d => d.Cantidad), // Sumar la cantidad de productos de todos los detalles
            Subtotal = x.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario), // Calcular el subtotal sumando el precio unitario * cantidad de cada detalle
            CostoEnvio = x.CostoEnvio,
            Total = x.CostoEnvio == 0 ? x.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario) : x.DetallePedidos.Sum(d => d.Cantidad * d.PrecioUnitario) + x.CostoEnvio,
            Abono = x.IdMetodoPagoNavigation.Nombre,
            Detalle = $"Datos del cliente:\nDNI: {x.IdClienteNavigation.Dni}\n{(x.IdMetodoEntregaNavigation.Nombre.ToLower().Contains("local") ? "" : "\nDirección:\n" + x.Direccion + "\n\nEntre calles:\n" + x.EntreCalles + "\n")}\nNúmero de teléfono:\n{x.IdClienteNavigation.Telefono}\n----------------------------------\nPedido:\n{string.Join("\n", x.DetallePedidos.Select(d => $"{d.Cantidad} x {d.IdProductoNavigation.Nombre} (${d.PrecioUnitario} c/u)\n{(string.IsNullOrEmpty(d.Aclaracion) ? "" : "Aclaración: " + d.Aclaracion + "\n")}"))}",
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
            var ListaEstadisticasPedidosMesAnioVacia = new ListaEstadisticasPedidosMesAnioDto();

            ListaEstadisticasPedidosMesAnioVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaEstadisticasPedidosMesAnioVacia.ErrorMessage = "No hay datos de pedidos verificados en el mes y año seleccionado";
            ListaEstadisticasPedidosMesAnioVacia.IsSuccess = false;

            return ListaEstadisticasPedidosMesAnioVacia;
          }
          else
          {
            var listaEstadisticasPedidosMesAnioDto = new ListaEstadisticasPedidosMesAnioDto();

            if (request.variable == 1)
            {
              // Para los vendedores
              var vendedoresFacturacion = pedidosQuery
                  .Where(p => p.Vendedor != null) // Filtrar pedidos con vendedor no nulo
                  .GroupBy(p => p.Vendedor) // Agrupar por nombre del vendedor
                  .OrderByDescending(g => g.Sum(p => p.Total)) // Ordenar por facturación total en orden descendente
                  .ToDictionary(g => g.Key, g => g.Sum(p => p.Total)); // Convertir a diccionario (nombre del vendedor -> facturación total)

              // Asignar los datos al DTO
              listaEstadisticasPedidosMesAnioDto.Vendedores = vendedoresFacturacion;

              // Para los productos
              var productosFacturacion = pedidosQuery
                  .SelectMany(p => p.Detalles)
                  .GroupBy(d => d.IdProducto)
                  .OrderByDescending(g => g.Sum(d => d.Cantidad * d.PrecioUnitario))
                  .Take(5) // Tomar los primeros cinco productos con mayor facturación
                  .Select(g => new { IdProducto = g.Key, Facturacion = g.Sum(d => d.Cantidad * d.PrecioUnitario) })
                  .Join(_context.Productos,
                        detalle => detalle.IdProducto,
                        producto => producto.IdProducto,
                        (detalle, producto) => new { NombreProducto = producto.Nombre, Facturacion = detalle.Facturacion })
                  .ToList();

              // Asignar los datos al DTO
              for (int i = 0; i < productosFacturacion.Count; i++)
              {
                switch (i)
                {
                  case 0:
                    listaEstadisticasPedidosMesAnioDto.Producto1 = productosFacturacion[i].NombreProducto;
                    listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion1 = productosFacturacion[i].Facturacion;
                    break;
                  case 1:
                    listaEstadisticasPedidosMesAnioDto.Producto2 = productosFacturacion[i].NombreProducto;
                    listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion2 = productosFacturacion[i].Facturacion;
                    break;
                  case 2:
                    listaEstadisticasPedidosMesAnioDto.Producto3 = productosFacturacion[i].NombreProducto;
                    listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion3 = productosFacturacion[i].Facturacion;
                    break;
                  case 3:
                    listaEstadisticasPedidosMesAnioDto.Producto4 = productosFacturacion[i].NombreProducto;
                    listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion4 = productosFacturacion[i].Facturacion;
                    break;
                  case 4:
                    listaEstadisticasPedidosMesAnioDto.Producto5 = productosFacturacion[i].NombreProducto;
                    listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion5 = productosFacturacion[i].Facturacion;
                    break;
                  default:
                    break;
                }
              }

              // Resto de la lógica para obtener los productos menos vendidos y asignarlos al DTO

              // Para el resto de los productos
              listaEstadisticasPedidosMesAnioDto.Producto6 = "Resto de los productos";

              // Obtener los IDs de los primeros cinco productos más vendidos
              var primerosCincoIds = pedidosQuery.SelectMany(p => p.Detalles)
                  .GroupBy(d => d.IdProducto)
                  .OrderByDescending(g => g.Sum(d => d.Cantidad * d.PrecioUnitario))
                  .Select(g => g.Key)
                  .Take(5)
                  .ToList();

              // Obtener la facturación de todos los productos que no están entre los primeros cinco más vendidos
              var facturacionRestoProductos = pedidosQuery.SelectMany(p => p.Detalles)
                  .Where(d => !primerosCincoIds.Contains(d.IdProducto)) // Filtrar productos que no están en la lista de primeros cinco
                  .Sum(d => d.Cantidad * d.PrecioUnitario); // Sumar la facturación

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion6 = facturacionRestoProductos;

              listaEstadisticasPedidosMesAnioDto.StatusCode = StatusCodes.Status200OK;
              listaEstadisticasPedidosMesAnioDto.IsSuccess = true;
              listaEstadisticasPedidosMesAnioDto.ErrorMessage = "";

              return listaEstadisticasPedidosMesAnioDto;
            }
            else
            {
              // Obtener la lista de vendedores y la cantidad de pedidos para cada uno
              var vendedoresPedidos = pedidosQuery
                  .Where(p => p.Vendedor != null) // Filtrar pedidos con vendedor no nulo
                  .GroupBy(p => p.Vendedor) // Agrupar por nombre del vendedor
                  .OrderByDescending(g => g.Count()) // Ordenar por cantidad de pedidos en orden descendente
                  .ToDictionary(g => g.Key, g => (float)g.Count());  // Convertir a diccionario (nombre del vendedor -> cantidad de pedidos como float)

              // Asignar los datos al DTO
              listaEstadisticasPedidosMesAnioDto.Vendedores = vendedoresPedidos;

              #region Primer producto mas vendido
              listaEstadisticasPedidosMesAnioDto.Producto1 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto) // Agrupar por ID de producto
              .OrderByDescending(g => g.Sum(d => d.Cantidad)) // Ordenar en orden descendente por cantidad vendida
              .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) }) // Obtener el ID del producto y la cantidad vendida
              .Join(_context.Productos, // Unir con la tabla de Productos para obtener el nombre del producto
                        detalle => detalle.IdProducto, // Clave de unión del DTO DetallePedidoDto
                        producto => producto.IdProducto, // Clave de unión de la tabla de Productos
                        (detalle, producto) => new { NombreProducto = producto.Nombre }) // Proyectar el nombre del producto
              .FirstOrDefault()?.NombreProducto; // Obtener el nombre del primer producto más vendido

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion1 =
              pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto) // Agrupar por ID de producto
              .OrderByDescending(g => g.Sum(d => d.Cantidad)) // Ordenar en orden descendente por la suma de la cantidad vendida
              .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0; // Obtener la suma de la cantidad del primer producto más vendido o establecer en 0 si no hay datos
              #endregion

              #region Segundo producto mas vendido
              listaEstadisticasPedidosMesAnioDto.Producto2 =
              pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
              .Skip(1) // Saltar el primer producto más vendido para obtener el segundo
              .Join(_context.Productos,
                    detalle => detalle.IdProducto,
                    producto => producto.IdProducto,
                    (detalle, producto) => new { NombreProducto = producto.Nombre })
              .FirstOrDefault()?.NombreProducto;

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion2 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Skip(1) // Saltar el primer producto más vendido para obtener el segundo
              .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0;
              #endregion

              #region Tercer producto mas vendido
              listaEstadisticasPedidosMesAnioDto.Producto3 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
              .Skip(2) // Saltar el primer y segundo producto más vendido para obtener el tercero
              .Join(_context.Productos,
                    detalle => detalle.IdProducto,
                    producto => producto.IdProducto,
                    (detalle, producto) => new { NombreProducto = producto.Nombre })
              .FirstOrDefault()?.NombreProducto;

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion3 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Skip(2) // Saltar el primer y segundo producto más vendido para obtener el tercero
              .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0;
              #endregion

              #region Cuarto producto mas vendido
              listaEstadisticasPedidosMesAnioDto.Producto4 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
              .Skip(3) // Saltar el primer, segundo y tercer producto más vendido para obtener el tercero
              .Join(_context.Productos,
                    detalle => detalle.IdProducto,
                    producto => producto.IdProducto,
                    (detalle, producto) => new { NombreProducto = producto.Nombre })
              .FirstOrDefault()?.NombreProducto;

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion4 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Skip(3) // Saltar el primer, segundo y tercer producto más vendido para obtener el tercero
              .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0;
              #endregion

              #region Quinto producto mas vendido
              listaEstadisticasPedidosMesAnioDto.Producto5 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
              .Skip(4) // Saltar el primer, segundo, tercer y cuarto producto más vendido para obtener el tercero
              .Join(_context.Productos,
                    detalle => detalle.IdProducto,
                    producto => producto.IdProducto,
                    (detalle, producto) => new { NombreProducto = producto.Nombre })
              .FirstOrDefault()?.NombreProducto;

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion5 = pedidosQuery.SelectMany(p => p.Detalles)
              .GroupBy(d => d.IdProducto)
              .OrderByDescending(g => g.Sum(d => d.Cantidad))
              .Skip(4) // Saltar el primer, segundo, tercer y cuarto producto más vendido para obtener el tercero
              .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0;
              #endregion

              #region Todo el resto de los productos
              listaEstadisticasPedidosMesAnioDto.Producto6 = "Resto de los productos";

              // Obtener los IDs de los primeros cinco productos más vendidos
              var primerosCincoIds = pedidosQuery.SelectMany(p => p.Detalles)
                  .GroupBy(d => d.IdProducto)
                  .OrderByDescending(g => g.Sum(d => d.Cantidad))
                  .Select(g => g.Key)
                  .Take(5)
                  .ToList();

              // Obtener la cantidad vendida de todos los productos que no están entre los primeros cinco más vendidos
              var cantidadVendidaRestoProductos = pedidosQuery.SelectMany(p => p.Detalles)
                  .Where(d => !primerosCincoIds.Contains(d.IdProducto)) // Filtrar productos que no están en la lista de primeros cinco
                  .Sum(d => d.Cantidad); // Sumar la cantidad vendida

              listaEstadisticasPedidosMesAnioDto.CantidadVentasFacturacion6 = cantidadVendidaRestoProductos;
              #endregion

              listaEstadisticasPedidosMesAnioDto.StatusCode = StatusCodes.Status200OK;
              listaEstadisticasPedidosMesAnioDto.IsSuccess = true;
              listaEstadisticasPedidosMesAnioDto.ErrorMessage = "";

              return listaEstadisticasPedidosMesAnioDto;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var ListaEstadisticasPedidosMesAnioVacia = new ListaEstadisticasPedidosMesAnioDto();

        ListaEstadisticasPedidosMesAnioVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaEstadisticasPedidosMesAnioVacia.ErrorMessage = ex.Message;
        ListaEstadisticasPedidosMesAnioVacia.IsSuccess = false;

        return ListaEstadisticasPedidosMesAnioVacia;
      }
    }

  }
}
