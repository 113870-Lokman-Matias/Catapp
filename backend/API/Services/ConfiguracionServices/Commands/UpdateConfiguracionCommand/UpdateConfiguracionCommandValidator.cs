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

            RuleFor(p => p.MontoMayorista)
                  .NotNull().WithMessage("El monto minimo mayorista no puede ser nul0");
        }
    }
}
