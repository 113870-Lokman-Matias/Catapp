using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Divisa
    {
        public Divisa()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdDivisa { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
