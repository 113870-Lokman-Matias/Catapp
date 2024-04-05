using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TiposPedido
    {
        public TiposPedido()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdTipoPedido { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
