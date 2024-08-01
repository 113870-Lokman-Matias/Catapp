using API.Data;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Queries.GetProductoByIdQuery
{
    public class GetProductoByIdQueryHandler : IRequestHandler<GetProductoByIdQuery, ProductoDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<GetProductoByIdQuery> _validator;

        public GetProductoByIdQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetProductoByIdQuery> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ProductoDto> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var ProductoVacio = new ProductoDto
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage)),
                        IsSuccess = false
                    };

                    return ProductoVacio;
                }
                else
                {
                    var cotizacionDolar = await _context.Cotizaciones.FirstAsync();
                    float valorDolar = cotizacionDolar.Precio;

                    var productoId = request.id;

                    var producto = await _context.Productos
                    .Where(x => x.IdProducto == productoId)
                    .Include(x => x.IdDivisaNavigation)
                    .Include(x => x.IdCategoriaNavigation)
                    .Include(x => x.IdSubcategoriaNavigation)
                    .Select(x => new ProductoDto
                    {
                        IdProducto = x.IdProducto,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Divisa = x.IdDivisaNavigation.Nombre,
                        PrecioPesos = CalcularPrecioFinalEnPesos(request.client, x.Precio, x.PorcentajeMinorista, x.PorcentajeMayorista, x.IdDivisaNavigation.Nombre, valorDolar, x.PrecioMinorista, x.PrecioMayorista),
                        Stock = x.Stock,
                        NombreCategoria = x.IdCategoriaNavigation.Nombre,
                        IdImagen = x.IdImagen,
                        UrlImagen = x.UrlImagen,
                        Ocultar = x.Ocultar,
                        IdDivisa = x.IdDivisa,
                        StockTransitorio = x.StockTransitorio,
                        NombreSubcategoria = x.IdSubcategoriaNavigation.Nombre
                    })
                    .FirstOrDefaultAsync();

                    if (producto == null)
                    {
                        var ProductoVacio = new ProductoDto
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            ErrorMessage = "No existe el producto",
                            IsSuccess = false
                        };

                        return ProductoVacio;
                    }
                    else
                    {
                        var productoDto = _mapper.Map<ProductoDto>(producto);

                        return productoDto;
                    }
                }
            }
            catch (Exception ex)
            {
                var ProductoVacio = new ProductoDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };

                return ProductoVacio;
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