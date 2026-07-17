using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbEstado
    {
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public virtual ICollection<tbCategoria> Categorias { get; set; }
        public virtual ICollection<tbProducto> Productos { get; set; }
    }
}