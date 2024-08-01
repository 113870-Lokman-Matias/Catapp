using API.AnswerBase;

namespace API.Dtos.ClienteDto
{
  public class ClienteMpDto : RespuestaBase
  {
    public string NombreCompleto { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public int TipoCliente { get; set; }
    public string? Direccion { get; set; }
    public string? EntreCalles { get; set; }
    public string IdVendedor { get; set; } = null!;
  }
}
