using API.Dtos.CotizacionDto;
using API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand;
using API.Services.CotizacionServices.Queries.GetCotizacionDolarQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("cotizacion")]
    public class CotizacionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CotizacionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<CotizacionDto> GetCotizacionDolar()
        {
            var cotizacionDolar = _mediator.Send(new GetCotizacionDolarQuery());
            return cotizacionDolar;
        }


        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<CotizacionDto> UpdateCotizacionDolar(UpdateCotizacionDolarCommand command)
        {
            var cotizacionDolarActualizada = await _mediator.Send(command);
            return cotizacionDolarActualizada;
        }
    }
}
