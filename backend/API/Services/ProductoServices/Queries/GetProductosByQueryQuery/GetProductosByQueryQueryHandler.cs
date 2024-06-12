using API.Data;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductosByQueryQuery
{
  public class GetProductosByQueryQueryHandler : IRequestHandler<GetProductosByQueryQuery, ListaProductosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetProductosByQueryQuery> _validator;

    public GetProductosByQueryQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetProductosByQueryQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaProductosDto> Handle(GetProductosByQueryQuery request, CancellationToken cancellationToken)
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
              .Where(x => (x.Nombre.ToLower().Contains(request.query.ToLower()) || x.Descripcion.ToLower().Contains(request.query.ToLower())) && x.Ocultar == false)
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
            ListaProductosVacia.ErrorMessage = "No hay productos con esa consulta";
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
