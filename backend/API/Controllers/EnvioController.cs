using API.Dtos.EnvioDto;
using API.Services.CategoriaServices.Commands.UpdateCategoriaCommand;
using API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand;
using API.Services.EnvioServices.Queries.GetCostoEnvioQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("envio")]
    public class EnvioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EnvioController(IMediator mediator)
        {
            _mediator = mediator;
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
            return costoEnvioActualizado;
        }
    }
}
