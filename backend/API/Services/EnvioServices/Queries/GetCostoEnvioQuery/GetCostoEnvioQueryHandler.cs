using API.Data;
using API.Dtos.EnvioDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EnvioServices.Queries.GetCostoEnvioQuery
{
    public class GetCostoEnvioQueryHandler : IRequestHandler<GetCostoEnvioQuery, EnvioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        public GetCostoEnvioQueryHandler(CatalogoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EnvioDto> Handle(GetCostoEnvioQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var costoEnvio = await _context.Envios
                .FirstAsync();

                if (costoEnvio != null)
                {
                    var envioDto = _mapper.Map<EnvioDto>(costoEnvio);

                    envioDto.StatusCode = StatusCodes.Status200OK;
                    envioDto.ErrorMessage = string.Empty;
                    envioDto.IsSuccess = true;

                    return envioDto;
                }
                else
                {
                    var envioVacio = new EnvioDto();

                    envioVacio.StatusCode = StatusCodes.Status404NotFound;
                    envioVacio.ErrorMessage = "No se ha encontrado el costo de envio";
                    envioVacio.IsSuccess = false;

                    return envioVacio;
                }
            }
            catch (Exception ex)
            {
                var envioVacio = new EnvioDto();

                envioVacio.StatusCode = StatusCodes.Status400BadRequest;
                envioVacio.ErrorMessage = ex.Message;
                envioVacio.IsSuccess = false;

                return envioVacio;
            }

        }
    }
}
