using API.Data;
using API.Dtos.MetodoPagoDto;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentValidation;
using MediatR;

namespace API.Services.MetodoPagoServices.Commands.DeleteMetodoPagoCommand
{
  public class DeleteMetodoPagoCommandHandler : IRequestHandler<DeleteMetodoPagoCommand, MetodoPagoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteMetodoPagoCommand> _validator;

    public DeleteMetodoPagoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeleteMetodoPagoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<MetodoPagoDto> Handle(DeleteMetodoPagoCommand request, CancellationToken cancellationToken)
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
          var MetodoPagoToDelete = await _context.MetodosPagos.FindAsync(request.IdMetodoPago);

          if (MetodoPagoToDelete == null)
          {
            var MetodoPagoVacio = new MetodoPagoDto();

            MetodoPagoVacio.StatusCode = StatusCodes.Status404NotFound;
            MetodoPagoVacio.ErrorMessage = "El metodo de pago no existe";
            MetodoPagoVacio.IsSuccess = false;

            return MetodoPagoVacio;
          }
          else
          {
              _context.MetodosPagos.Remove(MetodoPagoToDelete);
              await _context.SaveChangesAsync();

              var metodoPagoDto = _mapper.Map<MetodoPagoDto>(MetodoPagoToDelete);

              metodoPagoDto.StatusCode = StatusCodes.Status200OK;
              metodoPagoDto.IsSuccess = true;
              metodoPagoDto.ErrorMessage = "";

              return metodoPagoDto;
          }
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
