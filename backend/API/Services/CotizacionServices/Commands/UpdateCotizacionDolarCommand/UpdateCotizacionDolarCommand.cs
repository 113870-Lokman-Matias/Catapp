using API.Dtos.CotizacionDto;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.CotizacionServices.Commands.UpdateCotizacionDolarCommand
{
    public class UpdateCotizacionDolarCommand : IRequest<CotizacionDto>
    {
        [JsonIgnore]
        public int IdDolar { get; set; }

        public float Precio { get; set; }
    }
}
