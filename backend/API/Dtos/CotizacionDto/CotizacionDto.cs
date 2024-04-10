using API.AnswerBase;

namespace API.Dtos.CotizacionDto
{
    public class CotizacionDto : RespuestaBase
    {
        public int IdCotizacion { get; set; }
        public float Precio { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }
        public string UltimoModificador { get; set; } = null!;
    }
}
