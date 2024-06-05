using API.Data;
using API.Dtos.CategoriaDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CategoriaServices.Queries.GetCategoriasQuery
{
  public class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, ListaCategoriasDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    public GetCategoriasQueryHandler(CatalogoContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaCategoriasDto> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var categorias = await _context.Categorias
            .Include(c => c.Productos) // Incluir productos para contar
            .Where(c => c.Ocultar == false && c.Productos.Any(p => p.Ocultar == false)) // Filtrar categorías y productos no ocultos
            .Select(x => new ListaCategoriaDto { 
                IdCategoria = x.IdCategoria,
                Nombre = x.Nombre,
                UrlImagen = x.UrlImagen
            })
            .OrderBy(x => x.Nombre)
            .ToListAsync();

         var categoriasManuales = new List<ListaCategoriaDto>
        {
            new ListaCategoriaDto { IdCategoria = 9991, Nombre = "Promociones", UrlImagen = "https://yourfiles.cloud/uploads/4d751b925be85e50416ad5d00ae1369a/promociones.jpg" },
            new ListaCategoriaDto { IdCategoria = 9992, Nombre = "Destacados", UrlImagen = "https://yourfiles.cloud/uploads/df67f39e67d6040d65e038dcf37226b9/destacados.png" }
        };

        // Agregar Promociones primero si hay productos en promoción
        if (_context.Productos.Any(p => p.Ocultar == false && p.EnPromocion == true))
        {
            var promocionesCategoria = categoriasManuales.FirstOrDefault(c => c.IdCategoria == 9991);
            if (!categorias.Any(c => c.IdCategoria == 9991))
            {
                categorias.Insert(0, promocionesCategoria);
            }
        }

        // Agregar Destacados después de Promociones si hay productos destacados
        if (_context.Productos.Any(p => p.Ocultar == false && p.EnDestacado == true))
        {
            var destacadosCategoria = categoriasManuales.FirstOrDefault(c => c.IdCategoria == 9992);
            if (!categorias.Any(c => c.IdCategoria == 9992))
            {
                var indexPromociones = categorias.FindIndex(c => c.IdCategoria == 9991);
                categorias.Insert(indexPromociones + 1, destacadosCategoria);
            }
        }

        // Eliminar categorías manuales ya agregadas condicionalmente
        categoriasManuales.RemoveAll(c => c.IdCategoria == 9991 || c.IdCategoria == 9992);

        // Unir las categorías manuales y las categorías obtenidas de la base de datos
        categoriasManuales.AddRange(categorias);

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
