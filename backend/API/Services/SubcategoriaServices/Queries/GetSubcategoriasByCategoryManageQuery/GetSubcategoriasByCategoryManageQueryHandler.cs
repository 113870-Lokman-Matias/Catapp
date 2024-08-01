using API.Data;
using API.Dtos.SubcategoriaDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.SubcategoriaServices.Queries.GetSubcategoriasByCategoryManageQuery
{
  public class GetSubcategoriasByCategoryManageQueryHandler : IRequestHandler<GetSubcategoriasByCategoryManageQuery, ListaSubcategoriasDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetSubcategoriasByCategoryManageQuery> _validator;

    public GetSubcategoriasByCategoryManageQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetSubcategoriasByCategoryManageQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaSubcategoriasDto> Handle(GetSubcategoriasByCategoryManageQuery request, CancellationToken cancellationToken)
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
              .Where(x => x.IdCategoriaNavigation.IdCategoria == request.idCategory)
              .Select(x => new ListaSubcategoriaDto
              {
                IdSubcategoria = x.IdSubcategoria,
                Nombre = x.Nombre,
                Ocultar = x.Ocultar,
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
