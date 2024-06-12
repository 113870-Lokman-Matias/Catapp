using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Configuracion
    {
        public int IdConfiguracion { get; set; }
        public string? Direccion { get; set; }
        public string? UrlDireccion { get; set; }
        public string? Horarios { get; set; }
        public string? Cbu { get; set; }
        public string? Alias { get; set; }
        public string Whatsapp { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Facebook { get; set; }
        public string? UrlFacebook { get; set; }
        public string? Instagram { get; set; }
        public string? UrlInstagram { get; set; }
        public float MontoMayorista { get; set; }
        public string? UrlLogo { get; set; }
        public string? Codigo { get; set; }
    }
}
