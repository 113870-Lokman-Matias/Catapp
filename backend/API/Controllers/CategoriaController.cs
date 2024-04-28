using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.CategoriaDtos;
using API.Services.CategoriaServices.Queries.GetCategoriasQuery;
using API.Services.CategoriaServices.Queries.GetCategoriasMinoristaQuery;
using API.Services.CategoriaServices.Queries.GetCategoriasMayoristaQuery;
using API.Services.CategoriaServices.Commands.CreateCategoriaCommand;
using API.Services.CategoriaServices.Commands.UpdateCategoriaCommand;
using API.Services.CategoriaServices.Commands.DeleteCategoriaCommand;
using API.Services.CategoriaServices.Queries.GetCategoriasManageQuery;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("categoria")]
public class CategoriaController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IHubContext<GeneralHub> _hubContext;

  public CategoriaController(IMediator mediator, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }


  [HttpGet]
  [Route("manage")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public Task<ListaCategoriasDto> GetCategoriasManage()
  {
    var categoriasManage = _mediator.Send(new GetCategoriasManageQuery());
    return categoriasManage;
  }


  [HttpGet]
  public Task<ListaCategoriasDto> GetCategorias()
  {
    var categorias = _mediator.Send(new GetCategoriasQuery());
    return categorias;
  }

  [HttpGet]
  [Route("minorista")]
  public Task<ListaCategoriasDto> GetCategoriasMinoristas()
  {
    var categoriasMinoristas = _mediator.Send(new GetCategoriasMinoristaQuery());
    return categoriasMinoristas;
  }

  [HttpGet]
  [Route("mayorista")]
  public Task<ListaCategoriasDto> GetCategoriasMayoristas()
  {
    var categoriasMayoristas = _mediator.Send(new GetCategoriasMayoristaQuery());
    return categoriasMayoristas;
  }


  [HttpPost]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<CategoriaDto> CreateCategoria(CreateCategoriaCommand command)
  {
    var categoriaCreada = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudCategoria", "Se ha creado una nueva categoria");

    return categoriaCreada;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<CategoriaDto> UpdateCategoria(int id, UpdateCategoriaCommand command)
  {
    command.IdCategoria = id;
    var categoriaActualizada = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudCategoria", "Se ha actualizado una categoria existente");

    return categoriaActualizada;
  }


  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<CategoriaDto> DeleteCategoria(int id)
  {
    var categoriaEliminada = await _mediator.Send(new DeleteCategoriaCommand { IdCategoria = id });

    await _hubContext.Clients.All.SendAsync("MensajeCrudCategoria", "Se ha eliminado una categoria existente");

    return categoriaEliminada;
  }

}
