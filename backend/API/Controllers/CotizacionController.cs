using API.Dtos.CotizacionDto;
using API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand;
using API.Services.CotizacionServices.Queries.GetCotizacionDolarQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("cotizacion")]
    public class CotizacionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<GeneralHub> _hubContext;

        public CotizacionController(IMediator mediator, IHubContext<GeneralHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
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

            await _hubContext.Clients.All.SendAsync("MensajeUpdateCotizacion", "Se ha actualizado la cotización dolar");

            return cotizacionDolarActualizada;
        }
    }
}
