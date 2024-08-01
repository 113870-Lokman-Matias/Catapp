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
using MercadoPago.Client.Common;

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
                    var pagoVacio = new PagoDto();

                    pagoVacio.StatusCode = StatusCodes.Status400BadRequest;
                    pagoVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    pagoVacio.IsSuccess = false;

                    return pagoVacio;
                }
                else
                {
                    var productos = request.Productos;
                    var cliente = request.Cliente;
                    var costoEnvio = request.CostoEnvio;

                    var items = productos.Select(p => new PreferenceItemRequest
                    {
                        Id = p.IdProducto,
                        Title = p.Nombre,
                        Quantity = p.Cantidad,
                        CurrencyId = "ARS",
                        UnitPrice = p.Precio,
                        Description = p.Aclaracion
                    }).ToList();

                    var preferenceRequest = new PreferenceRequest
                    {
                        Items = items,
                        Payer = new PreferencePayerRequest
                        {
                            // aqui se pasara el nombre y apellido del cliente
                            Name = request.Cliente.NombreCompleto,
                            // aqui se pasara el tipo de cliente (minorista/mayorista)
                            Surname = request.Cliente.TipoCliente.ToString(),
                            Address = new AddressRequest
                            {
                                // aqui se pasara la dirección del cliente
                                StreetName = request.Cliente.Direccion,
                                // aqui se pasara entre que calles esta la dirección del cliente
                                StreetNumber = request.Cliente.EntreCalles,
                                // aqui se pasara el id del vendedor
                                ZipCode = request.Cliente.IdVendedor
                            },
                            Phone = new PhoneRequest
                            {
                                // aqui se pasara el número de documento del cliente
                                AreaCode = request.Cliente.Dni,
                                Number = request.Cliente.Telefono // Asigna el número de teléfono del cliente como string
                            }
                        },

                        Shipments = new PreferenceShipmentsRequest
                        {
                            Cost = costoEnvio
                        },
                        BackUrls = new PreferenceBackUrlsRequest
                        {
                            Success = request.Url,
                            Failure = request.Url,
                            Pending = request.Url
                        },
                        BinaryMode = true,
                        AutoReturn = "approved",
                        NotificationUrl = "https://31c4-190-30-76-133.ngrok-free.app/pago/webhook",
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
                            }
                        }
                    };

                    // Create RequestOptions with custom headers or different access token
                    var requestOptions = new RequestOptions();
                    requestOptions.CustomHeaders.Add(Headers.IDEMPOTENCY_KEY, Guid.NewGuid().ToString());

                    // Create the preference using the MercadoPago client
                    var client = new PreferenceClient();
                    Preference preference = await client.CreateAsync(preferenceRequest, requestOptions);

                    var pagoDto = new PagoDto(); // Declarar e inicializar pagoDto
                    pagoDto.StatusCode = StatusCodes.Status200OK;
                    pagoDto.IsSuccess = true;
                    pagoDto.ErrorMessage = string.Empty;

                    // Asigna el ID de la preferencia al resultado
                    pagoDto.PreferenceId = preference.Id;

                    return pagoDto;
                }
            }
            catch (FluentValidation.ValidationException ex)
            {
                var pagoVacio = new PagoDto();

                pagoVacio.StatusCode = StatusCodes.Status400BadRequest;
                pagoVacio.ErrorMessage = ex.Message;
                pagoVacio.IsSuccess = false;

                return pagoVacio;
            }
            catch (Exception ex)
            {
                var pagoVacio = new PagoDto();

                pagoVacio.StatusCode = StatusCodes.Status400BadRequest;
                pagoVacio.ErrorMessage = ex.Message;
                pagoVacio.IsSuccess = false;

                return pagoVacio;
            }
        }

    }
}
