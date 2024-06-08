using API.Data;
using API.Dtos.ConfiguracionDto;
using API.Dtos.ProductoDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace API.Services.ConfiguracionServices.Commands.UpdateConfiguracionCommand
{
    public class UpdateConfiguracionCommandHandler : IRequestHandler<UpdateConfiguracionCommand, ConfiguracionDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateConfiguracionCommand> _validator;

        public UpdateConfiguracionCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateConfiguracionCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ConfiguracionDto> Handle(UpdateConfiguracionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var ConfiguracionVacia = new ConfiguracionDto();

                    ConfiguracionVacia.StatusCode = StatusCodes.Status400BadRequest;
                    ConfiguracionVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    ConfiguracionVacia.IsSuccess = false;

                    return ConfiguracionVacia;
                }
                else
                {

                    var ConfiguracionToUpdate = await _context.Configuraciones.FirstAsync();

                    if (ConfiguracionToUpdate == null)
                    {
                        var ConfiguracionVacia = new ConfiguracionDto();

                        ConfiguracionVacia.StatusCode = StatusCodes.Status404NotFound;
                        ConfiguracionVacia.ErrorMessage = "La configuracion no existe";
                        ConfiguracionVacia.IsSuccess = false;

                        return ConfiguracionVacia;
                    }
                    else
                    {
                         ConfiguracionToUpdate.Direccion = request.Direccion;
                         ConfiguracionToUpdate.UrlDireccion = request.UrlDireccion;
                         ConfiguracionToUpdate.Horarios = request.Horarios;
                         ConfiguracionToUpdate.Cbu = request.Cbu;
                         ConfiguracionToUpdate.Alias = request.Alias;
                         ConfiguracionToUpdate.Whatsapp = request.Whatsapp;
                         ConfiguracionToUpdate.Telefono = request.Telefono;
                         ConfiguracionToUpdate.Facebook = request.Facebook;
                         ConfiguracionToUpdate.UrlFacebook = request.UrlFacebook;
                         ConfiguracionToUpdate.Instagram = request.Instagram;
                         ConfiguracionToUpdate.UrlInstagram = request.UrlInstagram;
                         ConfiguracionToUpdate.MontoMayorista = request.MontoMayorista;
                         ConfiguracionToUpdate.UrlLogo = request.UrlLogo;

                        await _context.SaveChangesAsync();

                        var configuracionDto = _mapper.Map<ConfiguracionDto>(ConfiguracionToUpdate);

                        configuracionDto.StatusCode = StatusCodes.Status200OK;
                        configuracionDto.IsSuccess = true;
                        configuracionDto.ErrorMessage = "";

                        return configuracionDto;
                    }

                }
            }
            catch (Exception ex)
            {
                var ConfiguracionVacia = new ConfiguracionDto();

                ConfiguracionVacia.StatusCode = StatusCodes.Status400BadRequest;
                ConfiguracionVacia.ErrorMessage = ex.Message;
                ConfiguracionVacia.IsSuccess = false;

                return ConfiguracionVacia;
            }
        }
    }
}
