using API.Data;
using API.Dtos.UsuarioDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace API.Services.UsuarioServices.Commands.SearchUsuarioCommand
{
    public class SearchUsuarioCommandHandler : IRequestHandler<SearchUsuarioCommand, UsuarioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<SearchUsuarioCommand> _validator;
        private readonly SmtpSettings _smtpSettings;


        public SearchUsuarioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<SearchUsuarioCommand> validator, IOptions<SmtpSettings> smtpSettings)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<UsuarioDto> Handle(SearchUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var UsuarioVacio = new UsuarioDto();

                    UsuarioVacio.StatusCode = StatusCodes.Status400BadRequest;
                    UsuarioVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    UsuarioVacio.IsSuccess = false;

                    return UsuarioVacio;
                }
                else
                {
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => (x.Username == request.Username || x.Email == request.Email));

                    if (usuario == null)
                    {
                        var UsuarioVacio = new UsuarioDto();

                        UsuarioVacio.StatusCode = StatusCodes.Status404NotFound;
                        UsuarioVacio.ErrorMessage = "Usuario/Email no registrado";
                        UsuarioVacio.IsSuccess = false;

                        return UsuarioVacio;
                    }
                    else
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(100000, 999999); // Genera un número aleatorio de 6 dígitos

                        using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                        {
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                            client.EnableSsl = _smtpSettings.EnableSsl;

                            var message = new MailMessage
                            {
                                From = new MailAddress(_smtpSettings.Username), // Especifica el remitente del correo electrónico
                                Subject = "Código de Verificación",
                                Body = $"Estimado/a {usuario.Nombre},\n\nSu código de verificación es: {randomNumber}\n\nUtilice este código para completar su proceso de verificación. Recuerde que este código es confidencial y no debe ser compartido con otras personas.\n\n¡Gracias por utilizar nuestro servicio!\n\nAtentamente,\nCatapp"
                            };

                            message.To.Add(new MailAddress(usuario.Email)); // Agrega el destinatario del correo electrónico

                            await client.SendMailAsync(message);

                            usuario.CodigoVerificacion = randomNumber;
                            await _context.SaveChangesAsync();

                            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                            usuarioDto.StatusCode = StatusCodes.Status200OK;
                            usuarioDto.IsSuccess = true;
                            usuarioDto.ErrorMessage = string.Empty;

                            return usuarioDto;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var UsuarioVacio = new UsuarioDto();

                UsuarioVacio.StatusCode = StatusCodes.Status400BadRequest;
                UsuarioVacio.ErrorMessage = ex.Message;
                UsuarioVacio.IsSuccess = false;

                return UsuarioVacio;
            }
        }
    }
}