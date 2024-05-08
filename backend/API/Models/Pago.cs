using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Pago
{
  public string Title { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public string PreferenceId { get; set; } // Propiedad para almacenar el ID de la preferencia
}
