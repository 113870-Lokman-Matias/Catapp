using API.Data;
using API.Dtos.UsuarioDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UsuarioServices.Queries.GetUsuariosVendedoresQuery
{
  public class GetUsuariosVendedoresQueryHandler : IRequestHandler<GetUsuariosVendedoresQuery, ListaUsuariosDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    public GetUsuariosVendedoresQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaUsuariosDto> Handle(GetUsuariosVendedoresQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var vendedores = await _context.Usuarios
            .Where(x => x.Activo == true && x.IdRolNavigation.Nombre == "Vendedor")
            .Select(x => new ListaUsuarioDto
            {
              IdUsuario = x.IdUsuario,
              Rol = x.IdRolNavigation.Nombre,
              Nombre = x.Nombre,
              Activo = x.Activo
            })
            .OrderBy(x => x.Nombre)
            .ToListAsync();

        if (vendedores.Count > 0)
        {
          var listaVendedoresDto = new ListaUsuariosDto();
          listaVendedoresDto.Usuarios = vendedores;

          listaVendedoresDto.StatusCode = StatusCodes.Status200OK;
          listaVendedoresDto.ErrorMessage = string.Empty;
          listaVendedoresDto.IsSuccess = true;

          return listaVendedoresDto;
        }
        else
        {
          var listaVendedoresVacia = new ListaUsuariosDto();

          listaVendedoresVacia.StatusCode = StatusCodes.Status404NotFound;
          listaVendedoresVacia.ErrorMessage = "No se han encontrado vendedores";
          listaVendedoresVacia.IsSuccess = false;

          return listaVendedoresVacia;
        }
      }
      catch (Exception ex)
      {
        var listaVendedoresVacia = new ListaUsuariosDto();

        listaVendedoresVacia.StatusCode = StatusCodes.Status400BadRequest;
        listaVendedoresVacia.ErrorMessage = ex.Message;
        listaVendedoresVacia.IsSuccess = false;

        return listaVendedoresVacia;
      }
    }
  }
}
