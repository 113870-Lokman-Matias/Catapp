namespace API.Dtos.MetodoPagoDto
{
  public class ListaMetodoPagoDto
  {
    public int IdMetodoPago { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Habilitado { get; set; }
    public int Disponibilidad { get; set; }
   }
}
