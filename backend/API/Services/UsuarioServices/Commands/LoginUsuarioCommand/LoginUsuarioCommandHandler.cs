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

namespace API.Services.UsuarioServices.Commands.LoginUsuarioCommand
{
    public class LoginUsuarioCommandHandler : IRequestHandler<LoginUsuarioCommand, TokenDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginUsuarioCommand> _validator;
        private readonly JwtSetings jwtSettings;

        public LoginUsuarioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<LoginUsuarioCommand> validator, IOptions<JwtSetings> options)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            this.jwtSettings = options.Value;
        }

        public async Task<TokenDto> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var UsuarioVacio = new TokenDto();

                    UsuarioVacio.StatusCode = StatusCodes.Status400BadRequest;
                    UsuarioVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    UsuarioVacio.IsSuccess = false;

                    return UsuarioVacio;
                }
                else
                {
                    var usuario = await _context.Usuarios
                                            .Include(u => u.IdRolNavigation) // Cargar la propiedad IdRolNavigation
                                            .FirstOrDefaultAsync(x => (x.Username == request.Username || x.Email == request.Email) && x.Activo);

                    if (usuario == null)
                    {
                        var UsuarioVacio = new TokenDto();

                        UsuarioVacio.StatusCode = StatusCodes.Status404NotFound;
                        UsuarioVacio.ErrorMessage = "Usuario desactivado";
                        UsuarioVacio.IsSuccess = false;

                        return UsuarioVacio;
                    }
                    else
                    {
                        var tokenhandler = new JwtSecurityTokenHandler();
                        var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
                        var tokendesc = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(
                                new Claim[] { new Claim(ClaimTypes.NameIdentifier, usuario.Username), new Claim(ClaimTypes.Name, usuario.Nombre), new Claim(ClaimTypes.Role, usuario.IdRolNavigation.Nombre) }
                            ),

                            Expires = DateTime.UtcNow.AddHours(3),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                        };
                        var token = tokenhandler.CreateToken(tokendesc);
                        string finaltoken = tokenhandler.WriteToken(token);

                        var usuarioDto = new TokenDto();

                        usuarioDto.Token = finaltoken;
                        usuarioDto.StatusCode = StatusCodes.Status200OK;
                        usuarioDto.IsSuccess = true;
                        usuarioDto.ErrorMessage = string.Empty;

                        return usuarioDto;
                    }
                }
            }
            catch (Exception ex)
            {
                var UsuarioVacio = new TokenDto();

                UsuarioVacio.StatusCode = StatusCodes.Status400BadRequest;
                UsuarioVacio.ErrorMessage = ex.Message;
                UsuarioVacio.IsSuccess = false;

                return UsuarioVacio;
            }
        }
    }
}