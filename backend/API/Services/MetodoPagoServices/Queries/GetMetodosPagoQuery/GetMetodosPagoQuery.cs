using API.Dtos.MetodoPagoDto;
using MediatR;

namespace API.Services.MetodoPagoServices.Queries.GetMetodosPagoQuery
{
  public class GetMetodosPagoQuery : IRequest<ListaMetodosPagoDto>
  {
  }
}
