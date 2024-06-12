using API.Data;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Services.ProductoServices.Queries.GetProductosBySubcategoryQuery
{
  public class GetProductosBySubcategoryQueryHandler : IRequestHandler<GetProductosBySubcategoryQuery, ListaProductosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetProductosBySubcategoryQuery> _validator;

    public GetProductosBySubcategoryQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetProductosBySubcategoryQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaProductosDto> Handle(GetProductosBySubcategoryQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaProductosVacia = new ListaProductosDto();

          ListaProductosVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaProductosVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaProductosVacia.IsSuccess = false;

          return ListaProductosVacia;
        }
        else
        {
          var productos = await _context.Productos
              .Where(x => x.IdCategoriaNavigation.IdCategoria == request.idCategory && x.IdSubcategoriaNavigation.IdSubcategoria == request.idSubcategory && x.Ocultar == false)
              .Select(x => new ListaProductoDto
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
                NombreCategoria = x.IdCategoriaNavigation.Nombre,
                UrlImagen = x.UrlImagen,
                StockTransitorio = x.StockTransitorio,
                NombreSubcategoria = x.IdSubcategoriaNavigation.Nombre
              })
              .OrderBy(x => x.Nombre)
              .ToListAsync();

          if (productos == null)
          {
            var ListaProductosVacia = new ListaProductosDto();

            ListaProductosVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaProductosVacia.ErrorMessage = "No hay productos con esa subcategoría";
            ListaProductosVacia.IsSuccess = false;

            return ListaProductosVacia;
          }
          else
          {
            var listaProductosDto = new ListaProductosDto();
            listaProductosDto.Productos = productos;

            listaProductosDto.StatusCode = StatusCodes.Status200OK;
            listaProductosDto.IsSuccess = true;
            listaProductosDto.ErrorMessage = "";

            return listaProductosDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaProductosVacia = new ListaProductosDto();

        ListaProductosVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaProductosVacia.ErrorMessage = ex.Message;
        ListaProductosVacia.IsSuccess = false;

        return ListaProductosVacia;
      }
    }

  }
}
