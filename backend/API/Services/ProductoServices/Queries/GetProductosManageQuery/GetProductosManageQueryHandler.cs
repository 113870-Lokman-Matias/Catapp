using API.Data;
using API.Dtos.CategoriaDtos;
using API.Dtos.ProductoDtos;
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
        var productosManage = await _context.Productos
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
              IdDivisa = x.IdDivisa,
              StockTransitorio = x.StockTransitorio
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
