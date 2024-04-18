using API.Data;
using API.Dtos.StockDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.StockServices.Commands.CreateDetalleStockCommand
{
  public class CreateDetalleStockCommandHandler : IRequestHandler<CreateDetalleStockCommand, StockDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateDetalleStockCommand> _validator;

    public CreateDetalleStockCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreateDetalleStockCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<StockDto> Handle(CreateDetalleStockCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var DetalleStockVacio = new StockDto();

          DetalleStockVacio.StatusCode = StatusCodes.Status400BadRequest;
          DetalleStockVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          DetalleStockVacio.IsSuccess = false;

          return DetalleStockVacio;
        }
        else
        {
          var detalleStockToCreate = _mapper.Map<DetallesStock>(request);

          detalleStockToCreate.Fecha = DateTimeOffset.Now.ToUniversalTime();

          await _context.AddAsync(detalleStockToCreate);
          await _context.SaveChangesAsync();

          var cuentaDto = _mapper.Map<StockDto>(detalleStockToCreate);

          cuentaDto.StatusCode = StatusCodes.Status200OK;
          cuentaDto.IsSuccess = true;
          cuentaDto.ErrorMessage = string.Empty;

          return cuentaDto;
        }
      }
      catch (Exception ex)
      {
        var DetalleStockVacio = new StockDto();

        DetalleStockVacio.StatusCode = StatusCodes.Status400BadRequest;
        DetalleStockVacio.ErrorMessage = ex.Message;
        DetalleStockVacio.IsSuccess = false;

        return DetalleStockVacio;
      }
    }

  }
}
