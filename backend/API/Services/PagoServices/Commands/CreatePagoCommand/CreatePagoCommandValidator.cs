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

      RuleFor(p => p.Productos)
        .NotEmpty().WithMessage("La lista de productos no puede estar vacía")
        .NotNull().WithMessage("La lista de productos no puede ser nula");
      
      RuleFor(p => p.Cliente)
        .NotNull().WithMessage("El cliente no puede ser nulo");

      RuleFor(p => p.CostoEnvio)
        .GreaterThanOrEqualTo(0).WithMessage("El costo de envío no puede ser negativo");
    }
  }
}
