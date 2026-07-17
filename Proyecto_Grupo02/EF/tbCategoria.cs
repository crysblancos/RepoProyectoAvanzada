using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbCategoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdEstado { get; set; }
        public virtual tbEstado Estado { get; set; }
        public virtual ICollection<tbProducto> Productos { get; set; }
    }
}