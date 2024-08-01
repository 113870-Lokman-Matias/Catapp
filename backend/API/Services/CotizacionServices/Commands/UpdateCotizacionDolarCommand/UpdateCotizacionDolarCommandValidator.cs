using FluentValidation;

namespace API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand
{
    public class UpdateCotizacionDolarCommandValidator : AbstractValidator<UpdateCotizacionDolarCommand>
    {
        public UpdateCotizacionDolarCommandValidator()
        {
            RuleFor(p => p.Precio)
                  .NotNull().WithMessage("El precio no puede ser nulo");
        }
    }
}
