using API.Data;
using API.Dtos.EnvioDto;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.EnvioServices.Commands.CreateEnvioCommand
{
  public class CreateEnvioCommandHandler : IRequestHandler<CreateEnvioCommand, EnvioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEnvioCommand> _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateEnvioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreateEnvioCommand> validator, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<EnvioDto> Handle(CreateEnvioCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var FormaEnvioVacia = new EnvioDto();

          FormaEnvioVacia.StatusCode = StatusCodes.Status400BadRequest;
          FormaEnvioVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          FormaEnvioVacia.IsSuccess = false;

          return FormaEnvioVacia;
        }
        else
        {
          var formaEnvioToCreate = _mapper.Map<Envio>(request);

          formaEnvioToCreate.UltimoModificador = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
          formaEnvioToCreate.FechaModificacion = DateTimeOffset.Now.ToUniversalTime();

          await _context.AddAsync(formaEnvioToCreate);
          await _context.SaveChangesAsync();

          var envioDto = _mapper.Map<EnvioDto>(formaEnvioToCreate);

          envioDto.StatusCode = StatusCodes.Status200OK;
          envioDto.IsSuccess = true;
          envioDto.ErrorMessage = string.Empty;

          return envioDto;
        }
      }
      catch (Exception ex)
      {
        var FormaEnvioVacia = new EnvioDto();

        FormaEnvioVacia.StatusCode = StatusCodes.Status400BadRequest;
        FormaEnvioVacia.ErrorMessage = ex.Message;
        FormaEnvioVacia.IsSuccess = false;

        return FormaEnvioVacia;
      }
    }

  }
}
