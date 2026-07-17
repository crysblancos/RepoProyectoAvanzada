namespace Proyecto_Grupo02.EF
{
    public class tbPedidoDetalle
    {
        public int IdDetalle { get; set; }
        public int Cantidad { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public virtual tbPedido Pedido { get; set; }
        public virtual tbProducto Producto { get; set; }
    }
}