using FluentValidation;

namespace API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand
{
    public class UpdateCotizacionDolarCommandValidator : AbstractValidator<UpdateCotizacionDolarCommand>
    {
        public UpdateCotizacionDolarCommandValidator()
        {
            RuleFor(p => p.Precio)
                  .NotNull().WithMessage("El precio no puede ser nulo");

            RuleFor(c => c.UltimoModificador)
                  .NotEmpty().WithMessage("El nombre del ultimo modificador no puede estar vacío")
                  .NotNull().WithMessage("El nombre del ultimo modificador no puede ser nulo");
        }
    }
}
