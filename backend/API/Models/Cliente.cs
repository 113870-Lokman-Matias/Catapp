using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdCliente { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public long Dni { get; set; }
        public long Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? EntreCalles { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
