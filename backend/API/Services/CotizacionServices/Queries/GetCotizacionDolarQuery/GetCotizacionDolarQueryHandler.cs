using API.Data;
using API.Dtos.CotizacionDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CotizacionServices.Queries.GetCotizacionDolarQuery
{
    public class GetCotizacionDolarQueryHandler : IRequestHandler<GetCotizacionDolarQuery, CotizacionDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        public GetCotizacionDolarQueryHandler(CatalogoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CotizacionDto> Handle(GetCotizacionDolarQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cotizacionDolar = await _context.Cotizaciones
                .FirstAsync();

                if (cotizacionDolar != null)
                {
                    var dolarDto = _mapper.Map<CotizacionDto>(cotizacionDolar);

                    dolarDto.StatusCode = StatusCodes.Status200OK;
                    dolarDto.ErrorMessage = string.Empty;
                    dolarDto.IsSuccess = true;

                    return dolarDto;
                }
                else
                {
                    var cotizacionVacia = new CotizacionDto();

                    cotizacionVacia.StatusCode = StatusCodes.Status404NotFound;
                    cotizacionVacia.ErrorMessage = "No se ha encontrado la cotización del dolar";
                    cotizacionVacia.IsSuccess = false;

                    return cotizacionVacia;
                }
            }
            catch (Exception ex)
            {
                var cotizacionVacia = new CotizacionDto();

                cotizacionVacia.StatusCode = StatusCodes.Status400BadRequest;
                cotizacionVacia.ErrorMessage = ex.Message;
                cotizacionVacia.IsSuccess = false;

                return cotizacionVacia;
            }

        }
    }
}
