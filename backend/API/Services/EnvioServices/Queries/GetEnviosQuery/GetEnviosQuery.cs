using API.Dtos.EnvioDto;
using MediatR;

namespace API.Services.EnvioServices.Queries.GetEnviosQuery
{
    public class GetEnviosQuery : IRequest<ListaEnviosDto>
    {
    }
}
