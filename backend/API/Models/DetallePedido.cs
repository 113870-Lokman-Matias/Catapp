using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class DetallePedido
    {
        public int IdDetallePedidos { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string? Aclaracion { get; set; }
        public float PrecioUnitario { get; set; }
        public Guid IdPedido { get; set; }

        public virtual Pedido IdPedidoNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}
