using API.Data;
using API.Dtos.UsuarioDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioCommand
{
  public class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, UsuarioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateUsuarioCommand> _validator;
    private readonly SmtpSettings _smtpSettings;

    public UpdateUsuarioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateUsuarioCommand> validator, IOptions<SmtpSettings> smtpSettings)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
      _smtpSettings = smtpSettings.Value;
    }

    public async Task<UsuarioDto> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
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
          var UsuarioToUpdate = await _context.Usuarios.FindAsync(request.IdUsuario);

          if (UsuarioToUpdate == null)
          {
            var UsuarioVacio = new UsuarioDto();

            UsuarioVacio.StatusCode = StatusCodes.Status404NotFound;
            UsuarioVacio.ErrorMessage = "El usuario no existe";
            UsuarioVacio.IsSuccess = false;

            return UsuarioVacio;
          }
          else
          {
            UsuarioToUpdate.IdRol = request.IdRol;
            UsuarioToUpdate.Nombre = request.Nombre;
            UsuarioToUpdate.Username = request.Username;
            UsuarioToUpdate.Activo = request.Activo;
            UsuarioToUpdate.Email = request.Email;

            await _context.SaveChangesAsync();

            if (request.Activo == true) // Verifica si cambió de false a true
            {
              using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
              {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = _smtpSettings.EnableSsl;

                var message = new MailMessage
                {
                  From = new MailAddress(_smtpSettings.Username), // Especifica el remitente del correo electrónico
                  Subject = "Activación de cuenta exitosa",
                  Body = $"Estimado/a {UsuarioToUpdate.Nombre},\n\nLe informamos que su cuenta ha sido activada con éxito. Ahora puede comenzar a utilizar nuestros servicios ingresando en el siguiente enlace: http://localhost:3000/login\n\n¡Gracias por unirse a nosotros!\n\nAtentamente,\nCatapp"
                };

                message.To.Add(new MailAddress(UsuarioToUpdate.Email)); // Agrega el destinatario del correo electrónico

                await client.SendMailAsync(message);
              }
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(UsuarioToUpdate);

            usuarioDto.StatusCode = StatusCodes.Status200OK;
            usuarioDto.IsSuccess = true;
            usuarioDto.ErrorMessage = "";

            return usuarioDto;
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
