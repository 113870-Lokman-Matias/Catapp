using API.AnswerBase;

namespace API.Dtos.PagoDto
{
  public class PagoDto : RespuestaBase
  {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string PreferenceId { get; set; } // Nueva propiedad para almacenar el ID de la preferencia
    }
}