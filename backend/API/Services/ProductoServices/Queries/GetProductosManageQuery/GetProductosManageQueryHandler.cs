using API.Data;
using API.Dtos.CategoriaDtos;
using API.Dtos.ProductoDtos;
using API.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductosManageQuery
{
  public class GetProductosManageQueryHandler : IRequestHandler<GetProductosManageQuery, ListaProductosManageDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;

    public GetProductosManageQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaProductosManageDto> Handle(GetProductosManageQuery request, CancellationToken cancellationToken)
    {
      try
      {
        IQueryable<Producto> query = _context.Productos;

        // Apply filters based on the request parameters
        if (!string.IsNullOrEmpty(request.Query))
        {
          query = query.Where(x => x.Nombre.ToLower().Contains(request.Query.ToLower()) || x.Descripcion.ToLower().Contains(request.Query.ToLower()));
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            if (request.Category.Equals("Promociones", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.EnPromocion == true);
            }
            else if (request.Category.Equals("Destacados", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.EnDestacado == true);
            }
            else
            {
                query = query.Where(x => x.IdCategoriaNavigation.Nombre == request.Category);
            }
        }

        if (request.Hidden.HasValue)
        {
          query = query.Where(x => x.Ocultar == request.Hidden.Value);
        }

        var productosManage = await query
            .Select(x => new ProductoManageDto
            {
              IdProducto = x.IdProducto,
              Nombre = x.Nombre,
              Descripcion = x.Descripcion,
              Divisa = x.IdDivisaNavigation.Nombre,
              Precio = x.Precio,
              PorcentajeMinorista = x.PorcentajeMinorista,
              PorcentajeMayorista = x.PorcentajeMayorista,
              PrecioMinorista = x.PrecioMinorista,
              PrecioMayorista = x.PrecioMayorista,
              Stock = x.Stock,
              IdCategoria = x.IdCategoria,
              NombreCategoria = x.IdCategoriaNavigation.Nombre,
              IdImagen = x.IdImagen,
              UrlImagen = x.UrlImagen,
              Ocultar = x.Ocultar,
              EnPromocion = x.EnPromocion,
              EnDestacado = x.EnDestacado,
              IdDivisa = x.IdDivisa,
              StockTransitorio = x.StockTransitorio,
              IdSubcategoria = x.IdSubcategoria,
              NombreSubcategoria = x.IdSubcategoriaNavigation.Nombre
            })
            .OrderBy(x => x.Ocultar)
            .ThenBy(x => x.StockTransitorio)
            .ThenBy(x => x.NombreCategoria)
            .ThenBy(x => x.Nombre)
            .ToListAsync();

        if (productosManage.Count > 0)
        {
          var listaProductosManageDto = new ListaProductosManageDto();
          listaProductosManageDto.Productos = productosManage;

          listaProductosManageDto.StatusCode = StatusCodes.Status200OK;
          listaProductosManageDto.ErrorMessage = string.Empty;
          listaProductosManageDto.IsSuccess = true;

          return listaProductosManageDto;
        }
        else
        {
          var listaProductosManageVacia = new ListaProductosManageDto();

          listaProductosManageVacia.StatusCode = StatusCodes.Status404NotFound;
          listaProductosManageVacia.ErrorMessage = "No se han encontrado productos";
          listaProductosManageVacia.IsSuccess = false;

          return listaProductosManageVacia;
        }
      }
      catch (Exception ex)
      {
        var listaProductosManageVacia = new ListaProductosManageDto();

        listaProductosManageVacia.StatusCode = StatusCodes.Status400BadRequest;
        listaProductosManageVacia.ErrorMessage = ex.Message;
        listaProductosManageVacia.IsSuccess = false;

        return listaProductosManageVacia;
      }
    }

  }
}
