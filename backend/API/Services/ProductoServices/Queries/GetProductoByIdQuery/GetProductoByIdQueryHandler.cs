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
                    .Include(x => x.IdDivisaNavigation)
                    .Include(x => x.IdCategoriaNavigation)
                    .Include(x => x.IdSubcategoriaNavigation)
                    .Select(x => new ProductoDto
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
                        IdImagen = x.IdImagen,
                        UrlImagen = x.UrlImagen,
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
    }
}