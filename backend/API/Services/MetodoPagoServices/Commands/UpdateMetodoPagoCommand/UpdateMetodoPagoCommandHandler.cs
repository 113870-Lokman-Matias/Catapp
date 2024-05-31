using API.Data;
using API.Dtos.MetodoPagoDto;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.MetodoPagoServices.Commands.UpdateMetodoPagoCommand
{
  public class UpdateMetodoPagoCommandHandler : IRequestHandler<UpdateMetodoPagoCommand, MetodoPagoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateMetodoPagoCommand> _validator;

    public UpdateMetodoPagoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateMetodoPagoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<MetodoPagoDto> Handle(UpdateMetodoPagoCommand request, CancellationToken cancellationToken)
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
          var MetodoPagoToUpdate = await _context.MetodosPagos.FindAsync(request.IdMetodoPago);

          if (MetodoPagoToUpdate == null)
          {
            var MetodoPagoVacio = new MetodoPagoDto();

            MetodoPagoVacio.StatusCode = StatusCodes.Status404NotFound;
            MetodoPagoVacio.ErrorMessage = "El metodo de pago no existe";
            MetodoPagoVacio.IsSuccess = false;

            return MetodoPagoVacio;
          }
          else
          {
            MetodoPagoToUpdate.Nombre = request.Nombre;
            MetodoPagoToUpdate.Habilitado = request.Habilitado;
            MetodoPagoToUpdate.Disponibilidad = request.Disponibilidad;

            await _context.SaveChangesAsync();

            var metodoPagoDto = _mapper.Map<MetodoPagoDto>(MetodoPagoToUpdate);

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
