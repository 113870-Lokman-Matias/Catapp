using API.Data;
using API.Dtos.EnvioDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EnvioServices.Queries.GetEnvioQuery
{
    public class GetEnvioQueryHandler : IRequestHandler<GetEnvioQuery, EnvioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        public GetEnvioQueryHandler(CatalogoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EnvioDto> Handle(GetEnvioQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var envio = await _context.Envios
                .FirstAsync();

                if (envio != null)
                {
                    var envioDto = _mapper.Map<EnvioDto>(envio);

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
