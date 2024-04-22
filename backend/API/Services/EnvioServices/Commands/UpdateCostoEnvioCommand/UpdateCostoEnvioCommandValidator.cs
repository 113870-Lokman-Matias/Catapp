using FluentValidation;

namespace API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand
{
    public class UpdateCostoEnvioCommandValidator : AbstractValidator<UpdateCostoEnvioCommand>
    {
        public UpdateCostoEnvioCommandValidator()
        {
            RuleFor(p => p.Precio)
                  .NotNull().WithMessage("El precio no puede ser nulo");
        }
    }
}
