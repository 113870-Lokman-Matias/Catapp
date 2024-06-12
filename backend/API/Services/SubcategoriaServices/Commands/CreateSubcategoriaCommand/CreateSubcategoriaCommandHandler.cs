using API.Data;
using API.Dtos.SubcategoriaDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.SubcategoriaServices.Commands.CreateSubcategoriaCommand
{
  public class CreateSubcategoriaCommandHandler : IRequestHandler<CreateSubcategoriaCommand, SubcategoriaDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateSubcategoriaCommand> _validator;

    public CreateSubcategoriaCommandHandler(CatalogoContext context, IMapper mapper, IValidator<CreateSubcategoriaCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<SubcategoriaDto> Handle(CreateSubcategoriaCommand request, CancellationToken cancellationToken)
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
          var subcategoriaToCreate = _mapper.Map<Subcategoria>(request);
          await _context.AddAsync(subcategoriaToCreate);
          await _context.SaveChangesAsync();

          var subcategoriaDto = _mapper.Map<SubcategoriaDto>(subcategoriaToCreate);

          subcategoriaDto.StatusCode = StatusCodes.Status200OK;
          subcategoriaDto.IsSuccess = true;
          subcategoriaDto.ErrorMessage = string.Empty;

          return subcategoriaDto;
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
