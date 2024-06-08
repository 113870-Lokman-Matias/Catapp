using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class MetodosPago
    {
        public MetodosPago()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdMetodoPago { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Habilitado { get; set; }
        public int Disponibilidad { get; set; }
        public int DisponibilidadCatalogo { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
