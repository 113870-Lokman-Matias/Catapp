using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Pedido
    {
        public Pedido()
        {
            DetallePedidos = new HashSet<DetallePedido>();
        }

        public Guid IdPedido { get; set; }
        public float CostoEnvio { get; set; }
        public DateTimeOffset Fecha { get; set; }
        public bool Verificado { get; set; }
        public string? Direccion { get; set; }
        public string? EntreCalles { get; set; }
        public string? PaymentId { get; set; }
        public int IdTipoPedido { get; set; }
        public int? IdVendedor { get; set; }
        public int IdMetodoPago { get; set; }
        public int IdCliente { get; set; }
        public int IdMetodoEntrega { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; } = null!;
        public virtual Envio IdMetodoEntregaNavigation { get; set; } = null!;
        public virtual MetodosPago IdMetodoPagoNavigation { get; set; } = null!;
        public virtual TiposPedido IdTipoPedidoNavigation { get; set; } = null!;
        public virtual Usuario? IdVendedorNavigation { get; set; }
        public virtual ICollection<DetallePedido> DetallePedidos { get; set; }
    }
}
