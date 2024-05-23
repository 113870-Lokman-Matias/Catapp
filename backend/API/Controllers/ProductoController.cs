using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Services.ProductoServices.Queries.GetProductosManageQuery;
using API.Dtos.ProductoDtos;
using API.Services.ProductoServices.Queries.GetProductosByCategoryQuery;
using API.Services.ProductoServices.Queries.GetProductosByQueryQuery;
using API.Services.ProductoServices.Queries.GetProductoByIdQuery;
using API.Services.ProductoServices.Queries.GetProductosQuery;
using API.Services.ProductoServices.Commands.CreateProductoCommand;
using API.Services.ProductoServices.Commands.UpdateProductoCommand;
using API.Services.ProductoServices.Commands.DeleteProductoCommand;
using API.Services.ProductoServices.Commands.UpdateStockProductoCommand;

namespace API.Controllers;

[ApiController]
[Route("producto")]
public class ProductoController : ControllerBase
{
  private readonly IMediator _mediator;

  public ProductoController(IMediator mediator)
  {
    _mediator = mediator;
  }


  [HttpGet]
  [Route("manage")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public Task<ListaProductosManageDto> GetProductosManage()
  {
    var productosManage = _mediator.Send(new GetProductosManageQuery());
    return productosManage;
  }

  [HttpGet("categoria/{category}")]
  public Task<ListaProductosDto> GetProductosByCategory(string category)
  {
    var productosByCategory = _mediator.Send(new GetProductosByCategoryQuery(category));
    return productosByCategory;
  }

  [HttpGet("query/{query}")]
  public Task<ListaProductosDto> GetProductosByQuery(string query)
  {
    var productosByQuery = _mediator.Send(new GetProductosByQueryQuery(query));
    return productosByQuery;
  }

  [HttpGet]
  public Task<ListaProductosDto> GetProductos()
  {
    var productos = _mediator.Send(new GetProductosQuery());
    return productos;
  }

  [HttpGet("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ProductoDto> GetProductoById(int id)
  {
    var producto = await _mediator.Send(new GetProductoByIdQuery(id));
    return producto;
  }


  [HttpPost]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ProductoDto> CreateProducto(CreateProductoCommand command)
  {
    var productoCreado = await _mediator.Send(command);
    return productoCreado;
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor, Vendedor")]
  public async Task<ProductoDto> UpdateProducto(int id, UpdateProductoCommand command)
  {
    command.IdProducto = id;
    var productoActualizado = await _mediator.Send(command);
    return productoActualizado;
  }


  [HttpPatch("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<ProductoDto> UpdateStockProducto(int id, UpdateStockProductoCommand command)
  {
    command.IdProducto = id;
    var productoConStockActualizado = await _mediator.Send(command);
    return productoConStockActualizado;
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "SuperAdmin, Supervisor")]
  public async Task<ProductoDto> DeleteProducto(int id)
  {
    var productoEliminado = await _mediator.Send(new DeleteProductoCommand { IdProducto = id });
    return productoEliminado;
  }

}
