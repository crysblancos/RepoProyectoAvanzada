using System;

namespace Proyecto_Grupo02.EF
{
    public class tbInventario
    {
        public int IdInventario { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public int Existencias { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int IdProducto { get; set; }
        public int IdSucursal { get; set; }
        public int IdEstado { get; set; }
        public virtual tbProducto Producto { get; set; }
        public virtual tbSucursal Sucursal { get; set; }
        public virtual tbEstado Estado { get; set; }
    }
}