using API.AnswerBase;

namespace API.Dtos.ConfiguracionDto
{
  public class ConfiguracionDto : RespuestaBase
  {
    public int IdConfiguracion { get; set; }
    public string? Direccion { get; set; }
    public string? UrlDireccion { get; set; }
    public string? Horarios { get; set; }
    public string? Cbu { get; set; }
    public string? Alias { get; set; }
    public string Whatsapp { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Facebook { get; set; }
    public string? UrlFacebook { get; set; }
    public string? Instagram { get; set; }
    public string? UrlInstagram { get; set; }
    public int CantidadMayorista { get; set; }
    public string? UrlLogo { get; set; }
  }
}
