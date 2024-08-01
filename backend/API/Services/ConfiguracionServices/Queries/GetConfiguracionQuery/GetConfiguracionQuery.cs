using API.Dtos.ConfiguracionDto;
using MediatR;

namespace API.Services.ConfiguracionServices.Queries.GetConfiguracionQuery
{
    public class GetConfiguracionQuery : IRequest<ConfiguracionDto>
    {
    }
}
