namespace Proyecto_Grupo02.EF
{
    public class tbPromoProducto
    {
        public int IdProducto { get; set; }
        public int IdPromocion { get; set; }
        public int IdEstado { get; set; }
        public virtual tbProducto Producto { get; set; }
        public virtual tbPromocion Promocion { get; set; }
        public virtual tbEstado Estado { get; set; }
    }
}