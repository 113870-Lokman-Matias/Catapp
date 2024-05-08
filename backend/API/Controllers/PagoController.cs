using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.PagoDto;
using API.Services.PagoServices.Commands.CreatePagoCommand;

namespace API.Controllers;

[ApiController]
[Route("pago")]
public class PagoController : ControllerBase
{
  private readonly IMediator _mediator;

  public PagoController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<PagoDto> Pagar(CreatePagoCommand command)
  {
    var pagoCreado = await _mediator.Send(command);
    return pagoCreado;
  }
}
