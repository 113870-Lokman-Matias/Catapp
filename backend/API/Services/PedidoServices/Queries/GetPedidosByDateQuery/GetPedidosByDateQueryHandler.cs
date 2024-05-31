using API.Data;
using API.Dtos.DetallePedidoDto;
using API.Dtos.PedidoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.PedidoServices.Queries.GetPedidosByDateQuery
{
  public class GetPedidosByDateQueryHandler : IRequestHandler<GetPedidosByDateQuery, ListaEstadisticasPedidosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetPedidosByDateQuery> _validator;

    public GetPedidosByDateQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetPedidosByDateQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaEstadisticasPedidosDto> Handle(GetPedidosByDateQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaPedidosVacia = new ListaEstadisticasPedidosDto();

          ListaPedidosVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaPedidosVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaPedidosVacia.IsSuccess = false;

          return ListaPedidosVacia;
        }
        else
        {
          var pedidosQuery = _context.Pedidos
      .Where(x => x.Verificado == true && x.Fecha.AddHours(-3) >= request.fechaDesde && x.Fecha.AddHours(-3) <= request.fechaHasta); // Filtrar por fecha

          // Aplicar filtros opcionales si se proporcionan en la consulta
          if (request.IdVendedor.HasValue)
          {
              if (request.IdVendedor.Value == -1)
              {
                  pedidosQuery = pedidosQuery.Where(x => x.IdVendedor == null);
              }
              else
              {
                  pedidosQuery = pedidosQuery.Where(x => x.IdVendedor == request.IdVendedor.Value);
              }
          }

          if (request.IdTipoPedido.HasValue)
          {
            pedidosQuery = pedidosQuery.Where(x => x.IdTipoPedido == request.IdTipoPedido.Value);
          }

          if (request.IdMetodoEntrega.HasValue)
          {
            pedidosQuery = pedidosQuery.Where(x => x.IdMetodoEntrega == request.IdMetodoEntrega.Value);
          }

          if (request.IdMetodoPago.HasValue)
          {
            pedidosQuery = pedidosQuery.Where(x => x.IdMetodoPago == request.IdMetodoPago.Value);
          }

          var pedidos = await pedidosQuery
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
            Verificado = x.Verificado,
            Detalles = x.DetallePedidos.Select(d => new DetallePedidoDto
            {
              IdProducto = d.IdProducto,
              Cantidad = d.Cantidad,
              Aclaracion = d.Aclaracion,
              PrecioUnitario = d.PrecioUnitario
            }).ToList()
          })
          .OrderByDescending(x => x.Fecha) // Ordenar por la fecha en orden descendente
          .ToListAsync();

          if (pedidos == null || !pedidos.Any())
          {
            var ListaPedidosVacia = new ListaEstadisticasPedidosDto();

            ListaPedidosVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaPedidosVacia.ErrorMessage = "No hay pedidos verificados entre esas fechas";
            ListaPedidosVacia.IsSuccess = false;

            return ListaPedidosVacia;
          }
          else
          {
            var listaEstadisticasPedidosDto = new ListaEstadisticasPedidosDto();
            listaEstadisticasPedidosDto.Pedidos = pedidos;

            listaEstadisticasPedidosDto.CantidadPedidos = pedidos.Count;
            listaEstadisticasPedidosDto.CantidadProductos = pedidos.Sum(p => p.CantidadProductos);
            listaEstadisticasPedidosDto.MontoTotalFacturado = pedidos.Sum(p => p.Total);
            listaEstadisticasPedidosDto.PromedioMontoTotalFacturado = listaEstadisticasPedidosDto.PromedioMontoTotalFacturado = pedidos.Any() ? pedidos.Average(p => p.Total) : 0;
            listaEstadisticasPedidosDto.CantidadClientes = pedidos.Select(p => p.Cliente).Distinct().Count();

            #region Vendedores con mas pedidos
            #region Primer vendedor con mas pedidos
            listaEstadisticasPedidosDto.PrimerVendedorMasVentas = pedidos
            .Where(p => p.Vendedor != null) // Filtrar los pedidos con vendedor no nulo
            .GroupBy(p => p.Vendedor)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

            listaEstadisticasPedidosDto.CantidadVentasPrimerVendedor = pedidos
            .Where(p => p.Vendedor != null)
            .GroupBy(p => p.Vendedor)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Count())
            .FirstOrDefault();
            #endregion

            #region Segundo vendedor con mas pedidos
            listaEstadisticasPedidosDto.SegundoVendedorMasVentas = pedidos
            .Where(p => p.Vendedor != null)
            .GroupBy(p => p.Vendedor)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Skip(1)
            .FirstOrDefault();

            listaEstadisticasPedidosDto.CantidadVentasSegundoVendedor = pedidos
            .Where(p => p.Vendedor != null)
            .GroupBy(p => p.Vendedor)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Count())
            .Skip(1)
            .FirstOrDefault();
            #endregion

            #region Tercer vendedor con mas pedidos
            listaEstadisticasPedidosDto.TercerVendedorMasVentas = pedidos
            .Where(p => p.Vendedor != null)
            .GroupBy(p => p.Vendedor)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Skip(2)
            .FirstOrDefault();

            listaEstadisticasPedidosDto.CantidadVentasTercerVendedor = pedidos
            .Where(p => p.Vendedor != null)
            .GroupBy(p => p.Vendedor)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Count())
            .Skip(2)
            .FirstOrDefault();
            #endregion
            #endregion

            #region Productos mas vendidos
            #region Primer producto mas vendido
            listaEstadisticasPedidosDto.PrimerProductoMasVendido = pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto) // Agrupar por ID de producto
            .OrderByDescending(g => g.Sum(d => d.Cantidad)) // Ordenar en orden descendente por cantidad vendida
            .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) }) // Obtener el ID del producto y la cantidad vendida
            .Join(_context.Productos, // Unir con la tabla de Productos para obtener el nombre del producto
                  detalle => detalle.IdProducto, // Clave de unión del DTO DetallePedidoDto
                  producto => producto.IdProducto, // Clave de unión de la tabla de Productos
                  (detalle, producto) => new { NombreProducto = producto.Nombre }) // Proyectar el nombre del producto
            .FirstOrDefault()?.NombreProducto; // Obtener el nombre del primer producto más vendido

            listaEstadisticasPedidosDto.CantidadPrimerProductoVendidos =
            pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto) // Agrupar por ID de producto
            .OrderByDescending(g => g.Sum(d => d.Cantidad)) // Ordenar en orden descendente por la suma de la cantidad vendida
            .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0; // Obtener la suma de la cantidad del primer producto más vendido o establecer en 0 si no hay datos

            listaEstadisticasPedidosDto.UrlPrimerProductoMasVendido =
            pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
            .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
            .Join(_context.Productos,
                  detalle => detalle.IdProducto,
                  producto => producto.IdProducto,
                  (detalle, producto) => new { Producto = producto, CantidadVendida = detalle.Cantidad })
            .FirstOrDefault()?.Producto.UrlImagen;
            #endregion

            #region Segundo producto mas vendido
            listaEstadisticasPedidosDto.SegundoProductoMasVendido =
            pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
            .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
            .Skip(1) // Saltar el primer producto más vendido para obtener el segundo
            .Join(_context.Productos,
                  detalle => detalle.IdProducto,
                  producto => producto.IdProducto,
                  (detalle, producto) => new { NombreProducto = producto.Nombre })
            .FirstOrDefault()?.NombreProducto;

            listaEstadisticasPedidosDto.CantidadSegundoProductoVendidos = pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
            .Skip(1) // Saltar el primer producto más vendido para obtener el segundo
            .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0;

            listaEstadisticasPedidosDto.UrlSegundoProductoMasVendido = pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
            .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
            .Skip(1) // Saltar el primer producto más vendido
            .Join(_context.Productos,
                  detalle => detalle.IdProducto,
                  producto => producto.IdProducto,
                  (detalle, producto) => new { Producto = producto, CantidadVendida = detalle.Cantidad })
            .FirstOrDefault()?.Producto.UrlImagen;
            #endregion

            #region Tercer producto mas vendido
            listaEstadisticasPedidosDto.TercerProductoMasVendido = pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
            .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
            .Skip(2) // Saltar el primer y segundo producto más vendido para obtener el tercero
            .Join(_context.Productos,
                  detalle => detalle.IdProducto,
                  producto => producto.IdProducto,
                  (detalle, producto) => new { NombreProducto = producto.Nombre })
            .FirstOrDefault()?.NombreProducto;

            listaEstadisticasPedidosDto.CantidadTercerProductoVendidos = pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
            .Skip(2) // Saltar el primer y segundo producto más vendido para obtener el tercero
            .FirstOrDefault()?.Sum(d => d.Cantidad) ?? 0;

            listaEstadisticasPedidosDto.UrlTercerProductoMasVendido = pedidos.SelectMany(p => p.Detalles)
            .GroupBy(d => d.IdProducto)
            .OrderByDescending(g => g.Sum(d => d.Cantidad))
             .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
            .Skip(2) // Saltar el primer y segundo producto más vendido para obtener el tercero
            .Join(_context.Productos,
                  detalle => detalle.IdProducto,
                  producto => producto.IdProducto,
                  (detalle, producto) => new { Producto = producto, CantidadVendida = detalle.Cantidad })
            .FirstOrDefault()?.Producto.UrlImagen;
            #endregion
            #endregion

            #region Categorías mas aparecidas
            var pedidosFiltrados = await pedidosQuery
            .Include(x => x.DetallePedidos)
                .ThenInclude(d => d.IdProductoNavigation.IdCategoriaNavigation) // Incluir la relación de categoría de producto
            .ToListAsync();

            // Obtener la lista de categorías más frecuentes y sus apariciones en los pedidos filtrados
            var categoriasMasFrecuentes = pedidosFiltrados
            .SelectMany(pedido => pedido.DetallePedidos
                .GroupBy(detalle => new { detalle.IdProductoNavigation.IdCategoria, pedido.IdPedido })) // Agrupa por ID de categoría y de pedido
            .GroupBy(grupo => grupo.Key.IdCategoria) // Agrupa por ID de categoría
            .Select(group => new
            {
              IdCategoria = group.Key,
              NombreCategoria = group.First().First().IdProductoNavigation.IdCategoriaNavigation.Nombre, // Suponiendo que el nombre de la categoría está en la primera instancia del primer detalle del primer pedido del grupo
              Apariciones = group.Count()
            })
            .OrderByDescending(resultado => resultado.Apariciones)
            .Take(3)
            .ToList();

            // Asignar los nombres de las categorías más frecuentes a las propiedades del DTO
            if (categoriasMasFrecuentes.Count > 0)
            {
              listaEstadisticasPedidosDto.PrimerCategoriaMasAparecida = categoriasMasFrecuentes[0].NombreCategoria;
              listaEstadisticasPedidosDto.CantidadPrimerCategoriaMasAparecida = categoriasMasFrecuentes[0].Apariciones;
            }

            if (categoriasMasFrecuentes.Count > 1)
            {
              listaEstadisticasPedidosDto.SegundaCategoriaMasAparecida = categoriasMasFrecuentes[1].NombreCategoria;
              listaEstadisticasPedidosDto.CantidadSegundaCategoriaMasAparecida = categoriasMasFrecuentes[1].Apariciones;
            }

            if (categoriasMasFrecuentes.Count > 2)
            {
              listaEstadisticasPedidosDto.TerceraCategoriaMasAparecida = categoriasMasFrecuentes[2].NombreCategoria;
              listaEstadisticasPedidosDto.CantidadTerceraCategoriaMasAparecida = categoriasMasFrecuentes[2].Apariciones;
            }

            #endregion

            listaEstadisticasPedidosDto.StatusCode = StatusCodes.Status200OK;
            listaEstadisticasPedidosDto.IsSuccess = true;
            listaEstadisticasPedidosDto.ErrorMessage = "";

            return listaEstadisticasPedidosDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaPedidosVacia = new ListaEstadisticasPedidosDto();

        ListaPedidosVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaPedidosVacia.ErrorMessage = ex.Message;
        ListaPedidosVacia.IsSuccess = false;

        return ListaPedidosVacia;
      }
    }

  }
}
