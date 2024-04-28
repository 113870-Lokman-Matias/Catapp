using API.Dtos.StockDtos;
using API.Services.StockServices.Queries.GetDetallesStockByIdQuery;
using API.Services.StockServices.Commands.CreateDetalleStockCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("stock")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<GeneralHub> _hubContext;

        public StockController(IMediator mediator, IHubContext<GeneralHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
        public Task<ListaStocksDto> GetDetallesStockById(int id)
        {
            var detallesStockByIdentificador = _mediator.Send(new GetDetallesStockByIdQuery(id));
            return detallesStockByIdentificador;
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
        public async Task<StockDto> CreateDetalleStock(CreateDetalleStockCommand command)
        {
            var detalleStockCreado = await _mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("MensajeCreateDetalleStock", "Se ha creado un nuevo detalle de stock");

            return detalleStockCreado;
        }
    }
}