using API.Data;
using API.Dtos.SubcategoriaDtos;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.SubcategoriaServices.Commands.DeleteSubcategoriaCommand
{
  public class DeleteSubcategoriaCommandHandler : IRequestHandler<DeleteSubcategoriaCommand, SubcategoriaDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteSubcategoriaCommand> _validator;

    public DeleteSubcategoriaCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeleteSubcategoriaCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<SubcategoriaDto> Handle(DeleteSubcategoriaCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var SubcategoriaVacia = new SubcategoriaDto();

          SubcategoriaVacia.StatusCode = StatusCodes.Status400BadRequest;
          SubcategoriaVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          SubcategoriaVacia.IsSuccess = false;

          return SubcategoriaVacia;
        }
        else
        {
          var SubcategoriaToDelete = await _context.Subcategorias.FindAsync(request.IdSubcategoria);

          if (SubcategoriaToDelete == null)
          {
            var SubcategoriaVacia = new SubcategoriaDto();

            SubcategoriaVacia.StatusCode = StatusCodes.Status404NotFound;
            SubcategoriaVacia.ErrorMessage = "La categoría no existe";
            SubcategoriaVacia.IsSuccess = false;

            return SubcategoriaVacia;
          }
          else
          {
              _context.Subcategorias.Remove(SubcategoriaToDelete);
              await _context.SaveChangesAsync();

              var subcategoriaDto = _mapper.Map<SubcategoriaDto>(SubcategoriaToDelete);

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
