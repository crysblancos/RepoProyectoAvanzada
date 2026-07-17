namespace Proyecto_Grupo02.EF
{
    public class tbCarritoDetalle
    {
        public int IdDetalleCarrito { get; set; }
        public int Cantidad { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public int IdCarrito { get; set; }
        public int IdProducto { get; set; }
        public virtual tbCarrito Carrito { get; set; }
        public virtual tbProducto Producto { get; set; }
    }
}