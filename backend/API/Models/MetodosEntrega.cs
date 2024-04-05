using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class MetodosEntrega
    {
        public MetodosEntrega()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdMetodoEntrega { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
