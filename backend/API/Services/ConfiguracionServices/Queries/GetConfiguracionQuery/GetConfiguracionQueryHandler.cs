using API.Data;
using API.Dtos.ConfiguracionDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ConfiguracionServices.Queries.GetConfiguracionQuery
{
    public class GetConfiguracionQueryHandler : IRequestHandler<GetConfiguracionQuery, ConfiguracionDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        public GetConfiguracionQueryHandler(CatalogoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ConfiguracionDto> Handle(GetConfiguracionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var configuracion = await _context.Configuraciones
                .FirstAsync();

                if (configuracion != null)
                {
                    var configuracionDto = _mapper.Map<ConfiguracionDto>(configuracion);

                    configuracionDto.StatusCode = StatusCodes.Status200OK;
                    configuracionDto.ErrorMessage = string.Empty;
                    configuracionDto.IsSuccess = true;

                    return configuracionDto;
                }
                else
                {
                    var configuracionVacia = new ConfiguracionDto();

                    configuracionVacia.StatusCode = StatusCodes.Status404NotFound;
                    configuracionVacia.ErrorMessage = "No se ha encontrado la configuracion";
                    configuracionVacia.IsSuccess = false;

                    return configuracionVacia;
                }
            }
            catch (Exception ex)
            {
                var configuracionVacia = new ConfiguracionDto();

                configuracionVacia.StatusCode = StatusCodes.Status400BadRequest;
                configuracionVacia.ErrorMessage = ex.Message;
                configuracionVacia.IsSuccess = false;

                return configuracionVacia;
            }

        }
    }
}
