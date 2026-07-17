using System;
using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbPromocion
    {
        public int IdPromocion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Descuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int IdEstado { get; set; }
        public virtual tbEstado Estado { get; set; }
        public virtual ICollection<tbPromoProducto> PromoProductos { get; set; }
    }
}