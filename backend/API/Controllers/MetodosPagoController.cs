using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.MetodoPagoDto;
using API.Services.MetodoPagoServices.Queries.GetMetodosPagoQuery;
using API.Services.MetodoPagoServices.Commands.CreateMetodoPagoCommand;
using API.Services.MetodoPagoServices.Commands.UpdateMetodoPagoCommand;
using API.Services.MetodoPagoServices.Commands.DeleteMetodoPagoCommand;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("metodospago")]
public class MetodosPagoController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IHubContext<GeneralHub> _hubContext;

  public MetodosPagoController(IMediator mediator, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }


  [HttpGet]
  public Task<ListaMetodosPagoDto> GetMetodoPago()
  {
    var metodosPago = _mediator.Send(new GetMetodosPagoQuery());
    return metodosPago;
  }


  [HttpPost]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<MetodoPagoDto> CreateMetodoPago(CreateMetodoPagoCommand command)
  {
    var metodoPagoCreado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudMetodoPago", "Se ha creado un nuevo metodo de pago");

    return metodoPagoCreado;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<MetodoPagoDto> UpdateMetodoPago(int id, UpdateMetodoPagoCommand command)
  {
    command.IdMetodoPago = id;
    var metodoPagoActualizado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudMetodoPago", "Se ha actualizado un metodo de pago existente");

    return metodoPagoActualizado;
  }


  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<MetodoPagoDto> DeleteMetodoPago(int id)
  {
    var metodoPagoEliminado = await _mediator.Send(new DeleteMetodoPagoCommand { IdMetodoPago = id });

    await _hubContext.Clients.All.SendAsync("MensajeCrudMetodoPago", "Se ha eliminado un metodo de pago existente");

    return metodoPagoEliminado;
  }

}
