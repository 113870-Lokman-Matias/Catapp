using API.Data;
using API.Dtos.EnvioDto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EnvioServices.Queries.GetEnviosManageQuery
{
    public class GetEnviosManageQueryHandler : IRequestHandler<GetEnviosManageQuery, ListaEnviosDto>
    {
        private readonly CatalogoContext _context;
        private readonly IMapper _mapper;
        public GetEnviosManageQueryHandler(CatalogoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListaEnviosDto> Handle(GetEnviosManageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var formasEntrega = await _context.Envios
                    .Select(x => new ListaEnvioDto
                    {
                        IdEnvio = x.IdEnvio,
                        Habilitado = x.Habilitado,
                        FechaModificacion = x.FechaModificacion,
                        UltimoModificador = x.UltimoModificador,
                        Costo = x.Costo,
                        Nombre = x.Nombre,
                        DisponibilidadCatalogo = x.DisponibilidadCatalogo,
                        Aclaracion = x.Aclaracion
                    })
                    .OrderBy(x => x.Costo)
                    .ToListAsync();

                if (formasEntrega.Count > 0)
                {
                    var listaEnviosDto = new ListaEnviosDto();
                    listaEnviosDto.Envios = formasEntrega;

                    listaEnviosDto.StatusCode = StatusCodes.Status200OK;
                    listaEnviosDto.ErrorMessage = string.Empty;
                    listaEnviosDto.IsSuccess = true;

                    return listaEnviosDto;
                }
                else
                {
                    var listaEnviosVacia = new ListaEnviosDto();

                    listaEnviosVacia.StatusCode = StatusCodes.Status404NotFound;
                    listaEnviosVacia.ErrorMessage = "No se han encontrado formas de entrega";
                    listaEnviosVacia.IsSuccess = false;

                    return listaEnviosVacia;
                }
            }
            catch (Exception ex)
            {
                var listaEnviosVacia = new ListaEnviosDto();

                listaEnviosVacia.StatusCode = StatusCodes.Status400BadRequest;
                listaEnviosVacia.ErrorMessage = ex.Message;
                listaEnviosVacia.IsSuccess = false;

                return listaEnviosVacia;
            }
        }
    }
}
