using API.Dtos.EnvioDto;
using API.Services.CategoriaServices.Commands.UpdateCategoriaCommand;
using API.Services.EnvioServices.Commands.UpdateEnvioCommand;
using API.Services.EnvioServices.Queries.GetEnvioQuery;
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
        public Task<EnvioDto> GetEnvio()
        {
            var envio = _mediator.Send(new GetEnvioQuery());
            return envio;
        }


        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<EnvioDto> UpdateEnvio(UpdateEnvioCommand command)
        {
            var envioActualizado = await _mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("MensajeUpdateEnvio", "Se ha actualizado el envio");

            return envioActualizado;
        }
    }
}
