using API.Data;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;


namespace API.Services.ProductoServices.Commands.UpdateStockProductoCommand
{
  public class UpdateStockProductoCommandHandler : IRequestHandler<UpdateStockProductoCommand, ProductoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateStockProductoCommand> _validator;

    public UpdateStockProductoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateStockProductoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ProductoDto> Handle(UpdateStockProductoCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ProductoVacio = new ProductoDto();

          ProductoVacio.StatusCode = StatusCodes.Status400BadRequest;
          ProductoVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ProductoVacio.IsSuccess = false;

          return ProductoVacio;
        }
        else
        {
          var ProductoToUpdate = await _context.Productos.FindAsync(request.IdProducto);

          if (ProductoToUpdate == null)
          {
            var ProductoVacio = new ProductoDto();

            ProductoVacio.StatusCode = StatusCodes.Status404NotFound;
            ProductoVacio.ErrorMessage = "El producto no existe";
            ProductoVacio.IsSuccess = false;

            return ProductoVacio;
          }
          else
          {
            // Obtener el stock anterior
            int stockAnterior = ProductoToUpdate.StockTransitorio;

            // Calcular la diferencia
            int diferencia = request.Stock - stockAnterior;

            // Calcular la diferencia
            int primerDiferencia = ProductoToUpdate.Stock - ProductoToUpdate.StockTransitorio;

            // Actualizar el stock y el stock transitorio
            ProductoToUpdate.Stock = request.Stock += primerDiferencia;
            ProductoToUpdate.StockTransitorio += diferencia;

            await _context.SaveChangesAsync();

            _context.Attach(ProductoToUpdate);

            await _context.Entry(ProductoToUpdate)
                .Reference(p => p.IdCategoriaNavigation)
                .LoadAsync();

            var productoDto = _mapper.Map<ProductoDto>(ProductoToUpdate);

            productoDto.StatusCode = StatusCodes.Status200OK;
            productoDto.IsSuccess = true;
            productoDto.ErrorMessage = "";

            return productoDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ProductoVacio = new ProductoDto();

        ProductoVacio.StatusCode = StatusCodes.Status400BadRequest;
        ProductoVacio.ErrorMessage = ex.Message;
        ProductoVacio.IsSuccess = false;

        return ProductoVacio;
      }
    }

  }
}
