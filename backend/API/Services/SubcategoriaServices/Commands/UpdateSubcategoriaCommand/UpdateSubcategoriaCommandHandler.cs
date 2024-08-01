using API.Data;
using API.Dtos.SubcategoriaDtos;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.SubcategoriaServices.Commands.UpdateSubcategoriaCommand
{
  public class UpdateSubcategoriaCommandHandler : IRequestHandler<UpdateSubcategoriaCommand, SubcategoriaDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateSubcategoriaCommand> _validator;

    public UpdateSubcategoriaCommandHandler(CatalogoContext context, IMapper mapper, IValidator<UpdateSubcategoriaCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<SubcategoriaDto> Handle(UpdateSubcategoriaCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var SubSubcategoriaVacia = new SubcategoriaDto();

          SubSubcategoriaVacia.StatusCode = StatusCodes.Status400BadRequest;
          SubSubcategoriaVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          SubSubcategoriaVacia.IsSuccess = false;

          return SubSubcategoriaVacia;
        }
        else
        {
          var SubcategoriaToUpdate = await _context.Subcategorias.FindAsync(request.IdSubcategoria);

          if (SubcategoriaToUpdate == null)
          {
            var SubSubcategoriaVacia = new SubcategoriaDto();

            SubSubcategoriaVacia.StatusCode = StatusCodes.Status404NotFound;
            SubSubcategoriaVacia.ErrorMessage = "La subategoría no existe";
            SubSubcategoriaVacia.IsSuccess = false;

            return SubSubcategoriaVacia;
          }
          else
          {
            SubcategoriaToUpdate.Nombre = request.Nombre;
            SubcategoriaToUpdate.Ocultar = request.Ocultar;
            SubcategoriaToUpdate.IdCategoria = request.IdCategoria;

            await _context.SaveChangesAsync();

            var subcategoriaDto = _mapper.Map<SubcategoriaDto>(SubcategoriaToUpdate);

            subcategoriaDto.StatusCode = StatusCodes.Status200OK;
            subcategoriaDto.IsSuccess = true;
            subcategoriaDto.ErrorMessage = "";

            return subcategoriaDto;
          }
        }
      }
      catch (Exception ex)
      {
        var SubcategoriaVacia = new SubcategoriaDto();

        SubcategoriaVacia.StatusCode = StatusCodes.Status400BadRequest;
        SubcategoriaVacia.ErrorMessage = ex.Message;
        SubcategoriaVacia.IsSuccess = false;

        return SubcategoriaVacia;
      }
    }

  }
}
