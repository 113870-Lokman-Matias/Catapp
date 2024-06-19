using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.UsuarioDtos;
using API.Services.UsuarioServices.Commands.LoginUsuarioCommand;
using API.Services.UsuarioServices.Queries.GetUsuariosVendedoresQuery;
using API.Services.UsuarioServices.Queries.GetUsuariosQuery;
using API.Services.UsuarioServices.Queries.GetUsuariosByRolManageQuery;
using API.Services.UsuarioServices.Commands.CreateUsuarioCommand;
using API.Services.UsuarioServices.Commands.CreateUsuarioNoLogueadoCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioCommand;
using API.Services.UsuarioServices.Commands.UpdateActivoUsuarioCommand;
using API.Services.UsuarioServices.Commands.DeleteUsuarioCommand;
using API.Services.UsuarioServices.Commands.SearchUsuarioCommand;
using API.Services.UsuarioServices.Commands.VerifyUsuarioCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioNoLogueadoPasswordCommand;
using API.Services.UsuarioServices.Commands.UpdateUsuarioPasswordCommand;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("usuario")]

public class UsuarioController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IHubContext<GeneralHub> _hubContext;

  public UsuarioController(IMediator mediator, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }

  [HttpPost]
  [Route("login")]
  public async Task<TokenDto> LoginUsuario(LoginUsuarioCommand command)
  {
    var token = await _mediator.Send(command);
    return token;
  }


  [HttpGet("manage/{role}")]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor, Vendedor")]
  public Task<ListaUsuariosDto> GetUsuariosByRoleManage(string role)
  {
    var usuariosByRoleManage = _mediator.Send(new GetUsuariosByRolManageQuery(role));
    return usuariosByRoleManage;
  }

  [HttpGet]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor")]
  public Task<ListaUsuariosDto> GetUsuarios()
  {
    var usuarios = _mediator.Send(new GetUsuariosQuery());
    return usuarios;
  }

  [HttpGet("vendedores")]
  public Task<ListaUsuariosDto> GetVendedores()
  {
    var vendedores = _mediator.Send(new GetUsuariosVendedoresQuery());
    return vendedores;
  }


  [HttpPost]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor")]
  public async Task<UsuarioDto> CreateUsuario(CreateUsuarioCommand command)
  {
    var usuarioCreado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudVendedor", "Se ha creado un nuevo vendedor");

    return usuarioCreado;
  }

  [HttpPost("nologueado")]
  public async Task<UsuarioDto> CreateUsuarioNoLogueado(CreateUsuarioNoLogueadoCommand command)
  {
    var usuarioCreado = await _mediator.Send(command);
    return usuarioCreado;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor")]
  public async Task<UsuarioDto> UpdateUsuario(int id, UpdateUsuarioCommand command)
  {
    command.IdUsuario = id;
    var usuarioActualizado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudVendedor", "Se ha creado actualizado un vendedor existente");

    return usuarioActualizado;
  }

  [HttpPut]
  public async Task<UsuarioDto> UpdatePasswordNoLogueadoUsuario(UpdateUsuarioNoLogueadoPasswordCommand command)
  {
    var usuarioActualizado = await _mediator.Send(command);
    return usuarioActualizado;
  }

  [HttpPut]
  [Route("reset")]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor, Vendedor")]
  public async Task<UsuarioDto> UpdatePasswordUsuario(UpdateUsuarioPasswordCommand command)
  {
    var usuarioActualizado = await _mediator.Send(command);
    return usuarioActualizado;
  }

  [HttpPatch("{id}")]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor")]
  public async Task<UsuarioDto> UpdateActivoUsuario(int id, UpdateActivoUsuarioCommand command)
  {
    command.IdUsuario = id;
    var usuarioActivoActualizado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudVendedor", "Se ha Se ha creado actualizado el estado de un vendedor existente");

    return usuarioActivoActualizado;
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Admin, Gerente, Supervisor")]
  public async Task<UsuarioDto> DeleteUsuario(int id)
  {
    var usuarioEliminado = await _mediator.Send(new DeleteUsuarioCommand { IdUsuario = id });

    await _hubContext.Clients.All.SendAsync("MensajeCrudVendedor", "Se ha Se ha eliminado actualizado un vendedor existente");

    return usuarioEliminado;
  }

  [HttpPost]
  [Route("search")]
  public async Task<UsuarioDto> SearchUsuario(SearchUsuarioCommand command)
  {
    var usuarioEncontrado = await _mediator.Send(command);
    return usuarioEncontrado;
  }

  [HttpPost]
  [Route("verify")]
  public async Task<UsuarioDto> VerifyUsuario(VerifyUsuarioCommand command)
  {
    var usuarioVerificado = await _mediator.Send(command);
    return usuarioVerificado;
  }

}
