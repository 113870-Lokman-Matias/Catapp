using API.Data;
using API.Dtos.MetodoPagoDto;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.MetodoPagoServices.Commands.CreateMetodoPagoCommand
{
  public class CreateMetodoPagoCommandHandler : IRequestHandler<CreateMetodoPagoCommand, MetodoPagoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateMetodoPagoCommand> _validator;

    public CreateMetodoPagoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreateMetodoPagoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<MetodoPagoDto> Handle(CreateMetodoPagoCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var MetodoPagoVacio = new MetodoPagoDto();

          MetodoPagoVacio.StatusCode = StatusCodes.Status400BadRequest;
          MetodoPagoVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          MetodoPagoVacio.IsSuccess = false;

          return MetodoPagoVacio;
        }
        else
        {
          var metodoPagoToCreate = _mapper.Map<MetodosPago>(request);
          await _context.AddAsync(metodoPagoToCreate);
          await _context.SaveChangesAsync();

          var metodoPagoDto = _mapper.Map<MetodoPagoDto>(metodoPagoToCreate);

          metodoPagoDto.StatusCode = StatusCodes.Status200OK;
          metodoPagoDto.IsSuccess = true;
          metodoPagoDto.ErrorMessage = string.Empty;

          return metodoPagoDto;
        }
      }
      catch (Exception ex)
      {
        var MetodoPagoVacio = new MetodoPagoDto();

        MetodoPagoVacio.StatusCode = StatusCodes.Status400BadRequest;
        MetodoPagoVacio.ErrorMessage = ex.Message;
        MetodoPagoVacio.IsSuccess = false;

        return MetodoPagoVacio;
      }
    }

  }
}
