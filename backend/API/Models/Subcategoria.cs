using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Subcategoria
    {
        public Subcategoria()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdSubcategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Ocultar { get; set; }
        public int IdCategoria { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; } = null!;
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
