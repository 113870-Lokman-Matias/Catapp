﻿using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Envio
    {
        public Envio()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdEnvio { get; set; }
        public bool Habilitado { get; set; }
        public float Costo { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }
        public string UltimoModificador { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int DisponibilidadCatalogo { get; set; }
        public string? Aclaracion { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
