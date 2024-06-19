using API.Dtos.EnvioDto;
using API.Services.EnvioServices.Queries.GetEnviosQuery;
using API.Services.EnvioServices.Queries.GetEnviosManageQuery;
using API.Services.EnvioServices.Commands.CreateEnvioCommand;
using API.Services.EnvioServices.Commands.UpdateEnvioCommand;
using API.Services.EnvioServices.Commands.DeleteEnvioCommand;

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
        [Route("manage")]
        [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
        public Task<ListaEnviosDto> GetFormasEntregaManage()
        {
            var enviosManage = _mediator.Send(new GetEnviosManageQuery());
            return enviosManage;
        }

        [HttpGet]
        public Task<ListaEnviosDto> GetFormasEntrega()
        {
            var envios = _mediator.Send(new GetEnviosQuery());
            return envios;
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<EnvioDto> CreateFormaEntrega(CreateEnvioCommand command)
        {
            var formaEntregaCreada = await _mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("MensajeCrudEntrega", "Se ha creado una nueva forma de entrega");

            return formaEntregaCreada;
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<EnvioDto> UpdateFormaEntrega(int id, UpdateEnvioCommand command)
        {
            command.IdEnvio = id;
            var formaEntregaActualizada = await _mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("MensajeCrudEntrega", "Se ha actualizado una forma de entrega existente");

            return formaEntregaActualizada;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, Supervisor")]
        public async Task<EnvioDto> DeleteFormaEntrega(int id)
        {
            var formaEntregaEliminada = await _mediator.Send(new DeleteEnvioCommand { IdEnvio = id });

            await _hubContext.Clients.All.SendAsync("MensajeCrudEntrega", "Se ha eliminado una forma de entrega existente");

            return formaEntregaEliminada;
        }
    }
}
