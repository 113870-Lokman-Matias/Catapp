using API.Data;
using API.Dtos.CategoriaDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CategoriaServices.Queries.GetCategoriaByIdQuery
{
    public class GetCategoriaByIdQueryHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCategoriaByIdQuery> _validator;

        public GetCategoriaByIdQueryHandler(CatalogoContext context, IMapper mapper, IValidator<GetCategoriaByIdQuery> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CategoriaDto> Handle(GetCategoriaByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var CategoriaVacia = new CategoriaDto
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage)),
                        IsSuccess = false
                    };

                    return CategoriaVacia;
                }
                else
                {
                    var categoriaId = request.id;

                    var categoria = await _context.Categorias
                    .Where(x => x.IdCategoria == categoriaId)
                    .Select(x => new CategoriaDto
                    {
                        IdCategoria = x.IdCategoria,
                        Nombre = x.Nombre,
                        UrlImagen = x.UrlImagen
                    })
                    .FirstOrDefaultAsync();

                    if (categoria == null)
                    {
                        var CategoriaVacia = new CategoriaDto
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            ErrorMessage = "No existe la categoría",
                            IsSuccess = false
                        };

                        return CategoriaVacia;
                    }
                    else
                    {
                        var categoriaDto = new CategoriaDto
                        {
                            IdCategoria = categoria.IdCategoria,
                            Nombre = categoria.Nombre,
                            UrlImagen = categoria.UrlImagen,
                            StatusCode = StatusCodes.Status200OK,
                            IsSuccess = true,
                            ErrorMessage = ""
                        };

                        return categoriaDto;
                    }
                }
            }
            catch (Exception ex)
            {
                var CategoriaVacia = new CategoriaDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };

                return CategoriaVacia;
            }
        }
    }
}