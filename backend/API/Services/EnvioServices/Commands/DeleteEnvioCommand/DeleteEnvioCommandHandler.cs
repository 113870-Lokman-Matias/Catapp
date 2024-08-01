using API.Data;
using API.Dtos.EnvioDto;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.EnvioServices.Commands.DeleteEnvioCommand
{
  public class DeleteEnvioCommandHandler : IRequestHandler<DeleteEnvioCommand, EnvioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteEnvioCommand> _validator;

    public DeleteEnvioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeleteEnvioCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<EnvioDto> Handle(DeleteEnvioCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var FormaEntregaVacia = new EnvioDto();

          FormaEntregaVacia.StatusCode = StatusCodes.Status400BadRequest;
          FormaEntregaVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          FormaEntregaVacia.IsSuccess = false;

          return FormaEntregaVacia;
        }
        else
        {
          var FormaEntregaToDelete = await _context.Envios.FindAsync(request.IdEnvio);

          if (FormaEntregaToDelete == null)
          {
            var FormaEntregaVacia = new EnvioDto();

            FormaEntregaVacia.StatusCode = StatusCodes.Status404NotFound;
            FormaEntregaVacia.ErrorMessage = "La forma de entrega no existe";
            FormaEntregaVacia.IsSuccess = false;

            return FormaEntregaVacia;
          }
          else
          {
              _context.Envios.Remove(FormaEntregaToDelete);
              await _context.SaveChangesAsync();

              var envioDto = _mapper.Map<EnvioDto>(FormaEntregaToDelete);

              envioDto.StatusCode = StatusCodes.Status200OK;
              envioDto.IsSuccess = true;
              envioDto.ErrorMessage = "";

              return envioDto;
          }
        }
      }
      catch (Exception ex)
      {
        var FormaEntregaVacia = new EnvioDto();

        FormaEntregaVacia.StatusCode = StatusCodes.Status400BadRequest;
        FormaEntregaVacia.ErrorMessage = ex.Message;
        FormaEntregaVacia.IsSuccess = false;

        return FormaEntregaVacia;
      }
    }

  }
}
