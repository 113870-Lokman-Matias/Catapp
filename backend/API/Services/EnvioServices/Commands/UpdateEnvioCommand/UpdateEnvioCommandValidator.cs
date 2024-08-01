using FluentValidation;

namespace API.Services.EnvioServices.Commands.UpdateEnvioCommand
{
    public class UpdateEnvioCommandValidator : AbstractValidator<UpdateEnvioCommand>
    {
        public UpdateEnvioCommandValidator()
        {
            RuleFor(c => c.Habilitado)
                      .NotNull().WithMessage("Habilitado no puede ser nulo");

            RuleFor(p => p.Costo)
                  .NotNull().WithMessage("El precio no puede ser nulo");

            RuleFor(p => p.Nombre)
                  .NotEmpty().WithMessage("El nombre no puede estar vacío")
                  .NotNull().WithMessage("El nombre no puede ser nulo");

            RuleFor(c => c.DisponibilidadCatalogo)
                  .NotEmpty().WithMessage("La disponibilidad del catálogo no puede estar vacía")
                  .NotNull().WithMessage("La disponibilidad del catálogo no puede ser nula");
        }
    }
}
