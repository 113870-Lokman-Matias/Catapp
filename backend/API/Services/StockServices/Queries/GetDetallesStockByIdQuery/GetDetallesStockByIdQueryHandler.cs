using API.Data;
using API.Dtos.StockDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.StockServices.Queries.GetDetallesStockByIdQuery
{
  public class GetDetallesStockByIdQueryHandler : IRequestHandler<GetDetallesStockByIdQuery, ListaStocksDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetDetallesStockByIdQuery> _validator;

    public GetDetallesStockByIdQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetDetallesStockByIdQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaStocksDto> Handle(GetDetallesStockByIdQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaDetallesStockVacia = new ListaStocksDto();

          ListaDetallesStockVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaDetallesStockVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaDetallesStockVacia.IsSuccess = false;

          return ListaDetallesStockVacia;
        }
        else
        {
          var productoId = request.id;

          var detalles = await _context.DetallesStocks
              .Where(x => x.IdProducto == productoId)
              .Select(x => new ListaStockDto
              {
                IdDetallesStock = x.IdDetallesStock,
                Fecha = x.Fecha,
                Accion = x.Accion,
                Cantidad = x.Cantidad,
                Motivo = x.Motivo,
                Modificador = x.Modificador,
                NombreProducto = x.IdProductoNavigation.Nombre,
              })
              .OrderByDescending(x => x.Fecha)
              .ToListAsync();

          if (detalles == null)
          {
            var ListaDetallesStockVacia = new ListaStocksDto();

            ListaDetallesStockVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaDetallesStockVacia.ErrorMessage = "No hay detalles de stock de ese producto";
            ListaDetallesStockVacia.IsSuccess = false;

            return ListaDetallesStockVacia;
          }
          else
          {
            var listaDetallesStockDto = new ListaStocksDto();
            listaDetallesStockDto.Stocks = detalles;

            listaDetallesStockDto.StatusCode = StatusCodes.Status200OK;
            listaDetallesStockDto.IsSuccess = true;
            listaDetallesStockDto.ErrorMessage = "";

            return listaDetallesStockDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaDetallesStockVacia = new ListaStocksDto();

        ListaDetallesStockVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaDetallesStockVacia.ErrorMessage = ex.Message;
        ListaDetallesStockVacia.IsSuccess = false;

        return ListaDetallesStockVacia;
      }
    }

  }
}
