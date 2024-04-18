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
                    var productoId = request.id;

                    var producto = await _context.Productos
                    .Where(x => x.IdProducto == productoId)
                    .Select(x => new ProductoDto
                    {
                        IdProducto = x.IdProducto,
                        Nombre = x.Nombre,
                        Stock = x.Stock,
                        UrlImagen = x.UrlImagen,
                        StockTransitorio = x.StockTransitorio
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
                        var productoDto = new ProductoDto
                        {
                            IdProducto = producto.IdProducto,
                            Nombre = producto.Nombre,
                            Stock = producto.Stock,
                            UrlImagen = producto.UrlImagen,
                            StockTransitorio = producto.StockTransitorio,
                            StatusCode = StatusCodes.Status200OK,
                            IsSuccess = true,
                            ErrorMessage = ""
                        };

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
    }
}