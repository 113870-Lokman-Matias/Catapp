using API.Data;
using API.Dtos.MetodoPagoDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.MetodoPagoServices.Queries.GetMetodosPagoQuery
{
  public class GetMetodosPagoQueryHandler : IRequestHandler<GetMetodosPagoQuery, ListaMetodosPagoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    public GetMetodosPagoQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaMetodosPagoDto> Handle(GetMetodosPagoQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var metodosPago = await _context.MetodosPagos
            .Select(x => new ListaMetodoPagoDto { 
                IdMetodoPago = x.IdMetodoPago,
                Nombre = x.Nombre,
                Habilitado = x.Habilitado,
                Disponibilidad = x.Disponibilidad
            })
            .OrderBy(x => x.Nombre)
            .ToListAsync();

        if (metodosPago.Count > 0)
        {
          var listaMetodosPagoDto = new ListaMetodosPagoDto();
          listaMetodosPagoDto.MetodosPago = metodosPago;

          listaMetodosPagoDto.StatusCode = StatusCodes.Status200OK;
          listaMetodosPagoDto.ErrorMessage = string.Empty;
          listaMetodosPagoDto.IsSuccess = true;

          return listaMetodosPagoDto;
        }
        else
        {
          var listaMetodosPagoVacia = new ListaMetodosPagoDto();

          listaMetodosPagoVacia.StatusCode = StatusCodes.Status404NotFound;
          listaMetodosPagoVacia.ErrorMessage = "No se han encontrado metodos de pago";
          listaMetodosPagoVacia.IsSuccess = false;

          return listaMetodosPagoVacia;
        }
      }
      catch (Exception ex)
      {
        var listaMetodosPagoVacia = new ListaMetodosPagoDto();

        listaMetodosPagoVacia.StatusCode = StatusCodes.Status400BadRequest;
        listaMetodosPagoVacia.ErrorMessage = ex.Message;
        listaMetodosPagoVacia.IsSuccess = false;

        return listaMetodosPagoVacia;
      }
    }
  }
}
