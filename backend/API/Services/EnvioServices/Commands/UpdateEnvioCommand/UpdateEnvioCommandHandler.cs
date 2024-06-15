using API.Data;
using API.Dtos.EnvioDto;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace API.Services.EnvioServices.Commands.UpdateEnvioCommand
{
    public class UpdateEnvioCommandHandler : IRequestHandler<UpdateEnvioCommand, EnvioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateEnvioCommand> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateEnvioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateEnvioCommand> validator, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EnvioDto> Handle(UpdateEnvioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var EnvioVacio = new EnvioDto();

                    EnvioVacio.StatusCode = StatusCodes.Status400BadRequest;
                    EnvioVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    EnvioVacio.IsSuccess = false;

                    return EnvioVacio;
                }
                else
                {
                    var EnvioToUpdate = await _context.Envios.FindAsync(request.IdEnvio);

                    if (EnvioToUpdate == null)
                    {
                        var EnvioVacio = new EnvioDto();

                        EnvioVacio.StatusCode = StatusCodes.Status404NotFound;
                        EnvioVacio.ErrorMessage = "El costo de envio no existe";
                        EnvioVacio.IsSuccess = false;

                        return EnvioVacio;
                    }
                    else
                    {
                        EnvioToUpdate.Habilitado = request.Habilitado;
                        EnvioToUpdate.Costo = request.Costo;
                        EnvioToUpdate.UltimoModificador = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                        EnvioToUpdate.FechaModificacion = DateTimeOffset.Now.ToUniversalTime();
                        EnvioToUpdate.Nombre = request.Nombre;
                        EnvioToUpdate.DisponibilidadCatalogo = request.DisponibilidadCatalogo;
                        EnvioToUpdate.Aclaracion = request.Aclaracion;

                        await _context.SaveChangesAsync();

                        var envioDto = _mapper.Map<EnvioDto>(EnvioToUpdate);

                        envioDto.StatusCode = StatusCodes.Status200OK;
                        envioDto.IsSuccess = true;
                        envioDto.ErrorMessage = "";

                        return envioDto;
                    }

                }
            }
            catch (Exception ex)
            {
                var EnvioVacio = new EnvioDto();

                EnvioVacio.StatusCode = StatusCodes.Status400BadRequest;
                EnvioVacio.ErrorMessage = ex.Message;
                EnvioVacio.IsSuccess = false;

                return EnvioVacio;
            }
        }
    }
}
