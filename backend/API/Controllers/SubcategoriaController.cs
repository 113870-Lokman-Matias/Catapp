using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.SubcategoriaDtos;
using API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryQuery;
using API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryManageQuery;
using API.Services.SubcategoriaServices.Commands.CreateSubcategoriaCommand;
using API.Services.SubcategoriaServices.Commands.UpdateSubcategoriaCommand;
using API.Services.SubcategoriaServices.Commands.DeleteSubcategoriaCommand;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("subcategoria")]
public class SubcategoriaController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IHubContext<GeneralHub> _hubContext;

  public SubcategoriaController(IMediator mediator, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }


  [HttpGet("manage/categoria/{idCategory}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public Task<ListaSubcategoriasDto> GetSubcategoriasByCategoryManage(int idCategory)
  {
    var subcategoriasByCategoryManage = _mediator.Send(new GetSubcategoriasByCategoryManageQuery(idCategory));
    return subcategoriasByCategoryManage;
  }


  [HttpGet("categoria/{idCategory}")]
  public Task<ListaSubcategoriasDto> GetSubcategoriasByCategory(int idCategory)
  {
    var subcategoriasByCategory = _mediator.Send(new GetSubcategoriasByCategoryQuery(idCategory));
    return subcategoriasByCategory;
  }


  [HttpPost]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<SubcategoriaDto> CreateSubcategoria(CreateSubcategoriaCommand command)
  {
    var subcategoriaCreada = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudSubcategoria", "Se ha creado una nueva subcategoria");

    return subcategoriaCreada;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<SubcategoriaDto> UpdateSubcategoria(int id, UpdateSubcategoriaCommand command)
  {
    command.IdSubcategoria = id;
    var subcategoriaActualizada = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudSubcategoria", "Se ha actualizado una subcategoria existente");

    return subcategoriaActualizada;
  }


  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<SubcategoriaDto> DeleteSubcategoria(int id)
  {
    var subcategoriaEliminada = await _mediator.Send(new DeleteSubcategoriaCommand { IdSubcategoria = id });

    await _hubContext.Clients.All.SendAsync("MensajeCrudSubcategoria", "Se ha eliminado una subcategoria existente");

    return subcategoriaEliminada;
  }

}
