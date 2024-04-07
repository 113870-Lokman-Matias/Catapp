using API.Data;
using API.Dtos.CategoriaDtos;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentValidation;
using MediatR;

namespace API.Services.CategoriaServices.Commands.DeleteCategoriaCommand
{
  public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, CategoriaDto>
  {
    private readonly CatalogoContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteCategoriaCommand> _validator;

    public DeleteCategoriaCommandHandler(CatalogoContext context, IMapper mapper, IValidator<DeleteCategoriaCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<CategoriaDto> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var CategoriaVacia = new CategoriaDto();

          CategoriaVacia.StatusCode = StatusCodes.Status400BadRequest;
          CategoriaVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          CategoriaVacia.IsSuccess = false;

          return CategoriaVacia;
        }
        else
        {
          var CategoriaToDelete = await _context.Categorias.FindAsync(request.IdCategoria);

          if (CategoriaToDelete == null)
          {
            var CategoriaVacia = new CategoriaDto();

            CategoriaVacia.StatusCode = StatusCodes.Status404NotFound;
            CategoriaVacia.ErrorMessage = "La categoría no existe";
            CategoriaVacia.IsSuccess = false;

            return CategoriaVacia;
          }
          else
          {
            var account = new Account(
                            "dvkgn8snm",
                            "184735187823824",
                            "mVZ1y5A4-JrgcjnfK6q7Eb3sTgk");

            var cloudinary = new Cloudinary(account);

            // Crear un objeto DeletionParams con el publicId de la imagen
            var deletionParams = new DeletionParams(CategoriaToDelete.IdImagen);

            // Eliminar la imagen de Cloudinary
            var deletionResult = cloudinary.Destroy(deletionParams);

            // Verificar si se eliminó correctamente la imagen
            if (deletionResult.StatusCode == System.Net.HttpStatusCode.OK)
            {

              _context.Categorias.Remove(CategoriaToDelete);
              await _context.SaveChangesAsync();

              var categoriaDto = _mapper.Map<CategoriaDto>(CategoriaToDelete);

              categoriaDto.StatusCode = StatusCodes.Status200OK;
              categoriaDto.IsSuccess = true;
              categoriaDto.ErrorMessage = "";

              return categoriaDto;
            }
            else
            {
              // Si falla la eliminación de la imagen en Cloudinary, devuelve un error
              var CategoriaVacia = new CategoriaDto();

              CategoriaVacia.StatusCode = StatusCodes.Status400BadRequest;
              CategoriaVacia.ErrorMessage = deletionResult.Error.Message;
              CategoriaVacia.IsSuccess = false;

              return CategoriaVacia;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var CategoriaVacia = new CategoriaDto();

        CategoriaVacia.StatusCode = StatusCodes.Status400BadRequest;
        CategoriaVacia.ErrorMessage = ex.Message;
        CategoriaVacia.IsSuccess = false;

        return CategoriaVacia;
      }
    }

  }
}
