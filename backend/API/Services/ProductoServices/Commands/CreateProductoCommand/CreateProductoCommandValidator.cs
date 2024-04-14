using API.Data;
using AutoMapper.Execution;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Services.ProductoServices.Commands.CreateProductoCommand
{
  public class CreateProductoCommandValidator : AbstractValidator<CreateProductoCommand>
  {
    private readonly CatalogoContext _context;

    public CreateProductoCommandValidator(CatalogoContext context)
    {
      _context = context;

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

      RuleFor(p => p.Stock)
      .NotNull().WithMessage("El stock no puede ser nulo");

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

      RuleFor(p => p)
    .MustAsync(ProductoExiste).WithMessage("Este producto ya se encuentra registrado");
    }

    private async Task<bool> ProductoExiste(CreateProductoCommand command, CancellationToken token)
    {
      bool existe = await _context.Productos.AnyAsync(p => p.Nombre == command.Nombre);
      return !existe;
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
