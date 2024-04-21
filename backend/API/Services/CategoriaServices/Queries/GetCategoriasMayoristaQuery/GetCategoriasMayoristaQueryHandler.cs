using API.Data;
using API.Dtos.CategoriaDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CategoriaServices.Queries.GetCategoriasMayoristaQuery
{
  public class GetCategoriasMayoristaQueryHandler : IRequestHandler<GetCategoriasMayoristaQuery, ListaCategoriasDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    public GetCategoriasMayoristaQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaCategoriasDto> Handle(GetCategoriasMayoristaQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var categorias = await _context.Categorias
            .Include(c => c.Productos) // Incluir productos para contar
            .Where(c => c.Ocultar == false && c.Productos.Any(p => p.Ocultar == false && (p.PrecioMayorista > 0 || p.PorcentajeMayorista > 0)))
            .Select(x => new ListaCategoriaDto { 
                IdCategoria = x.IdCategoria,
                Nombre = x.Nombre,
                UrlImagen = x.UrlImagen
            })
            .OrderBy(x => x.Nombre)
            .ToListAsync();

        if (categorias.Count > 0)
        {
          var listaCategoriasDto = new ListaCategoriasDto();
          listaCategoriasDto.Categorias = categorias;

          listaCategoriasDto.StatusCode = StatusCodes.Status200OK;
          listaCategoriasDto.ErrorMessage = string.Empty;
          listaCategoriasDto.IsSuccess = true;

          return listaCategoriasDto;
        }
        else
        {
          var listaCategoriasVacia = new ListaCategoriasDto();

          listaCategoriasVacia.StatusCode = StatusCodes.Status404NotFound;
          listaCategoriasVacia.ErrorMessage = "No se han encontrado categorías";
          listaCategoriasVacia.IsSuccess = false;

          return listaCategoriasVacia;
        }
      }
      catch (Exception ex)
      {
        var listaCategoriasVacia = new ListaCategoriasDto();

        listaCategoriasVacia.StatusCode = StatusCodes.Status400BadRequest;
        listaCategoriasVacia.ErrorMessage = ex.Message;
        listaCategoriasVacia.IsSuccess = false;

        return listaCategoriasVacia;
      }
    }
  }
}
