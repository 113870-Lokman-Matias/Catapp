using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Productos = new HashSet<Producto>();
            Subcategoria = new HashSet<Subcategoria>();
        }

        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string IdImagen { get; set; } = null!;
        public string UrlImagen { get; set; } = null!;
        public bool Ocultar { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Subcategoria> Subcategoria { get; set; }
    }
}
