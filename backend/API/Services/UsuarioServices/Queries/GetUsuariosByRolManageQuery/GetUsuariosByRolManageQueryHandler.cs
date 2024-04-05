using API.Data;
using API.Dtos.UsuarioDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Queries.GetUsuariosByRolManageQuery
{
  public class GetUsuariosByRolManageQueryHandler : IRequestHandler<GetUsuariosByRolManageQuery, ListaUsuariosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetUsuariosByRolManageQuery> _validator;

    public GetUsuariosByRolManageQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetUsuariosByRolManageQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaUsuariosDto> Handle(GetUsuariosByRolManageQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaUsuariosVacia = new ListaUsuariosDto();

          ListaUsuariosVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaUsuariosVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaUsuariosVacia.IsSuccess = false;

          return ListaUsuariosVacia;
        }
        else
        {
          var usuarios = await _context.Usuarios
              .Where(x => x.IdRolNavigation.Nombre == request.role || x.IdRolNavigation.IdRol == 6)
              .Select(x => new ListaUsuarioDto
              {
                IdUsuario = x.IdUsuario,
                Rol = x.IdRolNavigation.Nombre,
                Nombre = x.Nombre,
                Username = x.Username,
                Activo = x.Activo,
                Email = x.Email
              })
              .OrderByDescending(x => x.Rol == "Predeterminado")  // Ordena primero los usuarios con rol "Predeterminado"
              .ThenByDescending(x => x.Activo)  // Luego ordena por el campo "Activo" de forma descendente (activos primero)
              .ThenBy(x => x.Nombre)  // Si hay empate en "Activo", ordena por el campo "Nombre"
              .ToListAsync();


          if (usuarios == null)
          {
            var ListaUsuariosVacia = new ListaUsuariosDto();

            ListaUsuariosVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaUsuariosVacia.ErrorMessage = "No hay usuarios con ese rol";
            ListaUsuariosVacia.IsSuccess = false;

            return ListaUsuariosVacia;
          }
          else
          {
            var listaProductosDto = new ListaUsuariosDto();
            listaProductosDto.Usuarios = usuarios;

            listaProductosDto.StatusCode = StatusCodes.Status200OK;
            listaProductosDto.IsSuccess = true;
            listaProductosDto.ErrorMessage = "";

            return listaProductosDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaUsuariosVacia = new ListaUsuariosDto();

        ListaUsuariosVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaUsuariosVacia.ErrorMessage = ex.Message;
        ListaUsuariosVacia.IsSuccess = false;

        return ListaUsuariosVacia;
      }
    }

  }
}
