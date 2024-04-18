using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class DetallesStock
    {
        public int IdDetallesStock { get; set; }
        public string Accion { get; set; } = null!;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = null!;
        public DateTimeOffset Fecha { get; set; }
        public string Modificador { get; set; } = null!;
        public int IdProducto { get; set; }

        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}
