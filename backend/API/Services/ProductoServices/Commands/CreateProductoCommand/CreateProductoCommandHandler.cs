﻿using API.Data;
using API.Dtos.ProductoDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.ProductoServices.Commands.CreateProductoCommand
{
  public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, ProductoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductoCommand> _validator;

    public CreateProductoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreateProductoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ProductoDto> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
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
          var productoToCreate = _mapper.Map<Producto>(request);

          productoToCreate.StockTransitorio = request.Stock;
          if (request.IdSubcategoria == -1)
          {
            productoToCreate.IdSubcategoria = null; // Asignar null si el valor es -1 (representando "ninguno")
          }
          else
          {
            productoToCreate.IdSubcategoria = request.IdSubcategoria; // Asignar el valor normal
          }

          _context.Attach(productoToCreate);

          await _context.Entry(productoToCreate)
              .Reference(p => p.IdCategoriaNavigation)
              .LoadAsync();

          await _context.AddAsync(productoToCreate);
          await _context.SaveChangesAsync();

          var productoDto = _mapper.Map<ProductoDto>(productoToCreate);

          productoDto.StatusCode = StatusCodes.Status200OK;
          productoDto.IsSuccess = true;
          productoDto.ErrorMessage = string.Empty;

          return productoDto;
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
