using API.Data;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Services.ProductoServices.Queries.GetProductosByCategoryQuery
{
  public class GetProductosByCategoryQueryHandler : IRequestHandler<GetProductosByCategoryQuery, ListaProductosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetProductosByCategoryQuery> _validator;

    public GetProductosByCategoryQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetProductosByCategoryQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaProductosDto> Handle(GetProductosByCategoryQuery request, CancellationToken cancellationToken)
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
          IQueryable<Producto> productosQuery;

          // Verificar si la categoría es "Promociones" o "Destacados"
          if (request.category.Equals("Promociones", StringComparison.OrdinalIgnoreCase))
          {
              productosQuery = _context.Productos
                  .Where(x => x.Ocultar == false && x.EnPromocion == true);
          }
          else if (request.category.Equals("Destacados", StringComparison.OrdinalIgnoreCase))
          {
              productosQuery = _context.Productos
                  .Where(x => x.Ocultar == false && x.EnDestacado == true);
          }
          else
          {
              productosQuery = _context.Productos
                  .Where(x => x.IdCategoriaNavigation.Nombre == request.category && x.Ocultar == false && x.IdSubcategoria == null);
          }
          var cotizacionDolar = await _context.Cotizaciones.FirstAsync();
          float valorDolar = cotizacionDolar.Precio;

          var productos = await productosQuery
              .Select(x => new ListaProductoDto
              {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Divisa = x.IdDivisaNavigation.Nombre,
                PrecioPesos = request.client == 3 ? 0 : CalcularPrecioFinalEnPesos(request.client, x.Precio, x.PorcentajeMinorista, x.PorcentajeMayorista, x.IdDivisaNavigation.Nombre, valorDolar, x.PrecioMinorista, x.PrecioMayorista),
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
            ListaProductosVacia.ErrorMessage = "No hay productos con esa categoría";
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

    // Definición de la función para calcular el precio final en pesos
    public static float CalcularPrecioFinalEnPesos(int clientType, float costo, float porcentajeMinorista, float porcentajeMayorista, String divisa, float valorDolar, float precioMinorista, float precioMayorista) {
    float precioFinal = 0;

    // Verificar si ya hay un precio asignado en PrecioMinorista o PrecioMayorista
    if (clientType == 1 && precioMinorista > 0) {
        precioFinal = precioMinorista;
    } else if (clientType == 2 && precioMayorista > 0) {
        precioFinal = precioMayorista;
    } else {
        // Calcular el precio final en pesos si no hay precio asignado
        if (clientType == 1) {
            if (divisa == "Dólar") {
                if (precioMinorista == 0 && porcentajeMinorista == 0) {
                    precioFinal = 0;
                    return precioFinal;
                }
                precioFinal = (float) Math.Round(costo * valorDolar * (1 + porcentajeMinorista / 100));
            } else {
                if (precioMinorista == 0 && porcentajeMinorista == 0) {
                    precioFinal = 0;
                    return precioFinal;
                }
                precioFinal = (float) Math.Ceiling(costo * (1 + porcentajeMinorista / 100));
            }
        } else // clientType == "Mayorista"
        {
            if (divisa == "Dólar") {
                if (precioMayorista == 0 && porcentajeMayorista == 0) {
                    precioFinal = 0;
                    return precioFinal;
                }
                precioFinal = (float) Math.Round(costo * valorDolar * (1 + porcentajeMayorista / 100));
            } else {
                if (precioMayorista == 0 && porcentajeMayorista == 0) {
                    precioFinal = 0;
                    return precioFinal;
                }
                precioFinal = (float) Math.Ceiling(costo * (1 + porcentajeMayorista / 100));
            }
        }
    }

    return precioFinal;
}
  }
}
