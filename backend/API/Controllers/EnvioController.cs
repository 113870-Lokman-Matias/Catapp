using API.Dtos.EnvioDto;
using API.Services.CategoriaServices.Commands.UpdateCategoriaCommand;
using API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand;
using API.Services.EnvioServices.Queries.GetCostoEnvioQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("envio")]
    public class EnvioController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<GeneralHub> _hubContext;

        public EnvioController(IMediator mediator, IHubContext<GeneralHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpGet]
        public Task<EnvioDto> GetCostoEnvio()
        {
            var costoEnvio = _mediator.Send(new GetCostoEnvioQuery());
            return costoEnvio;
        }


        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<EnvioDto> UpdateCostoEnvio(UpdateCostoEnvioCommand command)
        {
            var costoEnvioActualizado = await _mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("MensajeUpdateCostoEnvio", "Se ha actualizado el costo de envio");

            return costoEnvioActualizado;
        }
    }
}
