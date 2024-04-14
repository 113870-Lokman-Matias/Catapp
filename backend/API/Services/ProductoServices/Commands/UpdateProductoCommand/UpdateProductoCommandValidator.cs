using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductoServices.Commands.UpdateProductoCommand
{
  public class UpdateProductoCommandValidator : AbstractValidator<UpdateProductoCommand>
  {
    private readonly CatalogoContext _context;

    public UpdateProductoCommandValidator(CatalogoContext context)
    {
      _context = context;

      RuleFor(p => p.IdProducto)
         .NotEmpty().WithMessage("El id no puede estar vacío")
         .NotNull().WithMessage("El id no puede ser nulo")
         .MustAsync(ProductoExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un producto existente");

      RuleFor(p => p.Nombre)
      .NotEmpty().WithMessage("El nombre no puede estar vacío")
      .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(p => p.Descripcion)
      .NotEmpty().WithMessage("La descripcion no puede estar vacía")
      .NotNull().WithMessage("La descripcion no puede ser nula");

      RuleFor(p => p.IdDivisa)
     .NotEmpty().WithMessage("El id de la divisa no puede estar vacía")
     .NotNull().WithMessage("El id de la divisa no puede ser nula")
     .MustAsync(DivisaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de una divisa existente");

      RuleFor(p => p.Precio)
      .NotNull().WithMessage("El precio no puede ser nulo");

      RuleFor(p => p.PorcentajeMinorista)
     .NotNull().WithMessage("El procentaje de ganancia minorista no puede ser nulo");

      RuleFor(p => p.PorcentajeMayorista)
      .NotNull().WithMessage("El procentaje de ganancia mayorista no puede ser nulo");

      RuleFor(p => p.PrecioMinorista)
      .NotNull().WithMessage("El precio manual minorista no puede ser nulo");

      RuleFor(p => p.PrecioMayorista)
     .NotNull().WithMessage("El precio manual mayorista no puede ser nulo");

      RuleFor(p => p.IdCategoria)
          .NotEmpty().WithMessage("El id de la categoría no puede estar vacío")
          .NotNull().WithMessage("El id de la categoría no puede ser nulo")
          .MustAsync(CategoriaExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de una categoría existente");

      RuleFor(p => p.IdImagen)
               .NotEmpty().WithMessage("El id de la imagen no puede estar vacío")
               .NotNull().WithMessage("El id de la imagen no puede ser nulo");

      RuleFor(p => p.UrlImagen)
          .NotEmpty().WithMessage("La url de la imagen no puede estar vacía")
          .NotNull().WithMessage("La url de la imagen no puede ser nula");

      RuleFor(p => p.Ocultar)
          .NotNull().WithMessage("Ocultar no puede ser nulo");
    }

    private async Task<bool> ProductoExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.IdProducto == id);
      return existe;
    }

    private async Task<bool> CategoriaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Categorias.AnyAsync(p => p.IdCategoria == id);
      return existe;
    }

    private async Task<bool> DivisaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Divisas.AnyAsync(p => p.IdDivisa == id);
      return existe;
    }

  }
}
