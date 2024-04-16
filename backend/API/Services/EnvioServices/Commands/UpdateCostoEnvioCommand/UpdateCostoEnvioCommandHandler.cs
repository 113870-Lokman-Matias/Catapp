using API.Data;
using API.Dtos.EnvioDto;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand
{
    public class UpdateCostoEnvioCommandHandler : IRequestHandler<UpdateCostoEnvioCommand, EnvioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCostoEnvioCommand> _validator;
        public UpdateCostoEnvioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateCostoEnvioCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<EnvioDto> Handle(UpdateCostoEnvioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var CostoEnvioVacio = new EnvioDto();

                    CostoEnvioVacio.StatusCode = StatusCodes.Status400BadRequest;
                    CostoEnvioVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    CostoEnvioVacio.IsSuccess = false;

                    return CostoEnvioVacio;
                }
                else
                {

                    var CostoEnvioToUpdate = await _context.Envios.FirstAsync();

                    if (CostoEnvioToUpdate == null)
                    {
                        var CostoEnvioVacio = new EnvioDto();

                        CostoEnvioVacio.StatusCode = StatusCodes.Status404NotFound;
                        CostoEnvioVacio.ErrorMessage = "El costo de envio no existe";
                        CostoEnvioVacio.IsSuccess = false;

                        return CostoEnvioVacio;
                    }
                    else
                    {
                        CostoEnvioToUpdate.Precio = request.Precio;
                        CostoEnvioToUpdate.UltimoModificador = request.UltimoModificador;
                        CostoEnvioToUpdate.FechaModificacion = DateTimeOffset.Now.ToUniversalTime();

                        await _context.SaveChangesAsync();

                        var envioDto = _mapper.Map<EnvioDto>(CostoEnvioToUpdate);

                        envioDto.StatusCode = StatusCodes.Status200OK;
                        envioDto.IsSuccess = true;
                        envioDto.ErrorMessage = "";

                        return envioDto;
                    }

                }
            }
            catch (Exception ex)
            {
                var CostoEnvioVacio = new EnvioDto();

                CostoEnvioVacio.StatusCode = StatusCodes.Status400BadRequest;
                CostoEnvioVacio.ErrorMessage = ex.Message;
                CostoEnvioVacio.IsSuccess = false;

                return CostoEnvioVacio;
            }
        }
    }
}
