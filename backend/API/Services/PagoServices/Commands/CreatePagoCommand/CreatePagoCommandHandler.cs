using API.Data;
using API.Dtos.PagoDto;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

using MercadoPago.Client;
using MercadoPago.Http;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace API.Services.PagoServices.Commands.CreatePagoCommand
{
  public class CreatePagoCommandHandler : IRequestHandler<CreatePagoCommand, PagoDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePagoCommand> _validator;

    public CreatePagoCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreatePagoCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<PagoDto> Handle(CreatePagoCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var PagoVacio = new PagoDto();

          PagoVacio.StatusCode = StatusCodes.Status400BadRequest;
          PagoVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          PagoVacio.IsSuccess = false;

          return PagoVacio;
        }
        else
        {
          var pagoToCreate = _mapper.Map<Pago>(request);

          var pagoDto = _mapper.Map<PagoDto>(pagoToCreate);

          // Crea la preferencia usando el cliente de MercadoPago
          var preferenceRequest = new PreferenceRequest
          {
            Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = pagoDto.Title,
                        Quantity = pagoDto.Quantity,
                        CurrencyId = "ARS",
                        UnitPrice = pagoDto.UnitPrice,
                    },
                },

            BackUrls = new PreferenceBackUrlsRequest
            {
              Success = request.Url,
              Failure = request.Url,
              Pending = request.Url,
            },
            BinaryMode = true,
            AutoReturn = "approved",
            PaymentMethods = new PreferencePaymentMethodsRequest
            {
              ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>
              {
                  new PreferencePaymentMethodRequest
                  {
                      Id = "rapipago",
                  },
                  new PreferencePaymentMethodRequest
                  {
                      Id = "pagofacil",
                  }
              },
            }
          };

          // Create RequestOptions with custom headers or different access token
          var requestOptions = new RequestOptions();
          requestOptions.CustomHeaders.Add(Headers.IDEMPOTENCY_KEY, Guid.NewGuid().ToString());

           // Crea la preferencia usando el cliente de MercadoPago
          var client = new PreferenceClient();
          Preference preference = await client.CreateAsync(preferenceRequest, requestOptions);

          pagoDto.StatusCode = StatusCodes.Status200OK;
          pagoDto.IsSuccess = true;
          pagoDto.ErrorMessage = string.Empty;

          // Asigna el ID de la preferencia al resultado
          pagoDto.PreferenceId = preference.Id;

          return pagoDto;
        }
      }
      catch (Exception ex)
      {
        var PagoVacio = new PagoDto();

        PagoVacio.StatusCode = StatusCodes.Status400BadRequest;
        PagoVacio.ErrorMessage = ex.Message;
        PagoVacio.IsSuccess = false;

        return PagoVacio;
      }
    }

  }
}
