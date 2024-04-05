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

namespace API.Services.UsuarioServices.Commands.VerifyUsuarioCommand
{
    public class VerifyUsuarioCommandHandler : IRequestHandler<VerifyUsuarioCommand, UsuarioDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<VerifyUsuarioCommand> _validator;

        public VerifyUsuarioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<VerifyUsuarioCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UsuarioDto> Handle(VerifyUsuarioCommand request, CancellationToken cancellationToken)
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
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => (x.CodigoVerificacion == request.CodigoVerificacion));

                    if (usuario == null)
                    {
                        var UsuarioVacio = new UsuarioDto();

                        UsuarioVacio.StatusCode = StatusCodes.Status404NotFound;
                        UsuarioVacio.ErrorMessage = "Usuario no verificado";
                        UsuarioVacio.IsSuccess = false;

                        return UsuarioVacio;
                    }
                    else
                    {
                        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

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