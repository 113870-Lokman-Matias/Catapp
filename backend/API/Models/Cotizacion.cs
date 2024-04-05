using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Cotizacion
    {
        public int IdCotizacion { get; set; }
        public float Precio { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }
        public string UltimoModificador { get; set; } = null!;
    }
}
