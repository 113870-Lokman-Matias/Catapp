using API.Data;
using API.Dtos.UsuarioDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Queries.GetUsuariosQuery
{
  public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQuery, ListaUsuariosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    public GetUsuariosQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaUsuariosDto> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var usuarios = await _context.Usuarios
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
            .ThenBy(x => x.Activo)  // Luego ordena por el campo "Activo" de forma descendente (activos primero)
            .ThenBy(x => x.Rol)  // Luego ordena por el campo "Rol"
            .ThenBy(x => x.Nombre)  // Si hay empate en "Activo", ordena por el campo "Nombre"
            .ToListAsync();

        if (usuarios.Count > 0)
        {
          var listaUsuariosDto = new ListaUsuariosDto();
          listaUsuariosDto.Usuarios = usuarios;

          listaUsuariosDto.StatusCode = StatusCodes.Status200OK;
          listaUsuariosDto.ErrorMessage = string.Empty;
          listaUsuariosDto.IsSuccess = true;

          return listaUsuariosDto;
        }
        else
        {
          var listaUsuariosVacia = new ListaUsuariosDto();

          listaUsuariosVacia.StatusCode = StatusCodes.Status404NotFound;
          listaUsuariosVacia.ErrorMessage = "No se han encontrado usuarios";
          listaUsuariosVacia.IsSuccess = false;

          return listaUsuariosVacia;
        }
      }
      catch (Exception ex)
      {
        var listaUsuariosVacia = new ListaUsuariosDto();

        listaUsuariosVacia.StatusCode = StatusCodes.Status400BadRequest;
        listaUsuariosVacia.ErrorMessage = ex.Message;
        listaUsuariosVacia.IsSuccess = false;

        return listaUsuariosVacia;
      }
    }
  }
}
