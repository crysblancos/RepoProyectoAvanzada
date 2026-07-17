using System;
using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbCarrito
    {
        public int IdCarrito { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstado { get; set; }
        public virtual tbEstado Estado { get; set; }
        public virtual ICollection<tbCarritoDetalle> Detalles { get; set; }
    }
}