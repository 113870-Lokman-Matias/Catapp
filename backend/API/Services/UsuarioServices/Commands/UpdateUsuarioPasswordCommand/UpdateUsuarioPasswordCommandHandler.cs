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

namespace API.Services.UsuarioServices.Commands.UpdateUsuarioPasswordCommand
{
    public class UpdateUsuarioPasswordCommandHandler : IRequestHandler<UpdateUsuarioPasswordCommand, UsuarioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUsuarioPasswordCommand> _validator;

        public UpdateUsuarioPasswordCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateUsuarioPasswordCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UsuarioDto> Handle(UpdateUsuarioPasswordCommand request, CancellationToken cancellationToken)
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
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username == request.Username);

                    if (usuario == null)
                    {
                        var UsuarioVacio = new UsuarioDto();

                        UsuarioVacio.StatusCode = StatusCodes.Status404NotFound;
                        UsuarioVacio.ErrorMessage = "No se encontro el usuario";
                        UsuarioVacio.IsSuccess = false;

                        return UsuarioVacio;
                    }
                    else
                    {
                        // Si la contraseña es la misma, no se actualiza
                        if (BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
                        {
                            var UsuarioVacio = new UsuarioDto();
                            UsuarioVacio.StatusCode = StatusCodes.Status400BadRequest;
                            UsuarioVacio.ErrorMessage = "La contraseña proporcionada es la misma que la actual";
                            UsuarioVacio.IsSuccess = false;

                            return UsuarioVacio;
                        }

                        // Hash de la contraseña utilizando bcrypt
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                        usuario.Password = hashedPassword; // Asigna el hash de la contraseña al usuario a crear

                        await _context.SaveChangesAsync();

                        var usuarioDto = new UsuarioDto();

                        usuarioDto.StatusCode = StatusCodes.Status200OK;
                        usuarioDto.IsSuccess = true;
                        usuarioDto.ErrorMessage = string.Empty;

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