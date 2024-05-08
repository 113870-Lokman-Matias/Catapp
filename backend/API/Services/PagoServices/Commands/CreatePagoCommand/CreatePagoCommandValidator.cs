using API.Data;
using AutoMapper.Execution;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Services.PagoServices.Commands.CreatePagoCommand
{
  public class CreatePagoCommandValidator : AbstractValidator<CreatePagoCommand>
  {

    public CreatePagoCommandValidator()
    {
      RuleFor(p => p.Url)
     .NotEmpty().WithMessage("La url no puede estar vacía")
     .NotNull().WithMessage("La url no puede ser nula");

      RuleFor(p => p.Title)
      .NotEmpty().WithMessage("El titulo no puede estar vacío")
      .NotNull().WithMessage("El titulo no puede ser nulo");

      RuleFor(p => p.Quantity)
      .NotNull().WithMessage("La cantidad no puede ser nula")
      .NotEmpty().WithMessage("La cantidad no puede estar vacía");

      RuleFor(p => p.UnitPrice)
      .NotNull().WithMessage("El precio no puede ser nulo")
      .NotEmpty().WithMessage("El precio no puede estar vacío");
    }
  }
}
