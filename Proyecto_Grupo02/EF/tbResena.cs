using System;

namespace Proyecto_Grupo02.EF
{
    public class tbResena
    {
        public int IdResena { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public int IdEstado { get; set; }
        public virtual tbProducto Producto { get; set; }
        public virtual tbEstado Estado { get; set; }
    }
}