using API.AnswerBase;

namespace API.Dtos.MetodoPagoDto
{
    public class MetodoPagoDto : RespuestaBase
    {
        public int IdMetodoPago { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Habilitado { get; set; }
        public int Disponibilidad { get; set; }
        public int DisponibilidadCatalogo { get; set; }
    }
}
    


