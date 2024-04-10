using API.Data;
using API.Dtos.CotizacionDto;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand
{
    public class UpdateCotizacionDolarCommandHandler : IRequestHandler<UpdateCotizacionDolarCommand, CotizacionDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCotizacionDolarCommand> _validator;
        public UpdateCotizacionDolarCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateCotizacionDolarCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CotizacionDto> Handle(UpdateCotizacionDolarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var CotizacionDolarVacia = new CotizacionDto();

                    CotizacionDolarVacia.StatusCode = StatusCodes.Status400BadRequest;
                    CotizacionDolarVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    CotizacionDolarVacia.IsSuccess = false;

                    return CotizacionDolarVacia;
                }
                else
                {

                    var CotizacionDolarToUpdate = await _context.Cotizaciones.FirstAsync();

                    if (CotizacionDolarToUpdate == null)
                    {
                        var CotizacionDolarVacia = new CotizacionDto();

                        CotizacionDolarVacia.StatusCode = StatusCodes.Status404NotFound;
                        CotizacionDolarVacia.ErrorMessage = "La cotización del dolar no existe";
                        CotizacionDolarVacia.IsSuccess = false;

                        return CotizacionDolarVacia;
                    }
                    else
                    {
                        CotizacionDolarToUpdate.Precio = request.Precio;
                        CotizacionDolarToUpdate.UltimoModificador = request.UltimoModificador;
                        CotizacionDolarToUpdate.FechaModificacion = DateTimeOffset.Now.ToUniversalTime();

                        await _context.SaveChangesAsync();

                        var dolarDto = _mapper.Map<CotizacionDto>(CotizacionDolarToUpdate);

                        dolarDto.StatusCode = StatusCodes.Status200OK;
                        dolarDto.IsSuccess = true;
                        dolarDto.ErrorMessage = "";

                        return dolarDto;
                    }

                }
            }
            catch (Exception ex)
            {
                var CotizacionDolarVacia = new CotizacionDto();

                CotizacionDolarVacia.StatusCode = StatusCodes.Status400BadRequest;
                CotizacionDolarVacia.ErrorMessage = ex.Message;
                CotizacionDolarVacia.IsSuccess = false;

                return CotizacionDolarVacia;
            }
        }
    }
}
