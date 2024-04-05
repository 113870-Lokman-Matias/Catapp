using API.Data;
using API.Dtos.UsuarioDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.UsuarioServices.Commands.CreateUsuarioCommand
{
  public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, UsuarioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateUsuarioCommand> _validator;

    public CreateUsuarioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreateUsuarioCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<UsuarioDto> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
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
          // Hash de la contraseña utilizando bcrypt
          string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

          var usuarioToCreate = _mapper.Map<Usuario>(request);
          usuarioToCreate.Password = hashedPassword; // Asigna el hash de la contraseña al usuario a crear
          usuarioToCreate.CodigoVerificacion = 0;

          await _context.AddAsync(usuarioToCreate);
          await _context.SaveChangesAsync();

          var usuarioDto = _mapper.Map<UsuarioDto>(usuarioToCreate);

          usuarioDto.StatusCode = StatusCodes.Status200OK;
          usuarioDto.IsSuccess = true;
          usuarioDto.ErrorMessage = string.Empty;

          return usuarioDto;
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
