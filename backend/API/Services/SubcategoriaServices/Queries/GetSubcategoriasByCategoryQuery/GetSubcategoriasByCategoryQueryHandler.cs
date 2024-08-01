using API.Data;
using API.Dtos.SubcategoriaDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryQuery
{
  public class GetSubcategoriasByCategoryQueryHandler : IRequestHandler<GetSubcategoriasByCategoryQuery, ListaSubcategoriasDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetSubcategoriasByCategoryQuery> _validator;

    public GetSubcategoriasByCategoryQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetSubcategoriasByCategoryQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaSubcategoriasDto> Handle(GetSubcategoriasByCategoryQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaSubcategoriasVacia = new ListaSubcategoriasDto();

          ListaSubcategoriasVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaSubcategoriasVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaSubcategoriasVacia.IsSuccess = false;

          return ListaSubcategoriasVacia;
        }
        else
        {
          var subcategorias = await _context.Subcategorias
              .Include(c => c.Productos) // Incluir productos para contar
              .Where(c => c.IdCategoriaNavigation.IdCategoria == request.idCategory && c.Ocultar == false && c.Productos.Any(p => p.Ocultar == false))
              .Select(x => new ListaSubcategoriaDto
              {
                IdSubcategoria = x.IdSubcategoria,
                Nombre = x.Nombre,
                IdCategoria = x.IdCategoria
              })
              .OrderBy(x => x.Nombre)
              .ToListAsync();

          if (subcategorias == null)
          {
            var ListaSubcategoriasVacia = new ListaSubcategoriasDto();

            ListaSubcategoriasVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaSubcategoriasVacia.ErrorMessage = "No hay subcategorias con esa categoría";
            ListaSubcategoriasVacia.IsSuccess = false;

            return ListaSubcategoriasVacia;
          }
          else
          {
            var listaSubcategoriasDto = new ListaSubcategoriasDto();
            listaSubcategoriasDto.Subcategorias = subcategorias;

            listaSubcategoriasDto.StatusCode = StatusCodes.Status200OK;
            listaSubcategoriasDto.IsSuccess = true;
            listaSubcategoriasDto.ErrorMessage = "";

            return listaSubcategoriasDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaSubcategoriasVacia = new ListaSubcategoriasDto();

        ListaSubcategoriasVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaSubcategoriasVacia.ErrorMessage = ex.Message;
        ListaSubcategoriasVacia.IsSuccess = false;

        return ListaSubcategoriasVacia;
      }
    }

  }
}
