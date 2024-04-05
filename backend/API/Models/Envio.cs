using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Envio
    {
        public int IdEnvio { get; set; }
        public float Precio { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }
        public string UltimoModificador { get; set; } = null!;
    }
}
