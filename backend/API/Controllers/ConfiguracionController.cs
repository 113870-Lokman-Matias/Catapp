using API.Dtos.ConfiguracionDto;
using API.Services.ConfiguracionServices.Commands.UpdateConfiguracionCommand;
using API.Services.ConfiguracionServices.Queries.GetConfiguracionQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("configuracion")]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<GeneralHub> _hubContext;

        public ConfiguracionController(IMediator mediator, IHubContext<GeneralHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpGet]
        public Task<ConfiguracionDto> GetConfiguracion()
        {
            var configuracion = _mediator.Send(new GetConfiguracionQuery());
            return configuracion;
        }


        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<ConfiguracionDto> UpdateConfiguracion(UpdateConfiguracionCommand command)
        {
            var configuracionActualizada = await _mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("MensajeUpdateConfiguracion", "Se ha actualizado la configuracion");

            return configuracionActualizada;
        }
    }
}
