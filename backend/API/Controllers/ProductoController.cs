using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Services.ProductoServices.Queries.GetProductosManageQuery;
using API.Dtos.ProductoDtos;
using API.Services.ProductoServices.Queries.GetProductosByCategoryQuery;
using API.Services.ProductoServices.Queries.GetProductosBySubcategoryQuery;
using API.Services.ProductoServices.Queries.GetProductosByQueryQuery;
using API.Services.ProductoServices.Queries.GetProductoByIdQuery;
using API.Services.ProductoServices.Commands.CreateProductoCommand;
using API.Services.ProductoServices.Commands.UpdateProductoCommand;
using API.Services.ProductoServices.Commands.DeleteProductoCommand;
using API.Services.ProductoServices.Commands.UpdateStockProductoCommand;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("producto")]
public class ProductoController : ControllerBase
{
  private readonly IMediator _mediator;

  private readonly IHubContext<GeneralHub> _hubContext;

  public ProductoController(IMediator mediator, IHubContext<GeneralHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }


  [HttpGet]
  [Route("manage")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ListaProductosManageDto> GetProductosManage(string? Query = null, string? Category = null, bool? Hidden = null, bool? Stock = null, string? Subcategory = null)
  {
    var query = new GetProductosManageQuery(Query, Category, Hidden, Stock, Subcategory);
    var productosManage = await _mediator.Send(query);
    return productosManage;
  }


  [HttpGet("categoria/{category}/{client}")]
  public Task<ListaProductosDto> GetProductosByCategory(string category, int client)
  {
    var productosByCategory = _mediator.Send(new GetProductosByCategoryQuery(category, client));
    return productosByCategory;
  }

  [HttpGet("subcategoria/{idCategory}/{idSubcategory}/{client}")]
  public Task<ListaProductosDto> GetProductosBySubcategory(int idCategory, int idSubcategory, int client)
  {
    var productosBySubcategory = _mediator.Send(new GetProductosBySubcategoryQuery(idCategory, idSubcategory, client));
    return productosBySubcategory;
  }

  [HttpGet("query/{query}/{client}")]
  public Task<ListaProductosDto> GetProductosByQuery(string query, int client)
  {
    var productosByQuery = _mediator.Send(new GetProductosByQueryQuery(query, client));
    return productosByQuery;
  }

  [HttpGet("{id}/{client}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ProductoDto> GetProductoById(int id, int client)
  {
    var producto = await _mediator.Send(new GetProductoByIdQuery(id, client));
    return producto;
  }


  [HttpPost]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ProductoDto> CreateProducto(CreateProductoCommand command)
  {
    var productoCreado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudProducto", "Se ha creado un nuevo producto");

    return productoCreado;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ProductoDto> UpdateProducto(int id, UpdateProductoCommand command)
  {
    command.IdProducto = id;
    var productoActualizado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudProducto", "Se ha actualizado un producto existente");

    return productoActualizado;
  }


  [HttpPatch("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<ProductoDto> UpdateStockProducto(int id, UpdateStockProductoCommand command)
  {
    command.IdProducto = id;
    var productoConStockActualizado = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("MensajeCrudProducto", "Se ha actualizado el stock de un producto existente");

    return productoConStockActualizado;
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<ProductoDto> DeleteProducto(int id)
  {
    var productoEliminado = await _mediator.Send(new DeleteProductoCommand { IdProducto = id });

    await _hubContext.Clients.All.SendAsync("MensajeCrudProducto", "Se ha eliminado un producto existente");

    return productoEliminado;
  }

}
