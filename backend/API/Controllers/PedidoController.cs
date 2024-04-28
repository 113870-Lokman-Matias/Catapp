using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.PedidoDtos;
using API.Services.PedidoServices.Queries.GetPedidosQuery;
using API.Services.PedidoServices.Queries.GetPedidosVerificadosQuery;
using API.Services.PedidoServices.Commands.CreatePedidoCommand;
using API.Services.PedidoServices.Commands.UpdatePedidoCommand;
using API.Services.PedidoServices.Commands.DeletePedidoCommand;
using API.Services.PedidoServices.Commands.UpdateVerificadoPedidoCommand;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("pedido")]
public class PedidoController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IHubContext<GeneralHub> _hubContext;

  public PedidoController(IMediator mediator, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }


  [HttpGet]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public Task<ListaPedidosDto> GetPedidos()
  {
    var pedidos = _mediator.Send(new GetPedidosQuery());
    return pedidos;
  }


  [HttpGet("verificado")]
  [Authorize(Roles = "SuperAdmin, Gerente")]
  public Task<ListaPedidosDto> GetPedidosVerificados()
  {
    var pedidosVerificados = _mediator.Send(new GetPedidosVerificadosQuery());
    return pedidosVerificados;
  }


  [HttpPost]
  public async Task<PedidoDto> CreatePedido(CreatePedidoCommand command)
  {
    var pedidoCreado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudPedido", "Se ha creado un nuevo pedido");

    return pedidoCreado;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<PedidoDto> UpdatePedido(Guid id, UpdatePedidoCommand command)
  {
    command.IdPedido = id;
    var pedidoActualizado = await _mediator.Send(command);
    return pedidoActualizado;
  }


  [HttpPatch("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<PedidoDto> UpdateVerificadoPedido(Guid id, UpdateVerificadoPedidoCommand command)
  {
    command.IdPedido = id;
    var peridoVerificadoActualizado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudPedido", "Se ha actualizado el estado de un pedido existente");

    return peridoVerificadoActualizado;
  }


  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<PedidoDto> DeletePedido(Guid id)
  {
    var pedidoEliminado = await _mediator.Send(new DeletePedidoCommand { IdPedido = id });

    await _hubContext.Clients.All.SendAsync("MensajeCrudPedido", "Se ha eliminado un pedido existente");

    return pedidoEliminado;
  }

}
