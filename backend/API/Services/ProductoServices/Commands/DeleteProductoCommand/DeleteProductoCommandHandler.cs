﻿using API.Data;
using API.Dtos.ProductoDtos;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentValidation;
using MediatR;

namespace API.Services.ProductoServices.Commands.DeleteProductoCommand
{
  public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, ProductoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteProductoCommand> _validator;

    public DeleteProductoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeleteProductoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ProductoDto> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
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
          var ProductoToDelete = await _context.Productos.FindAsync(request.IdProducto);

          if (ProductoToDelete == null)
          {
            var ProductoVacio = new ProductoDto();

            ProductoVacio.StatusCode = StatusCodes.Status404NotFound;
            ProductoVacio.ErrorMessage = "El producto no existe";
            ProductoVacio.IsSuccess = false;

            return ProductoVacio;
          }
          else
          {
            var account = new Account(
                            "dvkgn8snm",
                            "184735187823824",
                            "mVZ1y5A4-JrgcjnfK6q7Eb3sTgk");

            var cloudinary = new Cloudinary(account);

            // Crear un objeto DeletionParams con el publicId de la imagen
            var deletionParams = new DeletionParams(ProductoToDelete.IdImagen);

            // Eliminar la imagen de Cloudinary
            var deletionResult = cloudinary.Destroy(deletionParams);

            // Verificar si se eliminó correctamente la imagen
            if (deletionResult.StatusCode == System.Net.HttpStatusCode.OK)
            {

              // Eliminar todos los detalles de stock asociados al producto
              var detallesStockAEliminar = _context.DetallesStocks.Where(ds => ds.IdProducto == request.IdProducto);
              _context.DetallesStocks.RemoveRange(detallesStockAEliminar);

              _context.Productos.Remove(ProductoToDelete);
              await _context.SaveChangesAsync();

              _context.Attach(ProductoToDelete);

              await _context.Entry(ProductoToDelete)
                  .Reference(p => p.IdCategoriaNavigation)
                  .LoadAsync();

              var productoDto = _mapper.Map<ProductoDto>(ProductoToDelete);

              productoDto.StatusCode = StatusCodes.Status200OK;
              productoDto.IsSuccess = true;
              productoDto.ErrorMessage = "";

              return productoDto;
            }
            else
            {
              // Si falla la eliminación de la imagen en Cloudinary, devuelve un error
              var ProductoVacio = new ProductoDto();

              ProductoVacio.StatusCode = StatusCodes.Status400BadRequest;
              ProductoVacio.ErrorMessage = deletionResult.Error.Message;
              ProductoVacio.IsSuccess = false;

              return ProductoVacio;
            }
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
