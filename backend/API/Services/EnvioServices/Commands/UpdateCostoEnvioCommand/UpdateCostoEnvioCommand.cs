using API.Dtos.EnvioDto;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.EnvioServices.Commands.UpdateCostoEnvioCommand
{
    public class UpdateCostoEnvioCommand : IRequest<EnvioDto>
    {
        [JsonIgnore]
        public int IdEnvio { get; set; }

        public float Precio { get; set; }
    }
}
