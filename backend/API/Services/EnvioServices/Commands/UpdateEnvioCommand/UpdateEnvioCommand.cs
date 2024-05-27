using API.Dtos.EnvioDto;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.EnvioServices.Commands.UpdateEnvioCommand
{
    public class UpdateEnvioCommand : IRequest<EnvioDto>
    {
        [JsonIgnore]
        public int IdEnvio { get; set; }

        public bool Habilitado { get; set; }

        public float Precio { get; set; }
    }
}
