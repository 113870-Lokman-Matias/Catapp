using FluentValidation;

namespace API.Services.EnvioServices.Commands.UpdateEnvioCommand
{
    public class UpdateEnvioCommandValidator : AbstractValidator<UpdateEnvioCommand>
    {
        public UpdateEnvioCommandValidator()
        {
            RuleFor(c => c.Habilitado)
                      .NotNull().WithMessage("Habilitado no puede ser nulo");

            RuleFor(p => p.Precio)
                  .NotNull().WithMessage("El precio no puede ser nulo");
        }
    }
}
