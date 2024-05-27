using API.AnswerBase;

namespace API.Dtos.EnvioDto
{
    public class EnvioDto : RespuestaBase
    {
        public int IdEnvio { get; set; }
        public bool Habilitado { get; set; }
        public float Precio { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }
        public string UltimoModificador { get; set; } = null!;
    }
}
