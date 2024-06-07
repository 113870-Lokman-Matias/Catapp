using FluentValidation;

namespace API.Services.ConfiguracionServices.Commands.UpdateConfiguracionCommand
{
    public class UpdateConfiguracionCommandValidator : AbstractValidator<UpdateConfiguracionCommand>
    {
        public UpdateConfiguracionCommandValidator()
        {
            RuleFor(p => p.Whatsapp)
                   .NotEmpty().WithMessage("El número de WhatsApp no puede estar vacío")
                   .NotNull().WithMessage("El número de WhatsApp no puede ser nulo");

            RuleFor(p => p.CantidadMayorista)
                  .NotNull().WithMessage("La cantidad minima mayorista no puede ser nula");
        }
    }
}
