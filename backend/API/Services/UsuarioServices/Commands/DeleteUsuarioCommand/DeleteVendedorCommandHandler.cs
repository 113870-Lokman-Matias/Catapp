using API.Data;
using API.Dtos.UsuarioDtos;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.UsuarioServices.Commands.DeleteUsuarioCommand
{
  public class DeleteUsuarioCommandHandler : IRequestHandler<DeleteUsuarioCommand, UsuarioDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteUsuarioCommand> _validator;

    public DeleteUsuarioCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeleteUsuarioCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<UsuarioDto> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
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
          var UsuarioToDelete = await _context.Usuarios.FindAsync(request.IdUsuario);

          if (UsuarioToDelete == null)
          {
            var UsuarioVacio = new UsuarioDto();

            UsuarioVacio.StatusCode = StatusCodes.Status404NotFound;
            UsuarioVacio.ErrorMessage = "El usuario no existe";
            UsuarioVacio.IsSuccess = false;

            return UsuarioVacio;
          }
          else
          {
            _context.Usuarios.Remove(UsuarioToDelete);
            await _context.SaveChangesAsync();

            var usuarioDto = _mapper.Map<UsuarioDto>(UsuarioToDelete);

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
